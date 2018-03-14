using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Infobasis.Data.DataAccess
{
    public class SqlDataSource : ISqlDataSource
    {
		protected const string DEFAULT_TRANSACTION = "__datasourceTransaction";

		private static readonly IDictionary _emptyDictionary = new EmptyDictionary(); // use the same dictionary for every request that has no parameters.

		SqlTransaction _currentTransaction = null;

		//===========================================================================
		protected SqlDataSource(string connectionString)
		{
			_connectionString = connectionString;            
            CommandTimeout = 60;
		}

		//===========================================================================
		public virtual void BeginTransaction()
		{
			if (InTransaction)
				throw new InvalidOperationException("Cannot call BeginTransaction() whilst a transaction is already in progress (InTransaction = true).");

			SqlConnection connection = GetConnection();
			_currentTransaction = connection.BeginTransaction(DEFAULT_TRANSACTION);
		}

		//===========================================================================
		public virtual void CommitTransaction()
		{
			if (!InTransaction)
				throw new InvalidOperationException("Cannot call CommitTransaction() without first calling BeginTransaction() (InTransaction = false).");

			using (_currentTransaction.Connection)
			{
				try
				{
					_currentTransaction.Commit();
				}
				finally
				{
					_currentTransaction = null;
				}
			}
		}

		//===========================================================================
		public virtual void RollbackTransaction()
		{
			if (!InTransaction)
				throw new InvalidOperationException("Cannot call RollbackTransaction() without first calling BeginTransaction() (InTransaction = false).");

			using (_currentTransaction.Connection)
			{
				try
				{
					_currentTransaction.Rollback();
				}
				finally
				{
					_currentTransaction = null;
				}
			}
		}

		//===========================================================================
		public bool InTransaction
		{
			get { return _currentTransaction != null; }
		}

		//===========================================================================
		public string ConnectionString
		{
			get { return _connectionString; }
			set
			{
				_connectionString = value;
				OnConnectionStringChanged();
			}
		}string _connectionString;


		//===========================================================================
		/// <summary>
		/// Command timeout in seconds. Default is 60 seconds.
		/// </summary>
        public int CommandTimeout { get; set; }

		protected virtual void OnConnectionStringChanged()
		{
		}

		// Match '@MyParameter' but not '@@Identity', etc
		static Regex _paramNameRegex = new Regex(@"(?<=[^@\w])@\w+");

		//===========================================================================
		// Extracts the SQL parameters (e.g. "@paramName") from the SQL statement.
		static StringCollection getParameterNamesFromSql(string sql)
		{
			StringCollection parameterNames = new StringCollection();

			foreach (Match m in _paramNameRegex.Matches(sql))
				parameterNames.Add(m.Value);

			return parameterNames;
		}

		void ensureConnectionStringIsSet()
		{
			if (_connectionString == null)
				throw new InvalidOperationException("Must set ConnectionString property before executing queries.");
		}

		public static string GetHumanReadableStringFromSqlAndParameters(string sql, params object[] parameters)
		{
			StringCollection paramNames = getParameterNamesFromSql(sql);

			int paramsLength = (parameters == null ? 0 : parameters.Length);

			string message = sql + Environment.NewLine;
			for (int i = 0; i < (paramsLength > paramNames.Count ? paramsLength : paramNames.Count); i++)
			{
				string name = "?";
				string value = null;
				if (i < paramNames.Count)
					name = paramNames[i];
				if (i < paramsLength)
					value = parameters[i] + "";

				message += "  " + i + ". " + name + " = " + value + Environment.NewLine;
			}
			return message;
		}

		//===========================================================================
		IDictionary createParametersDictionary(string sql, object[] parameters)
		{
			int paramsLength = (parameters == null ? 0 : parameters.Length);

			CaseInsensitiveStringCollection paramNames = new CaseInsensitiveStringCollection();
			foreach (string paramName in getParameterNamesFromSql(sql))
			{
				if (!paramNames.Contains(paramName))
					paramNames.Add(paramName);
			}

			// Sanity check - see if we have a parameter name/value mismatch
			if (paramNames.Count != paramsLength)
			{
				string message = "Expected " + paramNames.Count + " parameter(s) but " + paramsLength + " supplied: " + Environment.NewLine;
				message += sql + Environment.NewLine;
				for (int i = 0; i < (paramsLength > paramNames.Count ? paramsLength : paramNames.Count); i++)
				{
					string name = "?";
					string value = null;
					if (i < paramNames.Count)
						name = paramNames[i];
					if (i < paramsLength)
						value = parameters[i] + "";
					message += "  " + i + ". " + name + " = " + value + Environment.NewLine;
				}
				throw new ArgumentException(message, "parameters");
			}

			if (paramsLength == 0)
			{
				return _emptyDictionary; // Save RAM, no need to make a new empty hashtable.
			}

			// Add all unique parameter names to hash with corresponding value from parameters array
			Hashtable paramsHash = new Hashtable();
			for (int i = 0; i < paramsLength; i++)
				paramsHash.Add(paramNames[i], parameters[i]);

			return paramsHash;
			

		}


		//===========================================================================
		// Returns: a new command with parameters filled in - ready to be executed. 
		protected SqlCommand CreateCommand(SqlConnection connection, string sql, params object[] parameters)
		{
			return CreateCommand(connection, sql, createParametersDictionary(sql, parameters));
		}

		protected virtual SqlTransaction CurrentTransaction
		{
			get
			{
				return _currentTransaction;
			}
		}

		//===========================================================================
		// Returns: a new command with parameters filled in - ready to be executed.
		protected SqlCommand CreateCommand(SqlConnection connection, string sql, IDictionary parameters)
		{
			SqlCommand command = new SqlCommand(sql, connection, CurrentTransaction);
			command.CommandTimeout = CommandTimeout;

			if (parameters != null)
			{
				int paramIndex = 0;

				foreach (string paramName in parameters.Keys)
				{
					object paramValue = parameters[paramName];

					if (paramValue == null)
						paramValue = DBNull.Value;

					command.Parameters.AddWithValue(paramName, paramValue);
					paramIndex++;
				}
			}

			traceLog(command);

			return command;
		}

		//#######################################################################
		class CaseInsensitiveStringCollection : CollectionBase
		{
			public int Add(string value)
			{
				return InnerList.Add(value);
			}

			public bool Contains(string value)
			{
				return InnerList.BinarySearch(value, CaseInsensitiveComparer.DefaultInvariant) > -1;
			}

			public string this[int index]
			{
				get { return (string)InnerList[index]; }
				set { InnerList[index] = value; }
			}
		}

		//===========================================================================
		/// <overloads>Executes a SELECT or a stored procedure into a DataTable</overloads>
		/// <summary>
		/// Executes a parameterless SELECT or a stored procedure into a DataTable.
		/// </summary>
		/// 
		/// <param name="sql">A SELECT statement or "EXEC StoredProcedure" </param>
		/// <returns>A DataTable of the results.</returns>
		public DataTable ExecuteTable(string sql)
		{ return ExecuteTable(sql, _emptyDictionary); }
		//---------------------------------------------------------------------------
		/// <summary>
		/// Executes a SELECT or a stored procedure with parameters into a DataTable.
		/// </summary>
		/// 
		/// <param name="sql">A SELECT statement with parameters embedded as "@param" 
		/// or "EXEC StoredProcedure @param1, @param2" </param>
		/// <param name="parameters">The values to pass as the parameters.</param>
		/// <returns>A DataTable of the results.</returns>
		public DataTable ExecuteTable(string sql,
			params object[] parameters)
		//===========================================================================
		{
			return ExecuteTable(sql, createParametersDictionary(sql, parameters));
		}

		public DataTable ExecuteTable(string sql,
			IDictionary parameters)
		//===========================================================================
		{
			DataSet dataSet = ExecuteDataSet(sql, parameters);
			if (dataSet.Tables.Count != 0)
				return dataSet.Tables[0];
			else
				return null;
		}

		//===========================================================================
		/// <overloads>Executes a single-row returning SELECT or a stored procedure 
		/// into a DataRow</overloads>
		/// 
		/// <summary>
		/// Executes a single-row returning SELECT or a stored procedure into a DataRow.
		/// </summary>
		/// 
		/// <param name="sql">A SELECT statement or "EXEC StoredProcedure" </param>
		/// <returns>A DataRow of the first row from the results of the query. 
		/// A null is returned if no record is returned by the query.</returns>
		public DataRow ExecuteRow(string sql)
		{
			return ExecuteRow(sql, _emptyDictionary);
		}
		//---------------------------------------------------------------------------
		/// <summary>
		/// Executes a single-row returning SELECT or a stored procedure into a DataRow.
		/// </summary>
		/// 
		/// <param name="sql">A SELECT statement with parameters embedded as "@param" 
		/// or "EXEC StoredProcedure @param1, @param2" </param>
		/// <param name="parameters">The values to pass as the parameters.</param>
		/// <returns>A DataRow of the first row from the results of the query. 
		/// A null is returned if no record is returned by the query.</returns>
		public DataRow ExecuteRow(string sql, IDictionary parameters)
		{
			DataTable table = ExecuteTable(sql, parameters);
			if (table != null && table.Rows.Count > 0)
				return table.Rows[0];
			else
				return null;
		}

		public DataRow ExecuteRow(string sql, params object[] parameters)
		{
			return ExecuteRow(sql, createParametersDictionary(sql, parameters));
		}

		//===========================================================================
		public DataSet ExecuteDataSet(string sql)
		{
			return ExecuteDataSet(sql, _emptyDictionary);
		}

		//---------------------------------------------------------------------------
		public DataSet ExecuteDataSet(string sql, IDictionary parameters)
		{
			ensureConnectionStringIsSet();

			if (sql == null)
				throw new ArgumentNullException("sql");

			DataSet dataSet = new DataSet();

			SqlConnection connection = GetConnection();
			SqlCommand command = CreateCommand(connection, sql, parameters);

			SqlDataAdapter adapter = new SqlDataAdapter(command);
			try
			{
				// Get data
				adapter.Fill(dataSet);
				return dataSet;
			}
			catch (Exception error)
			{
				throw new SqlDataSourceException(command, error);
			}
			finally
			{
				adapter.Dispose();
				if (!InTransaction)
					connection.Close();
			}
		}

		//===========================================================================
		public DataSet ExecuteDataSet(string sql, params object[] parameters)
		{
			return ExecuteDataSet(sql, createParametersDictionary(sql, parameters));
		}

		//===========================================================================
		public object ExecuteScalar(string sql)
		{
			return ExecuteScalar(sql, _emptyDictionary);
		}
		//---------------------------------------------------------------------------
		public object ExecuteScalar(string sql, params object[] parameters)
		{
			return ExecuteScalar(sql, createParametersDictionary(sql, parameters));
		}
		//---------------------------------------------------------------------------
		public object ExecuteScalar(string sql, IDictionary parameters)
		{
			ensureConnectionStringIsSet();

			SqlConnection connection = GetConnection();

			SqlCommand command = CreateCommand(connection, sql, parameters);

			object value = null;

			try
			{
				value = command.ExecuteScalar();
			}
			catch (Exception error)
			{
				throw new SqlDataSourceException(command, error);
			}
			finally
			{
				if (!InTransaction)
					connection.Close();
			}

			return value;
		}


		//===========================================================================
		public int ExecuteNonQuery(string sql)
		{ return ExecuteNonQuery(sql, _emptyDictionary); }
		//---------------------------------------------------------------------------
		public int ExecuteNonQuery(string sql, params object[] parameters)
		{ return ExecuteNonQuery(sql, createParametersDictionary(sql, parameters)); }
		//---------------------------------------------------------------------------
		public int ExecuteNonQuery(string sql, IDictionary parameters)
		//===========================================================================
		{
			ensureConnectionStringIsSet();

			SqlConnection connection = GetConnection();

			SqlCommand command = CreateCommand(connection, sql, parameters);

			int affected = 0;
			try
			{
				affected = command.ExecuteNonQuery();
				if (!InTransaction)
					connection.Close();
			}
			catch (Exception error)
			{
				throw new SqlDataSourceException(command, error);
			}
			return affected;
		}

		/// <summary>
		/// Indicates whether the specified store procedure exists in the database specified by the connection string.
		/// </summary>
		public bool ProcedureExists(string procedureName)
		{
			return Change.ToBool(ExecuteScalar("SELECT CONVERT(bit, COUNT(1)) FROM sys.procedures WHERE name = @procName",
				procedureName));
		}

		//===========================================================================
		/// <summary>
		/// Constants that represent common Error numbers found in 
		/// <see cref="SqlException.ErrorNumber"/> 
		/// </summary>
		public class ErrorNumbers
		{
			public const int DuplicateKey = 2627;
			public const int UniqueIndexViolation = 2601;
			public const int StringTruncated = 8152;
			public const int TableReferenceConstraint = 547;


			private ErrorNumbers() {/*not createable*/}
		}

		//===========================================================================
		/// <overloads>
		/// Makes strings 'safe' for directly embedding in **string literals** within SQL statements.
		/// </overloads>
		/// <example>
		/// <c>O'Flanagan</c> becomes <c>O''Flanagan</c>.
		/// </example>
		public static string SqlString(object o)
		{ return SqlString(o.ToString()); }
		//---------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		/// <example>
		/// <c>O'Flanagan</c> becomes <c>O''Flanagan</c>.
		/// </example>
		public static string SqlString(string s)
		//===========================================================================
		{
			return s.Replace("'", "''");
		}

        public class SqlDataSourceException : Exception, System.Runtime.Serialization.ISerializable
		{
			public SqlDataSourceException(SqlCommand command, Exception innerException)
				: base("Error \"" + innerException.Message + "\" occurred executing: \n" + getCommandAsText(command), innerException)
			{

			}

			public SqlException SqlException
			{
				get { return InnerException as SqlException; }
			}


			static string getCommandAsText(SqlCommand command)
			{
				StringWriter text = new StringWriter();
				text.WriteLine();

				StringWriter parameterComment = new StringWriter();
				StringWriter parameterDeclarations = new StringWriter();
				for (int i = 0; i < command.Parameters.Count; i++)
				{
					string name = command.Parameters[i].ParameterName;
					string value = null;
					string type = command.Parameters[i].SqlDbType.ToString().ToUpper();

					if (command.Parameters[i].Value is string)
					{
						value = "'" + ((string)command.Parameters[i].Value).Replace("'", "''") + "'";
						type += "(" + ((string)command.Parameters[i].Value).Length + ")";
					}
					else if (command.Parameters[i].Value is DateTime)
					{
						value = "CONVERT(DATETIME, '" + ((DateTime)command.Parameters[i].Value).ToString("yyyy/MM/dd") + "')";
					}
					else if (command.Parameters[i].Value is bool)
					{
						value = ((bool)command.Parameters[i].Value) ? "1" : "0";
					}
					else
					{
						value = command.Parameters[i].Value + "";
					}

					parameterComment.WriteLine("-- {0}: {1} = {2}", i, name, command.Parameters[i].Value + "", value, type);
					parameterDeclarations.WriteLine("DECLARE {1} {4}\n\tSET {1} = {3}", i, name, command.Parameters[i].Value + "", value, type);
				}

				text.WriteLine(parameterComment.ToString());
				text.WriteLine(parameterDeclarations.ToString());
				text.WriteLine(command.CommandText);

				return text.ToString();
			}
		}



		//===========================================================================
		public void ExecuteNonQueryAsync(string sql)
		{
			ExecuteNonQueryAsync(sql, null);
		}
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		public void ExecuteNonQueryAsync(string sql,
			params object[] parameters)
		{
            CommandTimeout = 60;
			ExecuteNonQueryDelegate d = new ExecuteNonQueryDelegate(ExecuteNonQuery);
			AsyncHelper.FireAndForget(d, sql, parameters);
		}

		//===========================================================================
		// Delegate used by ExecuteNonQueryAsync method.
		delegate int ExecuteNonQueryDelegate(string sql,
			params object[] parameters);

        public XmlDocument ExecuteXmlDoc(string rootNodeName, string sql)
        {
            return ExecuteXmlDoc(rootNodeName, sql, _emptyDictionary);

        }

        //===========================================================================
        /// <summary>Executes FOR XML queries to an XmlDocument.</summary>
        /// 
        /// <param name="sql">SQL statement or stored procedure (e.g. <c>"EXEC MyProc"</c>)
        /// to execute. Parameters should be specified using an '@' prefix. e.g. 
        /// <c>"SELECT * FROM table WHERE column=<b>@myParam</b>"</c>.
        /// Multiple statements/procedures can be executed at once by seperating 
        /// the queries using semi-colons ";".
        /// </param>
        /// <param name="parameters">Query parameter values. These should match up with 
        /// parameters in the <b>sql</b> parameter.</param>
        /// <returns>An XmlDocument with a root node &lt;ROOT&gt;.</returns>
        public XmlDocument ExecuteXmlDoc(string rootNodeName, string sql, params object[] parameters)
        {
            return ExecuteXmlDoc(rootNodeName, sql, createParametersDictionary(sql, parameters));
        }
        public XmlDocument ExecuteXmlDoc(string rootNodeName, string sql, IDictionary parameters)
        {
            ensureConnectionStringIsSet();

            if (sql == null)
                throw new ArgumentNullException("sql");

            SqlConnection connection = GetConnection();
            XmlDocument doc = new SerializableXmlDocument();
            XmlReader reader = null;
            SqlCommand command = CreateCommand(connection, sql, parameters);
            try
            {
                try
                {
                    reader = command.ExecuteXmlReader();
                }
                catch (NullReferenceException)
                {
                    // No data returned by query
                    return null;
                }

                //NOTE: we don't use "doc.Load(reader)" as it doesn't work for queries 
                //      that don't return a root element

                XmlNode rootNode = doc.AppendChild(doc.CreateElement(rootNodeName));

                // Load nodes into root
                while (!reader.EOF)
                {
                    XmlNode newNode = doc.ReadNode(reader);
                    if (newNode != null)
                        rootNode.AppendChild(newNode);
                }

                return doc;
            }
            catch (Exception e)
            {
                throw new SqlDataSourceException(command, e);
            }
            finally
            {
                // Tidy up
                if (reader != null)
                    reader.Close();
                if (!InTransaction)
                    connection.Close();
            }

        }

		//===========================================================================
		public SqlDataReader ExecuteReader(string sql)
		{
			return ExecuteReader(sql, null);
		}
		public SqlDataReader ExecuteReader(string sql,
			params object[] parameters)
		{
			return ExecuteReader(CommandBehavior.CloseConnection, sql, parameters);
		}
		public SqlDataReader ExecuteReader(CommandBehavior behaviour,
			string sql)
		{
			return ExecuteReader(behaviour, sql, null);
		}
		public SqlDataReader ExecuteReader(CommandBehavior behaviour,
			string sql, params object[] parameters)
		{
			ensureConnectionStringIsSet();

			SqlCommand command = null;
			SqlConnection connection = GetConnection();
			command = CreateCommand(connection, sql, parameters);

			try
			{
				return command.ExecuteReader(behaviour);
			}
			catch (Exception e)
			{
				throw new SqlDataSourceException(command, e);
			}

		}

		//==========================================================
		protected string GetDefaultValue(string defaultName)
		{
			string defaultValue = null;

			string sql =
				"SELECT c.text FROM sysobjects o JOIN syscomments c on o.id = c.id " +
				"WHERE o.id = object_id(@defaultName) AND o.xtype='D'";

			// returns something like "CREATE DEFAULT SiteNumber AS 1"
			object result = ExecuteScalar(sql, defaultName);
			if (result == null || result == DBNull.Value)
				throw new ApplicationException("Couldn't find '" + defaultName + "' Default in sysobjects/syscomments.");

			string createDefaultText = result.ToString();

			Match match = Regex.Match(createDefaultText, @"\s*CREATE\s+DEFAULT\s+.*?\s+AS\s+(?<DEFAULT_VALUE>\d+)\s*", RegexOptions.IgnoreCase);
			if (match.Groups["DEFAULT_VALUE"].Success)
			{
				try
				{
					defaultValue = match.Groups["DEFAULT_VALUE"].Value;
				}
				catch
				{
					throw new ApplicationException("Couldn't parse an integer from \"" +
						match.Groups["DEFAULT_VALUE"].Value + "\" when looking up '" + defaultName + "' Default.");
				}
			}
			else
				throw new ApplicationException("Couldn't parse '" + defaultName + "' Default from \"" + createDefaultText + "\".");

			return defaultValue;
		}

		//===========================================================================
		// Creates an opened connection
		protected virtual SqlConnection GetConnection()
		{
			if (!InTransaction)
			{
				SqlConnection connection = new SqlConnection(_connectionString);
				connection.Open();
				return connection;
			}
			else
				// If we're in a transaction return the transaction's connection
				return _currentTransaction.Connection;
		}

		//===========================================================================
		static void traceLog(SqlCommand command)
		{
			if (Debugger.IsAttached)
			{
				StringWriter message = new StringWriter();
				getParameterTrace(command, message, Environment.NewLine + "   ");
				Debugger.Log(1, "SQL", "Created SQL Command: " + command.CommandText + Environment.NewLine + "   " + message.ToString() + Environment.NewLine);
			}
		}

		private static void getParameterTrace(SqlCommand command, TextWriter destination, string delimiter)
		{
			bool first = true;

			foreach (SqlParameter p in command.Parameters)
			{
				if (!first)
				{
					destination.Write(delimiter);
				} first = false;

				destination.Write(p.ParameterName);
				destination.Write(" = ");
				switch (p.DbType)
				{
					case DbType.String:
						destination.Write("'" + p.Value.ToString().Replace("'", "''") + "'");
						break;
					default:
						destination.Write(p.Value);
						break;
				}
			}
		}

		// fast dummy collections to represent empty parameter sets. foreach on one of
		// these babies will get a hard-coded 'return 0' or a 'return false' and stop dead - no faffing about in
		// datastructures at all.

		private class EmptyDictionary : IDictionary
		{
			private static readonly ICollection _keyList = ArrayList.ReadOnly(new ArrayList(0)); // a readonly, guaranteed empty arraylist
			private static readonly ICollection _valueList = ArrayList.ReadOnly(new ArrayList(0)); // another readonly, guaranteed empty arraylist

			#region IDictionary Members

			public void Add(object key, object value)
			{
				throw new InvalidOperationException("The collection is readonly.");
			}

			public void Clear()
			{
				throw new InvalidOperationException("The collection is readonly.");
			}

			public bool Contains(object key)
			{
				return false;
			}

			public IDictionaryEnumerator GetEnumerator()
			{
				return new EmptyDictionaryEnumerator();
			}

			public bool IsFixedSize
			{
				get { return true; }
			}

			public bool IsReadOnly
			{
				get { return true; }
			}

			public ICollection Keys
			{
				get { return _keyList; }
			}

			public void Remove(object key)
			{
				throw new InvalidOperationException("The collection is readonly.");
			}

			public ICollection Values
			{
				get { return _valueList; }
			}

			public object this[object key]
			{
				get
				{
					return null;
				}
				set
				{
					throw new InvalidOperationException("The collection is readonly.");
				}
			}

			#endregion

			#region ICollection Members

			public void CopyTo(Array array, int index)
			{
				// no-op
			}

			public int Count
			{
				get { return 0; }
			}

			public bool IsSynchronized
			{
				get { return true; }
			}

			public object SyncRoot
			{
				get { return new object(); } // we're threadsafe, by definition. Let 'em think they've got an exclusive lock, and ignore contention 
			}

			#endregion

			#region IEnumerable Members

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new EmptyDictionaryEnumerator();
			}


			#endregion
		}
		private class EmptyDictionaryEnumerator : IDictionaryEnumerator
		{
			bool _moved = false;

			#region IDictionaryEnumerator Members

			// we just force Current to be accessed, in a way that satisfies the compiler that type safety will be upheld.
			// Of course, all 'Current' will do is throw an exception...

			public DictionaryEntry Entry
			{
				get { return (DictionaryEntry)Current; }
			}

			public object Key
			{
				get { return Entry.Key; }
			}

			public object Value
			{
				get { return Entry.Value; }
			}

			#endregion

			#region IEnumerator Members

			public object Current
			{
				get { throw new InvalidOperationException(_moved ? "Enumeration already finished." : "Enumeration has not started. Call MoveNext."); }
			}

			public bool MoveNext()
			{
				_moved = true;
				return false;
			}

			public void Reset()
			{
				_moved = false;
			}

			#endregion
		}

    }
}

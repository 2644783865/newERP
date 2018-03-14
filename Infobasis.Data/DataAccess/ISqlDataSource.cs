using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Infobasis.Data.DataAccess
{
    public interface ISqlDataSource
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        bool InTransaction { get; }
        string ConnectionString { get; set; }
        /// <summary>
        /// 执行时间，默认60秒
        /// </summary>
        int CommandTimeout { get; set; }
        DataTable ExecuteTable(string sql);
        DataTable ExecuteTable(string sql, params object[] parameters);
        DataTable ExecuteTable(string sql, IDictionary parameters);

        DataRow ExecuteRow(string sql);
        DataRow ExecuteRow(string sql, IDictionary parameters);
        DataRow ExecuteRow(string sql, params object[] parameters);

        DataSet ExecuteDataSet(string sql);
        DataSet ExecuteDataSet(string sql, IDictionary parameters);
        DataSet ExecuteDataSet(string sql, params object[] parameters);

        object ExecuteScalar(string sql);
        object ExecuteScalar(string sql, params object[] parameters);
        object ExecuteScalar(string sql, IDictionary parameters);

        int ExecuteNonQuery(string sql);
        int ExecuteNonQuery(string sql, params object[] parameters);
        int ExecuteNonQuery(string sql, IDictionary parameters);

        bool ProcedureExists(string procedureName);

        void ExecuteNonQueryAsync(string sql);
        void ExecuteNonQueryAsync(string sql, params object[] parameters);

        XmlDocument ExecuteXmlDoc(string rootNodeName, string sql);
        XmlDocument ExecuteXmlDoc(string rootNodeName, string sql, params object[] parameters);
        XmlDocument ExecuteXmlDoc(string rootNodeName, string sql, IDictionary parameters);

        SqlDataReader ExecuteReader(string sql);
        SqlDataReader ExecuteReader(string sql, params object[] parameters);
        SqlDataReader ExecuteReader(CommandBehavior behaviour, string sql);
        SqlDataReader ExecuteReader(CommandBehavior behaviour, string sql, params object[] parameters);

    }
}

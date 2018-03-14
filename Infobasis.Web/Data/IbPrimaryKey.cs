using Infobasis.Web.Util.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Infobasis.Web.Data
{
    //#########################################################################
    class IbPrimaryKeyConverter : TypeConverter
    {
        //=======================================================================
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        //=======================================================================
        public override object ConvertFrom(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value)
        {
            return IbPrimaryKey.Parse(value.ToString());
        }

    }

    //#########################################################################
    [Serializable, TypeConverter(typeof(IbPrimaryKeyConverter)), DebuggerDisplay("{ID}")]
    public struct IbPrimaryKey
    {
        /// <summary>
        /// The separator used by <see cref="ToString"/> and <see cref="IbPrimaryKey.Parse"/>.
        /// </summary>
        public const char SeparatorChar = '_';

        /// <summary>
        /// The ID value.
        /// </summary>
        public int ID;

        /// <summary>
        /// Reporesents an Empty/uninitialised IbPrimaryKey.
        /// </summary>
        public static readonly IbPrimaryKey Empty = new IbPrimaryKey(0);

        //=======================================================================
        /// <summary>
        /// Creates a new instance of IbPrimaryKey with the values specified.
        /// </summary>
        /// <param name="id">The ID value.</param>
        public IbPrimaryKey(int id)
        {
            ID = id;
        }

        //=======================================================================
        /// <summary>
        /// Creates an IbPrimaryKey from the specified DataRow by looking up the
        /// ID and ST columns specified by columnNamePrefix. e.g. if 
        /// columnNamePrefix = "SKtbSkill" then it will look up "SKtbSkillID" and 
        /// "SKtbSkillST" from the row and instantiate a new IbPrimary key from 
        /// their values.
        /// </summary>
        /// <param name="dataRow">The row to extract the values of ID and ST from.</param>
        /// <param name="columnNamePrefix">The prefix of the coluns to look up the values.</param>
        public IbPrimaryKey(DataRow dataRow)
        {
            if (dataRow == null)
                throw new ArgumentNullException("dataRow");

            int idColumnIndex = dataRow.Table.Columns.IndexOf("ID");

            if (idColumnIndex == -1)
                throw new ArgumentException("DataRow doesn't contain columns '" + "ID'");

            try
            {
                object idValue = dataRow[idColumnIndex];
                if (idValue is DBNull) idValue = 0;
                ID = (int)idValue;
            }
            catch (InvalidCastException)
            {
                string msg = string.Format("Columns specified must contain {0}, not {1}.",
                    typeof(int), dataRow[idColumnIndex].GetType());
                throw new InvalidCastException(msg);
            }
        }


        //=======================================================================
        /// <param name="dataRow">The row to extract the values of ID from.</param>
        public static IbPrimaryKey FromRow(DataRow dataRow)
        {
            if (dataRow == null)
                throw new ArgumentNullException("dataRow");

            int idColumnIndex = dataRow.Table.Columns.IndexOf("ID");

            if (idColumnIndex == -1)
                throw new ArgumentException("DataRow doesn't contain columns ID");

            try
            {
                object idValue = dataRow[idColumnIndex];
                if (idValue is DBNull) idValue = 0;
                return new IbPrimaryKey((int)idValue);
            }
            catch (InvalidCastException)
            {
                string msg = string.Format("Columns specified by must contain {0}, not {1}.",
                    typeof(int), dataRow[idColumnIndex].GetType());
                throw new InvalidCastException(msg);
            }
        }


        //=======================================================================
        public static IbPrimaryKey FromRow(DataRowView dataRowView)
        {
            return IbPrimaryKey.FromRow(dataRowView.Row);
        }


        //=======================================================================
        /// <summary>
        /// Creates an IbPrimaryKey from the specified DataRow by looking up the
        /// ID and ST columns specified by columnNamePrefix. e.g. if 
        /// columnNamePrefix = "SKtbSkill" then it will look up "SKtbSkillID" and 
        /// "SKtbSkillST" from the row and instantiate a new IbPrimary key from 
        /// their values.
        /// </summary>
        /// <param name="dataRowView">The row to extract the values of ID and ST from.</param>
        /// <param name="columnNamePrefix">The prefix of the coluns to look up the values.</param>
        public IbPrimaryKey(DataRowView dataRowView)
            : this(dataRowView.Row)
        {
        }


        //=======================================================================
        /// <summary>
        /// Translates the string representation of a key into an instance.
        /// </summary>
        /// <param name="key">The string to parse.</param>
        /// <returns>An IbPrimaryKey.</returns>
        public static IbPrimaryKey Parse(string key)
        {
            return Parse(key, false);
        }

        //=======================================================================
        /// <summary>
        /// Translates the string representation of a key into an instance.
        /// </summary>
        /// <param name="key">The string to parse.</param>
        /// <param name="ensureEncrypted">Throws an exception if the input string was not encrypted.</param>
        /// <returns>An IbPrimaryKey.</returns>
        public static IbPrimaryKey Parse(string keyString, bool ensureEncrypted)
        {
            Exception parseException;
            IbPrimaryKey result = parseInternal(keyString, ensureEncrypted, out parseException);
            if (parseException != null)
                throw parseException;
            return result;
        }


        static IbPrimaryKey parseInternal(string keyString, bool ensureEncrypted, out Exception parseException)
        {
            parseException = null;

            if (keyString == null)
            {
                parseException = new ArgumentNullException("key", "Primary key string can't be null.");
                return IbPrimaryKey.Empty;
            }

            if (keyString.Length == 0)
            {
                parseException = new ArgumentException("Primary key string cannot be zero-length string.", "key"); // Isn't it great how consistent .NET is!
                return IbPrimaryKey.Empty;
            }

            bool isEncrypted = keyString.IsValidEncryptedFormat();

            if (Crypto.IsEncryptionEnabled && ensureEncrypted && !isEncrypted)
            {
                parseException = new FormatException("Key \"" + keyString + "\" was expected to be in encrypted format. Use key.ToString(true) to encrypt the key.");
                return IbPrimaryKey.Empty;
            }

            if (isEncrypted)
                keyString = keyString.Decrypt();

            int separatorPos = keyString.IndexOf(SeparatorChar);
            if (separatorPos == -1 || separatorPos == 0 || separatorPos == keyString.Length - 1)
            {
                parseException = new FormatException("Key '" + keyString + "' not in correct format.");
                return IbPrimaryKey.Empty;
            }

            int id, st;
            if (int.TryParse(keyString.Substring(0, separatorPos), out id)
                && int.TryParse(keyString.Substring(separatorPos + 1), out st))
            {
                return new IbPrimaryKey(id); // If all went well, we exit here
            }
            else
            {
                parseException = new FormatException("Key '" + keyString + "' not in correct format.");
                return IbPrimaryKey.Empty;
            }
        }

        /// <summary>
        /// Tries to parse an IbPrimaryKey.
        /// </summary>
        /// <param name="keyString">The string to attempt to parse.</param>
        /// <param name="result">The resulting IbPrimaryKey.</param>
        /// <returns>True if the parse was successful, False otherwise.</returns></returns>
        public static bool TryParse(string keyString, out IbPrimaryKey result)
        {
            Exception parseException;
            result = parseInternal(keyString, false, out parseException);
            return (parseException == null);
        }

        //=======================================================================
        public string ToString(bool encrypt)
        {
            string value = string.Concat(ID.ToString());

            if (encrypt)
                return value.Encrypt();
            else
                return value;
        }
        //=======================================================================
        public override string ToString()
        {
            return ToString(true);
        }

        //=======================================================================
        /// <summary>
        /// Indiactes whether two instances of IbPrimaryKey are equal in value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is IbPrimaryKey)
            {
                IbPrimaryKey other = (IbPrimaryKey)obj;
                return (this.ID == other.ID);
            }
            else
                return false;
        }
        //=======================================================================
        public static bool operator ==(IbPrimaryKey lhs, IbPrimaryKey rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(IbPrimaryKey lhs, IbPrimaryKey rhs)
        {
            return !lhs.Equals(rhs);
        }

        //=======================================================================
        public override int GetHashCode()
        {
            // Rely on String's implementation of GetHashCode()
            return this.ToString(false).GetHashCode();
        }

        public bool IsEmpty
        {
            get { return this.Equals(IbPrimaryKey.Empty); }
        }
    }
}
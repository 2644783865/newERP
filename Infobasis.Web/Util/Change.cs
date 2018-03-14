using System;
using System.Xml;

namespace Infobasis.Web.Util
{
	/// <summary>
	/// Conversion utilities.
	/// </summary>
	public class Change
	{
		private Change(){}

		//=======================================================================
		public static bool ToBool(object o) 
		{
			return ToBool(o, false);
		}
		public static bool ToBool(object o, bool defaultValue) 
		{
			if(o==null || o is DBNull || (o is string && ((string)o).Length==0))
				return defaultValue;
			else 
			{
				object value = o;
				if(o is XmlNode)
					value = ((XmlNode)o).InnerText;
				else if(o is string)
				{
					string str = ((string)o).ToLower();
					if(str == "1" || str == "true")
						return true;
					else if(str == "0" || str == "false")
						return false;
				}
				
				try 
				{
					return Convert.ToBoolean(value);
				}
				catch 
				{
					double doubleVal = ToDouble(value);
					return (doubleVal != 0);
				}
			}
		}
		//=======================================================================
		public static int ToInt(object o) 
		{
			return ToInt(o, 0);
		}
		//=======================================================================
		public static int ToInt(object o, int defaultValue) 
		{
			return (int)ToDouble(o, defaultValue);
		}

		//=======================================================================
		public static double ToDouble(object o)
		{
			return ToDouble(o,0);
		}
		public static double ToDouble(object o, double defaultValue)
		{
			if(o==null || o is DBNull)
				return defaultValue;

			string value = ToString(o);
			if(value.Length==0)
				return defaultValue;

			double result;
			bool success = double.TryParse(value, System.Globalization.NumberStyles.Any, null, out result);
			
			if(success)
				return result;
			else
				return defaultValue;
		}

        //=======================================================================
        public static decimal ToDecimal(object o)
        {
            return ToDecimal(o, 0);
        }

        public static decimal ToDecimal(object o, decimal defaultValue)
        {
            if (o == null || o is DBNull)
                return defaultValue;

            string value = ToString(o);
            if (value.Length == 0)
                return defaultValue;

            decimal result;
            bool success = decimal.TryParse(value, System.Globalization.NumberStyles.Any, null, out result);

            if (success)
                return result;
            else
                return defaultValue;
        }

		//=======================================================================
		public static long ToLong(object o) 
		{
			return ToLong(o, 0);
		}
		
		//=======================================================================
		public static long ToLong(object o, long defaultValue) 
		{
			return (long)ToDouble(o, defaultValue);
		}

		//=======================================================================
		public static short ToShort(object o) 
		{
			return ToShort(o, 0);
		}
		//=======================================================================
		public static short ToShort(object o, short defaultValue) 
		{
			return (short)ToDouble(o, defaultValue);
		}

		//=======================================================================
		public static string ToString(object o) 
		{
			return ToString(o, string.Empty);
		}
		public static string ToString(object o, string defaultValue) 
		{
			if(o==null || o is DBNull)
				return defaultValue;
			else if(o is XmlNode)
				return ((XmlNode)o).InnerText;
			else
				return o.ToString();
		}

		//=======================================================================
		public static DateTime ToDateTime(object o)
		{
			if(o==null || o is DBNull)
				return DateTime.MinValue;
			else 
			{
				object value = o;
				if(o is XmlNode)
					value = ((XmlNode)o).InnerText;

				try
				{
					return Convert.ToDateTime(value);
				}
				catch
				{
					return DateTime.MinValue;
				}
			}
		}

        public static bool ValueIsChanged(Nullable<int> value1, Nullable<int> value2)
        {
            if (!value1.HasValue && !value2.HasValue)
                return false;
            else if (value1.HasValue && value2.HasValue)
            {
                return value1.Value != value2.Value;
            }
            else if (value1.HasValue && !value2.HasValue)
                return true;
            else if (!value1.HasValue && value2.HasValue)
                return true;

            return false;
        }

	}
}

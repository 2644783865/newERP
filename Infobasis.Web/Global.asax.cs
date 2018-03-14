using Infobasis.Data;
using Infobasis.Data.DataAccess;
using Infobasis.Web.Util;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;

namespace Infobasis.Web
{
    public class Global : System.Web.HttpApplication
    {

        public static int TOKENEXPIREDSECONDS = 5 * 24 * 60 * 60; // 5 天
        private static object _configMutex = new object();
        private static object _refreshMutex = new object();
        private static bool _refreshHasRun = false;
        private static XmlDocument _ibConfig;

        #region 属性

        //自动获取配置
        //可以在Config文件中配置
        [IbConfig(Default = true)]
        public static bool TEST_MODEL { get; private set; }

        [IbConfig(Default = true)]
        public static bool DISABLE_REGISTRATION_SMS { get; private set; }

        [IbConfig(Default = "B22A8B3c36E3xC4D3882xA30C5gEBDsE4Ae9BE")]
        public static string SECRETKEY { get; private set; }

        [IbConfig(Default = "~/Uploads/")]
        public static string UploadFolderPath { get; private set; }
        [IbConfig(Default = "/Uploads")]
        public static string UploadFolderVirualPath { get; private set; }

        [IbConfig(Default = "test")]
        public static string SMS_DEFAULT_SIGNUPCODE { get; private set; }

        [IbConfig(Default = "test")]
        public static string DEFAULT_PASSWORD { get; private set; }

        [IbConfig(Default = "http://gw.api.taobao.com/router/rest")]
        public static string SMS_SURL { get; private set; }

        [IbConfig(Default = "23412487")]
        public static string SMS_APPKEY { get; private set; }

        [IbConfig(Default = "8b68ca2389d9036bf581a75a37c28d12")]
        public static string SMS_SECRET { get; private set; }

        [IbConfig(Default = 30)]
        public static int AccountLockoutMinutes { get; private set; }
        [IbConfig(Default = 10)]
        public static int MaxLogonAttempts { get; private set; }
        [IbConfig(Default = false)]
        public static bool SecureFormsLogin { get; private set; }
        [IbConfig(Default = true)]
        public static bool DynamicTraceAllowed { get; private set; }
        [IbConfig(Default = "coreyliuu@163.com")]
        public static string ErrorEmailAddress { get; private set; }
        [IbConfig(Default = "coreyliuu@163.com")]
        public static string SystemEmailAddress { get; private set; }
        [IbConfig(Default = "~/res/images/blank_180.png")]
        public static string Default_User_Portrait_Path { get; private set; }
        [IbConfig]
        public static string WebApiRootUrl { get; private set; }

        public static string DateFormat
        {
            get
            {
                return GetDateFormat(System.Threading.Thread.CurrentThread.CurrentUICulture);
            }
        }

        private static Dictionary<string, string> _dateFormats = new Dictionary<string, string>();

        public static string GetDateFormat(CultureInfo culture)
        {


            string dateFormat;

            if (!_dateFormats.TryGetValue(culture.IetfLanguageTag, out dateFormat))
            {
                dateFormat = culture.DateTimeFormat.LongDatePattern;
                dateFormat = dateFormat.Replace("MMMM", "MMM"); // force abbreviated month names
                dateFormat = Regex.Replace(dateFormat, "y+", "yyyy"); // and 4 digit years
                dateFormat = Regex.Replace(dateFormat, @"\S*ddd+\S*", ""); // and no day-of-week name

                dateFormat = dateFormat.Trim();

                _dateFormats[culture.IetfLanguageTag] = dateFormat;
            }
            return dateFormat;
        }

        public static string SiteRootPath { get; private set; }

        //=======================================================================
        public static string Version
        {
            get
            {
                if (_version == null)
                {
                    string dllFileName = Path.Combine(HttpRuntime.AppDomainAppPath, @"bin\Infobasis.Web.dll");
                    _version = System.Reflection.AssemblyName.GetAssemblyName(dllFileName).Version.ToString();
                }
                return _version;
            }
        } static string _version = null;

        //===========================================================================
        public static bool IsDebugVersion
        {
            get
            {
#if DEBUG
                return true;
#else
				return false;
#endif
            }
        }


        #endregion

        static FileSystemWatcher _ibConfigWatcher;
        private static void _ibConfigWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            _ibConfigWatcher.EnableRaisingEvents = false;
            //Log("\"" + e.FullPath + "\" was changed. Application will restart on next request.");
            HttpRuntime.UnloadAppDomain();
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Application_Start");
            Application["OnlineUserCount"] = 0;
            //预启动 DB
            using (var dbcontext = new InfobasisContext())
            {
                var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());

                string log4netConfigFile = HttpContext.Current.Server.MapPath("~/Config/log4net.xml");
                var logCfg = new FileInfo(log4netConfigFile);
                //XmlConfigurator.Configure();
                XmlConfigurator.ConfigureAndWatch(logCfg);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // 这种统计在线人数的做法会有一定的误差
            Application.Lock();
            Application["OnlineUserCount"] = (int)Application["OnlineUserCount"] + 1;
            Application.UnLock();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!_refreshHasRun)
                Refresh();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

            if (Server.GetLastError() == null)
                return;

            // Store the error away and clear it
            Context.Items["LastError"] = Server.GetLastError();
            Server.ClearError();

            // Hand over to Error page
            try
            {
                Server.Execute("~/Error.aspx");
            }
            catch (Exception ex)
            {
                // Oh dear there was an error in Error.aspx!
                try
                {
                    Response.StatusCode = 500;
                }
                catch (Exception) { }

                Response.Write("<h1>Error</h1>");
                if (Global.DynamicTraceAllowed)
                {
                    Response.Write("<pre>");
                    Response.Write(StringUtil.HtmlEncode(ex.ToString()));
                    Response.Write("</pre>");
                    try
                    {
                        Response.Write("<h1>Last Error</h1>");
                        Response.Write("<pre>");
                        Response.Write(StringUtil.HtmlEncode(Context.Items["LastError"] + string.Empty));
                        Response.Write("</pre>");
                    }
                    catch (Exception) { }
                }
                else
                {
                    Response.Write("<p>发生一个错误, 系统管理员马上会处理，请稍等.</p>");
                }

                try
                {
                    Infobasis.Web.Error.LogAndEmailError("Error executing Error.aspx: " + ex
                        + "\r\n\r\nLast Error: " + Context.Items["LastError"]);
                }
                catch { }
            }

            // So that no more page content appended to the response
            Response.End();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["OnlineUserCount"] = (int)Application["OnlineUserCount"] - 1;
            Application.UnLock();




        }

        protected void Application_End(object sender, EventArgs e)
        {
            var context = HttpContext.Current.Items["__InfobasisContext"] as InfobasisContext;
            if (context != null)
            {
                context.Dispose();
            }

            var contextUnitofWork = HttpContext.Current.Items["__InfobasisUnitOfWork"] as UnitOfWork;
            if (contextUnitofWork != null)
            {
                contextUnitofWork.Dispose();
            }
        }

        public static void Refresh()
        {
            lock (_refreshMutex)
            {
                if (_refreshHasRun)
                    return;

                string startupLogMessage = "Application " + HttpRuntime.AppDomainAppId + " at \"" + HttpRuntime.AppDomainAppPath + "\" starting up. \r\n";
#if DEBUG
                startupLogMessage += "Debug";
#else
			    startupLogMessage += "Release";
#endif
                startupLogMessage += " version " + Version + ".";
                //Log(startupLogMessage);

                string ibConfigPath = HttpContext.Current.Server.MapPath("~/Config/IB.config");
                if (!File.Exists(ibConfigPath))
                    throw new FileNotFoundException("在\"" + ibConfigPath + "\"找不到 IB.config 文件", ibConfigPath);
                _ibConfig = new XmlDocument();
                try
                {
                    _ibConfig.Load(ibConfigPath);
                }
                catch (XmlException error)
                {
                    throw new ApplicationException("解析 IB.config 文件错误 at line " + error.LineNumber + ", column " + error.LinePosition + ". QF.config must be a valid XML document."
                        + " Note: XML is case-sensitive; all tags must be </closed> or <self-closing/>; any special XML characters such as '& < >' must be XML-encoded properly ('&amp;', '&lt;', '&gt;' respectively)."
                        + " More information: " + error.Message,
                        error);
                }

                // Watch for any changes to Config/*.config - e.g. QF.config etc
                if (_ibConfigWatcher == null)
                {
                    _ibConfigWatcher = new FileSystemWatcher(Path.GetDirectoryName(ibConfigPath), "*.config");
                    _ibConfigWatcher.Changed += new FileSystemEventHandler(_ibConfigWatcher_Changed);
                    _ibConfigWatcher.Renamed += new RenamedEventHandler(_ibConfigWatcher_Changed);
                    _ibConfigWatcher.EnableRaisingEvents = true;
                }

                initStaticProperties<IbConfigAttribute>(toDictionary(Config));
                _refreshHasRun = true;
            }
        }

        public static NameValueCollection Config
        {
            get
            {
                if (__config == null)
                {
                    lock (_configMutex)
                    {
                        __config = new NameValueCollection();
                        if (_ibConfig != null)
                        {
                            foreach (XmlElement node in _ibConfig.SelectNodes("/Settings/Setting"))
                            {
                                string name = node.Attributes["Name"].Value;
                                string value = node.Attributes["Value"].Value;

                                // check for duplicates
                                if (__config[name] == null)
                                    __config.Add(name, value);
                                else
                                    throw new ApplicationException("Error in IB.config file. \"<Setting Name='" + name + "' .../>\" is specified more than once. Please remove duplicate entries.");
                            }
                        }
                    }
                }
                return __config;
            }
        } static NameValueCollection __config;

        static void initStaticProperties<TAttr>(Dictionary<string, object> values) where TAttr : SettingSourceAttribute
        {
            var props =
                from propertyInfo in typeof(Global).GetProperties(BindingFlags.Public | BindingFlags.Static)
                let attribute = propertyInfo.GetCustomAttributes(false).OfType<TAttr>().FirstOrDefault()
                where propertyInfo.CanWrite
                    && attribute != null
                select new
                {
                    Name = attribute.SourceAttributeName ?? propertyInfo.Name,
                    Type = propertyInfo.PropertyType,
                    Info = propertyInfo,
                    Default = attribute.Default
                };

            foreach (var property in props)
            {
                object value = property.Default;

                object configValue;
                if (values.TryGetValue(property.Name, out configValue))
                    if (!(configValue == null || configValue is DBNull))
                        value = configValue;

                if (value != null)
                {
                    try
                    {
                        value = changeType(value, property.Type);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Error setting property " + property.Name + ": Can't convert from " + configValue.GetType() + " \"" + configValue + "\" to " + property.Type, ex);
                    }

                    //Debug.WriteLine("* " + property.Name + " = " + value);
                    property.Info.SetValue(null, value, null);
                }
            }
        }

        static object changeType(object value, Type type)
        {
            // MS failed to provide a converter for TimeZoneInfo.
            if (type == typeof(TimeZoneInfo))
            {
                return TimeZoneInfo.FindSystemTimeZoneById(Convert.ToString(value));
            }

            try
            {
                // Uses IConvertible...
                return Convert.ChangeType(value, type);
            }
            catch (Exception)
            {
                // Try System.ComponentModel.TypeConverter
                var converter = TypeDescriptor.GetConverter(type);
                return converter.ConvertFrom(value);
            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        class SettingSourceAttribute : Attribute
        {
            public object Default { get; set; }
            public string SourceAttributeName { get; set; }
        }

        class IbConfigAttribute : SettingSourceAttribute { }

        static Dictionary<string, object> toDictionary(DataRow row)
        {
            return row.Table.Columns.Cast<DataColumn>().ToDictionary(
                column => column.ColumnName, // Key
                column => row[column],       // Value
                StringComparer.InvariantCultureIgnoreCase);
        }

        static Dictionary<string, object> toDictionary(NameValueCollection collection)
        {
            return collection.Keys.Cast<string>().ToDictionary(
                key => key,             // Key
                key => (object)collection[key], // Value
                StringComparer.InvariantCultureIgnoreCase);
        }

    }
}
using Infobasis.Data.DataAccess;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace Infobasis.Web.Data
{
    /// <summary>
    /// Provides static methods for logging .
    /// </summary>
    public class Authentication
    {
        private Authentication() {/*not creatable*/}

        /// <summary>
        /// Singleton LogonCount.For[name] provides count on number of times a logon was attempted
        /// </summary>
        public class LogonCount
        {
            private LogonCount() { ;}
            public static LogonCount For = new LogonCount();
            public int this[string userName]
            {
                get
                {
                    string key = "LogonCount:" + userName.Trim();

                    object countObj = HttpContext.Current.Cache[key];
                    if (countObj == null)
                        return 0;
                    else
                        return (int)countObj;
                }
                set
                {
                    string key = "LogonCount:" + userName.Trim();

                    if (value == 0)
                        HttpContext.Current.Cache.Remove(key);
                    else
                    {
                        HttpContext.Current.Cache.Insert(key, value, null,
                            System.Web.Caching.Cache.NoAbsoluteExpiration,
                            new TimeSpan(0, Global.AccountLockoutMinutes, 0));
                    }
                }
            }
        }

        public static bool FormsLogin(string companyCode, string userName)
        {
            IInfobasisDataSource eds = InfobasisDataSource.Create();
            string sql = "Select Password from SYtbUser where Name = @username";
            object[] parameters = new object[] { userName };
            object password = eds.ExecuteScalar(sql, parameters);

            return FormsLogin(companyCode, userName, password == DBNull.Value ? null : password.ToString(), null);
        }

        public static bool FormsLogin(bool isGuest, string companyCode, string userName)
        {
            return FormsLogin(companyCode, userName, null, null, isGuest);
        }

        public static bool FormsLogin(string companyCode, string userName, string password, string returnUrl)
        {
            return FormsLogin(companyCode, userName, password, returnUrl, false);
        }

        public static bool FormsLogin(string companyCode, string userName, string password, string returnUrl, bool isGuest)
        {
            HttpContext httpContext = HttpContext.Current;

            // Just incase they've managed to get to the login page whilst 
            // already logged on as someone else
            //httpContext.Session.Abandon();

            UserInfo user = UserInfo.VerifyUser(companyCode, userName, password, isGuest);
            string nameKey = companyCode + "," + userName;

            if (user != null)
            {
                httpContext.Session.Abandon();
                httpContext.Session["IbUserInfo"] = user;
                httpContext.Response.AppendToLog("[UserInfo.VerifyUser:SUCCESS('" + nameKey + "')]");
                LogonCount.For[nameKey] = 0; //reset logon attempts

                SetAuthCookie(user.CompanyID, userName);
                createJWTToken(user);
                saveCompanyInfoCookies(companyCode, userName);

                // Send them to requested URL if any, or their Home page
                if (returnUrl != null)
                    httpContext.Response.Redirect(returnUrl, true);
                else
                    // Home.aspx decides where to go initially. We need to 
                    // redirect because otherwise FormsAuth won't have 
                    // initialised properly
                    httpContext.Response.Redirect("~/Main.aspx", true);

                return true; // this actually never gets called because Response.Redirect exits
            }
            else
            {
                httpContext.Response.AppendToLog("[UserInfo.VerifyUser:FAIL('" + nameKey + "')]");

                logLoginFailure(companyCode, userName);

                if (Global.MaxLogonAttempts > 0
                    && ++LogonCount.For[nameKey] >= Global.MaxLogonAttempts)
                {
                    lockOutAccount(companyCode, userName);
                }
                return false;
            }
        }

        private static bool updateUserTokenInfo(int userID, string token)
        {
            IInfobasisDataSource db = InfobasisDataSource.Create();
            db.ExecuteNonQueryAsync("UPDATE SYtbUser SET Token = @Token, TokenCreationDate = @TokenCreationDate WHERE ID = @ID",
                token,
                DateTime.Now,
                userID);

            return true;
        }

        private static void createJWTToken(UserInfo user)
        {
            var payload = new Dictionary<string, object>()
                {
                    { "id", user.ID },
                    { "companyID", user.CompanyID },
                    { "userName", user.Name},
                    { "Email", user.Email}
                };
            var secretKey = Global.SECRETKEY;
            string token = JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);

            if (token == null)
                return;

            updateUserTokenInfo(user.ID, token);
            HttpCookie cookie = new HttpCookie(_default.JWT_COOKIE_KEY, token);
            cookie.Path = HttpRuntime.AppDomainAppVirtualPath;
            cookie.Expires = DateTime.MaxValue;
            //cookie.Expires = System.DateTime.Now.AddDays(1);//设置过期时间  1天
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private static void saveCompanyInfoCookies(string companyCode, string userName)
        {
            HttpCookie cookie = new HttpCookie(_default.USERINFO_COOKIE_KEY);
            cookie.Values["companyCode"] = companyCode;
            cookie.Values["userName"] = userName;
            cookie.Expires = DateTime.MaxValue;
            //cookie.Expires = System.DateTime.Now.AddDays(1);//设置过期时间  1天
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Alternative to FormsAuthentication.SetAuthCookie that: 
        /// * uses FormsAuthTimeout from IB.config if set.
        /// * uses Site's SecureFormsLogin to ensure Auth Cookie is set to be secure (SSL only)
        /// </summary>
        public static void SetAuthCookie(int companyID, string userName)
        {
            const int FORMS_AUTH_TICKET_VERSION = 2;

            int timeout = Infobasis.Web.Util.Change.ToInt(Global.Config["FormsAuthTimeout"], _webConfigFormsAuthTimeout);
            string nameKey = companyID.ToString() + "," + userName;

            // This code is mostly taken from code://System.Web.Security.FormsAuthentication.GetAuthCookie
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                FORMS_AUTH_TICKET_VERSION,
                nameKey,
                /*issueDate*/  DateTime.Now,
                /*expriation*/ DateTime.Now.AddMinutes((double)timeout),
                /*persistent*/ false,
                /*userData*/   string.Empty,
                HttpRuntime.AppDomainAppVirtualPath);
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            if (encryptedTicket == null)
                throw new ApplicationException("FormsAuthentication.Encrypt() failed. Check that the Web.config system.web/machineKey setting is valid.");
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authCookie.HttpOnly = true;
            authCookie.Path = HttpRuntime.AppDomainAppVirtualPath;
            authCookie.Secure = FormsAuthentication.RequireSSL || Global.SecureFormsLogin;
            if (FormsAuthentication.CookieDomain != null)
                authCookie.Domain = FormsAuthentication.CookieDomain;

            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        /// <summary>
        /// Alternative to FormsAuthentication.SignOut that uses same cookie attributes as SetAuthCookie 
        /// (e.g. Global.SecureFormsLogin, Path = HttpRuntime.AppDomainAppVirtualPath, etc)
        /// </summary>
        public static void SignOut()
        {
            HttpContext context = HttpContext.Current;

            string cookieValue = string.Empty;
            if (context.Request.Browser.SupportsEmptyStringInCookieValue == false)
                cookieValue = "NoCookie";

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue);
            cookie.HttpOnly = true;
            cookie.Path = HttpRuntime.AppDomainAppVirtualPath;
            cookie.Expires = new DateTime(1999, 10, 12);
            cookie.Secure = FormsAuthentication.RequireSSL || Global.SecureFormsLogin;
            if (FormsAuthentication.CookieDomain != null)
                cookie.Domain = FormsAuthentication.CookieDomain;

            context.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            context.Response.Cookies.Add(cookie);
        }

        static int _webConfigFormsAuthTimeout = getWebConfigFormsAuthTimeout();
        private static int getWebConfigFormsAuthTimeout()
        {
            Configuration webConfig = WebConfigurationManager.OpenWebConfiguration(HttpRuntime.AppDomainAppVirtualPath/* Same as Request.ApplicationPath*/);
            AuthenticationSection authConfig = (AuthenticationSection)webConfig.GetSection("system.web/authentication");
            return (int)authConfig.Forms.Timeout.TotalMinutes;
        }

        static void logLoginFailure(string companyCode, string userName)
        {
            HttpContext httpContext = HttpContext.Current;
            try
            {
                string sql = "INSERT SYtbErrorLog(CompanyCode, UserID, ErrorMessage, ErrorType) VALUES(@CompanyCode, @UserID, @msg, 'L')";
                string message = string.Format("Failed to log in as user '{0}' from Name {1}.",
                    userName, httpContext.Request.UserHostAddress);

                IInfobasisDataSource eds = InfobasisDataSource.Create();
                eds.ExecuteNonQuery(sql, companyCode, userName, message);
            }
            catch {/* Ignore errors */}
        }

        static void lockOutAccount(string companyCode, string userName)
        {
            clientAlert("账号锁定");

            IInfobasisDataSource eds = InfobasisDataSource.Create();
            int? companyID = eds.ExecuteScalar("SELECT ID FROM SYtbCompany WHERE CompanyCode = @CompanyCode", companyCode) as int?;
            eds.ExecuteNonQuery(
                "UPDATE SYtbUser SET " +
                " AccountLockedUntil = DATEADD(minute, @lockoutMins, GETDATE()) " +
                "WHERE Name = @userName AND CompanyID = @companyID",
                Global.AccountLockoutMinutes, userName, companyID);
        }

        static void clientAlert(string message)
        {
            clientScript("alert(\"" + StringUtil.JSEncode(message) + "\");");
        }

        static void clientScript(string script)
        {
            TextWriter writer = HttpContext.Current.Response.Output;
            writer.WriteLine("<script language='javascript' type='text/javascript'>");
            writer.WriteLine("<!--");
            writer.WriteLine(script);
            writer.WriteLine("//-->");
            writer.WriteLine("</script>");
        }
    }
}
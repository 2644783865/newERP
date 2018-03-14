using FineUIPro;
using Infobasis.Data.DataAccess;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web
{
    public partial class _default : PageBase
    {
        private static readonly int MAX_FAIL_COUNT = 3;
        public static readonly string USERINFO_COOKIE_KEY = "userKey";
        public static readonly string JWT_COOKIE_KEY = "JWTToken";

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            AntiCsrf.Current.Enabled = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                firstView();
            }
            DataBind();
        }

        private void LoadData()
        {
            if (GetLoginFailCount() >= MAX_FAIL_COUNT)
            {
                panelCaptcha.Hidden = false;
                InitCaptchaCode();
            }
            else
            {
                panelCaptcha.Hidden = true;
            }
        }

        /// <summary>
        /// 初始化验证码
        /// </summary>
        private void InitCaptchaCode()
        {
            // 创建一个 6 位的随机数并保存在 Session 对象中
            Session["CaptchaImageText"] = GenerateRandomCode();
            imgCaptcha.Text = String.Format("<img src=\"{0}\" />", ResolveUrl("~/captcha/captcha.ashx?w=100&h=26&t=" + DateTime.Now.Ticks));
        }

        /// <summary>
        /// 创建一个 6 位的随机数
        /// </summary>
        /// <returns></returns>
        private string GenerateRandomCode()
        {
            string s = String.Empty;
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                s += random.Next(10).ToString();
            }
            return s;
        }

        protected void imgCaptcha_Click(object sender, EventArgs e)
        {
            InitCaptchaCode();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Session数据判断是否应该验证码
            int failCount = GetLoginFailCount();
            if (failCount >= MAX_FAIL_COUNT)
            {
                if (tbxCaptcha.Text != Session["CaptchaImageText"].ToString())
                {
                    Alert.ShowInTop("验证码错误！");
                    return;
                }
            }

            string companyCode = tbxCompanyCode.Text;
            string userName = tbxUserName.Text;

            if (PerformLogin(companyCode, userName, tbxPassword.Text))
            {
                ClearLoginFailCount();
                LoadData();
                Alert.ShowInTop("成功登录！");

                Response.Redirect("~/Main.aspx");
            }
            else
            {
                PlusLoginFailCount();
                LoadData();
                Alert.ShowInTop("用户名或密码错误！", MessageBoxIcon.Error);
            }
        }
        private string[] getCompanyInfoCookies()
        {
            HttpCookie cookie = Request.Cookies[USERINFO_COOKIE_KEY];
            string[] rtn = new string[2];
            if (cookie != null)
            {
                rtn[0] = cookie["companyCode"];
                rtn[1] = cookie["userName"];
            }
            return rtn;
        }

        public bool PerformLogin(string companyCode, string userName, string password)
        {
            return Authentication.FormsLogin(companyCode, userName, password, Request.QueryString["ReturnUrl"]);
        }

        //======================================================================
        void firstView()
        {
            if (GetLoginFailCount() >= MAX_FAIL_COUNT)
            {
                panelCaptcha.Hidden = false;
                InitCaptchaCode();
            }
            else
            {
                panelCaptcha.Hidden = true;
            }

            string[] companyInfo = getCompanyInfoCookies();
            tbxCompanyCode.Text = companyInfo[0];
            tbxUserName.Text = companyInfo[1];

            if (Request.QueryString["action"] == "logoff")
                logOut();

            string returnUrl = Request.QueryString["ReturnUrl"];

            // If they're already logged on. Or, we're using Windows Auth then send them to their home page
            if (UserInfo.IsLoggedOn)
                Response.Redirect("~/Main.aspx", true);

            // If 'ReturnUrl' is present in the querystring then they've timed out
            bool isFormsAuthTimedOut = (returnUrl != null);
            if (isFormsAuthTimedOut)
            {
                // If they have a non-default ExitUrl then send them there
                string exitUrl = getExitUrl();
                if (exitUrl != null)
                    Response.Redirect(exitUrl, true);
            }
        }

        //======================================================================
        void logOut()
        {
            AntiCsrf.Current.VerifyToken(Request.QueryString["token"]);

            Authentication.SignOut();
            Session.Abandon();

            // 'Delete' optionState cookie used by Skills/Framework
            HttpCookie cookie = Response.Cookies["optionState"];
            cookie.Path = HttpRuntime.AppDomainAppVirtualPath;
            cookie.Expires = DateTime.Now.AddDays(-7);
            cookie.Value = "";

            // Do we have an 'ExitUrl' configured anywhere?
            string exitUrl = getExitUrl();
            if (exitUrl == null)
                exitUrl = "~/";
            Response.Redirect(exitUrl, true);
        }

        /// <summary>
        /// Get URL to page where they can log in again. Returns NULL if there's none configured.
        /// </summary>
        string getExitUrl()
        {
            // Exit url in QueryString?
            string queryStringExitUrl = Request.QueryString["ExitUrl"];
            if (queryStringExitUrl != null)
                return queryStringExitUrl;

            // Exit url in cookie?
            HttpCookie cookieExitUrl = Request.Cookies["ExitUrl"];
            if (cookieExitUrl != null && cookieExitUrl.Value.Length > 0)
                return cookieExitUrl.Value;

            // Exit url in config?
            string configExitUrl = Global.Config["ExitUrl"];
            if (configExitUrl != null && configExitUrl.Length > 0)
                return configExitUrl;

            // Default
            return null;
        }

        #region LoginFailCount

        private int GetLoginFailCount()
        {
            object count = HttpContext.Current.Session["LoginFailCount"];
            if (count == null)
            {
                count = HttpContext.Current.Session["LoginFailCount"] = 0;
            }

            return Convert.ToInt32(count);
        }

        private void PlusLoginFailCount()
        {
            int count = GetLoginFailCount();

            HttpContext.Current.Session["LoginFailCount"] = count + 1;
        }

        private void ClearLoginFailCount()
        {
            HttpContext.Current.Session["LoginFailCount"] = 0;
        }

        #endregion
    }
}
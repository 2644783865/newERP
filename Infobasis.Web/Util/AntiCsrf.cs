using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Infobasis.Web.Util
{
    public class AntiCsrf
    {
        const string ANTI_CSRF_TOKEN_NAME = "__ANTICSRF";
        const string CONTEXT_ITEM_NAME = "Infobasis.Util.AntiCsrf";

        Page _page;

        public static void Attach(Page page)
        {
            if (HttpContext.Current.Items[CONTEXT_ITEM_NAME] is AntiCsrf) return; // enforce idempotency

            AntiCsrf antiCsrf = new AntiCsrf(page);

            HttpContext.Current.Items[CONTEXT_ITEM_NAME] = antiCsrf; 

            page.Init += delegate { antiCsrf.checkAntiCsrfToken(); };
            page.PreRender += delegate { antiCsrf.registerAntiCsrfHiddenField(); };
        }

        private AntiCsrf(Page page)
        {
            _page = page;
            Enabled = true;
        }

        public bool Enabled { get; set; }

        void registerAntiCsrfHiddenField()
        {
            if (Enabled && _page.Response.ContentType.EndsWith("html"))
                _page.ClientScript.RegisterHiddenField(ANTI_CSRF_TOKEN_NAME, this.SecureToken);
        }

        void checkAntiCsrfToken()
        {
            if (shouldBypassTokenCheck())
                return;

            string submittedToken = _page.Request.Form[ANTI_CSRF_TOKEN_NAME];
            VerifyToken(submittedToken);
        }

        public void VerifyToken(string submittedToken)
        {
            if (string.IsNullOrEmpty(submittedToken))
                throw new AntiCsrfException("No Anti CSRF token was supplied." + Enabled);
            if (submittedToken != this.SecureToken)
                throw new AntiCsrfException("Anti CSRF token value was '" + submittedToken + "', but should have been '" + this.SecureToken + "'.");
        }
        bool shouldBypassTokenCheck()
        {
            return
                !Enabled
                || _page.Request.Headers["X-Requested-With"] == "XMLHttpRequest" // Ajax request
                || _page.Request.HttpMethod != "POST";
        }

        public string SecureToken
        {
            get
            {
                HttpCookie cookie = _page.Request.Cookies[ANTI_CSRF_TOKEN_NAME];
                if (cookie != null)
                {
                    return cookie.Value;
                }
                else
                {
                    string tokenValue = getNewToken();
                    cookie = new HttpCookie(ANTI_CSRF_TOKEN_NAME)
                    {
                        Value = tokenValue,
                        HttpOnly = true,
                        Path = HttpRuntime.AppDomainAppVirtualPath
                    };
                    _page.Response.Cookies.Set(cookie);
                    return tokenValue;
                }
            }
        }

        string getNewToken()
        {
            return Guid.NewGuid().ToString("N");
        }


        public static AntiCsrf Current
        {
            get { return HttpContext.Current.Items[CONTEXT_ITEM_NAME] as AntiCsrf; }
        }

        public static Control GenerateAntiCsrfHiddenFieldControl(Page page)
        {
            Attach(page);

            LiteralControl literal = new LiteralControl(string.Format(@"<input type='hidden' name='{0}' value='{1}' />", StringUtil.HtmlAttributeEncode(ANTI_CSRF_TOKEN_NAME), StringUtil.HtmlAttributeEncode(Current.SecureToken)));

            return literal;
        }
    }

    public class AntiCsrfException : HttpException
    {
        public AntiCsrfException(string message)
            : base((int)System.Net.HttpStatusCode.BadRequest, message)
        {
        }
    }
}

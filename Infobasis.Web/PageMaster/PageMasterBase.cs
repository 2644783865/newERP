using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.PageMaster
{
    public class PageMasterBase : MasterPage
    {
        protected System.Web.UI.HtmlControls.HtmlForm MainForm;
        protected System.Web.UI.WebControls.ContentPlaceHolder HeadContent;
        protected System.Web.UI.WebControls.ContentPlaceHolder MainContent;

        public PageMasterBase()
        {
        }

        public ContentPlaceHolder MainContentPlaceHolder
        {
            get { return this.MainContent; }
        }

        public ContentPlaceHolder HeadContentPlaceHolder
        {
            get { return this.HeadContent; }
        }

        public string BaseUrl
        {
            get
            {
                string url = Request.Url.AbsoluteUri;
                return url.Substring(0, url.Length - (Request.Url.Query.Length + Request.PathInfo.Length));
            }
        }
    }
}
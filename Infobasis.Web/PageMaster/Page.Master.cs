using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Infobasis.Web.PageMaster
{
    public partial class Page : PageMasterBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Header.DataBind();
            }
        }

        public HtmlForm Form
        {
            get { return MainForm; }
        }
    }
}
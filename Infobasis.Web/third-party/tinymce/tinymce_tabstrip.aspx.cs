using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace FineUIPro.Examples.aspnet
{
    public partial class tinymce_tabstrip : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HtmlEditor1.Text = "<p>FineUI（专业版）<br>基于 jQuery 的专业 ASP.NET 控件库。<br><br>FineUI的使命<br>创建 No JavaScript，No CSS，No UpdatePanel，No ViewState，No WebServices 的网站应用程序。<br><br>支持的浏览器<br>Chrome、Firefox、Safari、IE 8.0+<br><br>相关链接<br>首页：http://fineui.com/pro/<br>示例：http://fineui.com/demo_pro/<br>更新：http://fineui.com/version_pro/</p>";
                HtmlEditor2.Text = "<p>这是编辑器二的值</p>";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ShowNotify("编辑器一：" + HttpUtility.HtmlEncode(HtmlEditor1.Text) +
                "<br/>" + "编辑器二：" + HttpUtility.HtmlEncode(HtmlEditor2.Text));
        }




    }
}

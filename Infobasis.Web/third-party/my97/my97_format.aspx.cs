using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.Examples.aspnet
{
    public partial class my97_format : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbxStartDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00");
                tbxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd 23");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            labResult.Text = String.Format("开始日期：{0}<br/>结束日期：{1}",
                tbxStartDate.Text,
                tbxEndDate.Text);
        }

    }
}

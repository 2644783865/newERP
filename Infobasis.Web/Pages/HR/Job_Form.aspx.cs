using FineUIPro;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.HR
{
    public partial class Job_Form : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            int id = GetQueryIntValue("id");
            if (id > 0)
            {
                JobRole current = DB.JobRoles.Find(id);
                if (current == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }

                tbxName.Text = current.Name;
                tbxRemark.Text = current.Remark;
                if (current.Level.HasValue)
                {
                    tbxLevel.Text = current.Level.ToString();
                }
                tbxDisplayOrder.Text = current.DisplayOrder.ToString();
                cbxEnabled.Checked = current.IsActive;
            }
        }

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int id = GetQueryIntValue("id");
            JobRole jobrole = null;
            if (id > 0)
            {
                jobrole = DB.JobRoles.Find(id);
            }
            else
            {
                jobrole = new JobRole();
            }
            jobrole.Name = tbxName.Text.Trim();
            jobrole.Remark = tbxRemark.Text.Trim();
            if (!string.IsNullOrEmpty(tbxLevel.Text))
                jobrole.Level = Change.ToInt(tbxLevel.Text);
            jobrole.DisplayOrder = Change.ToInt(tbxDisplayOrder.Text);
            jobrole.IsActive = cbxEnabled.Checked;

            if (id == 0)
            {
                DB.JobRoles.Add(jobrole);
            }
            SaveChanges();

            //FineUIPro.Alert.Show("保存成功！", String.Empty, FineUIPro.Alert.DefaultIcon, FineUIPro.ActiveWindow.GetHidePostBackReference());
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}
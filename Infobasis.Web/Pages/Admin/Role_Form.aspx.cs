using FineUIPro;
using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Admin
{
    public partial class Role_Form : PageBase
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
                PermissionRole current = DB.PermissionRoles.Find(id);
                if (current == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }

                tbxName.Text = current.Name;
                tbxRemark.Text = current.Remark;
                tbxIsActive.Checked = current.IsActive;
                tbxDisplayOrder.Text = current.DisplayOrder.ToString();
                if (current.IsClientAdminRole)
                {
                    tbxIsActive.Enabled = false;
                    tbxIsActive.Checked = true;
                }
            }

        }

        #region Events

        private void SaveItem()
        {
            int id = GetQueryIntValue("id");
            if (id > 0)
            {
                PermissionRole item = DB.PermissionRoles.Find(id);
                item.Name = tbxName.Text.Trim();
                item.Remark = tbxRemark.Text.Trim();
                item.IsActive = tbxIsActive.Checked;
                item.CreateDatetime = DateTime.Now;
            }
            else
            {
                PermissionRole item = new PermissionRole();
                item.Name = tbxName.Text.Trim();
                item.Remark = tbxRemark.Text.Trim();
                item.IsActive = tbxIsActive.Checked;
                item.LastUpdateDatetime = DateTime.Now;

                DB.PermissionRoles.Add(item);
            }
            DB.SaveChanges();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();

            //Alert.Show("添加成功！", String.Empty, ActiveWindow.GetHidePostBackReference());
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion
    }
}
using FineUIPro;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Admin
{
    public partial class User_Form : PageBase
    {
        #region Page_Load

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

            // 初始化用户所属角色
            InitUserRole();
        }


        #region InitUserRole

        private void InitUserRole()
        {
            cblRoles.DataSource = DB.PermissionRoles.Where(item => item.IsActive == true);
            cblRoles.DataBind();
        }
        #endregion


        #endregion

        #region Events


        private void SaveItem()
        {
            int companyID = UserInfo.Current.CompanyID;
            Infobasis.Data.DataEntity.User item = new Infobasis.Data.DataEntity.User();
            item.Name = tbxName.Text.Trim();
            item.Password = PasswordUtil.CreateDbPassword(tbxPassword.Text.Trim());
            item.ChineseName = tbxRealName.Text.Trim();
            item.EmployeeSpellCode = ChinesePinyin.GetPinyin(tbxRealName.Text.Trim());
            item.FirstSpellCode = ChinesePinyin.GetFirstPinyin(tbxRealName.Text.Trim());
            item.Email = tbxEmail.Text.Trim();
            item.Remark = tbxRemark.Text.Trim();
            item.Enabled = cbxEnabled.Checked;
            item.CreateDatetime = DateTime.Now;
            item.CompanyID = companyID;
            item.UserType = UserType.Employee;

            // 添加所有角色
            if (ddbRoles.Values != null)
            {
                item.UserPermissionRoles = new List<UserPermissionRole>();
                foreach (var roleID in ddbRoles.Values)
                {
                    item.UserPermissionRoles.Add(new UserPermissionRole()
                    {
                        UserID = item.ID,
                        CompanyID = companyID,
                        PermissionRoleID = Change.ToInt(roleID),
                        CreateDatetime = DateTime.Now
                    });
                }
            }

            DB.Users.Add(item);
            DB.SaveChanges();

        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            string inputUserName = tbxName.Text.Trim();

            Infobasis.Data.DataEntity.User user = DB.Users.Where(u => u.Name == inputUserName).FirstOrDefault();

            if (user != null)
            {
                Alert.Show("用户 " + inputUserName + " 已经存在！");
                return;
            }

            SaveItem();

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion
    }
}
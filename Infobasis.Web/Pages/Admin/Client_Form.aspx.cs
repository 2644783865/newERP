using FineUIPro;
using Infobasis.Data.DataAccess;
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
    public partial class Client_Form : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCheckDuplicate_Click(object sender, EventArgs e)
        {
            if (checkCompanyCodeAvailable())
            {
                Alert.Show("代号可用", MessageBoxIcon.Success);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (checkCompanyCodeAvailable())
            {
                SaveItem();
            }
        }

        private void SaveItem()
        {
            Infobasis.Data.DataEntity.Company item = new Infobasis.Data.DataEntity.Company();
            item.Name = tbxName.Text.Trim();
            item.CompanyCode = tbxCompanyCode.Text.Trim();
            item.Notes = tbxRemark.Text;
            if (tbxExpiredDatetime.SelectedDate.HasValue)
                item.ExpiredDatetime = tbxExpiredDatetime.SelectedDate.Value;
            item.MaxUsers = Infobasis.Web.Util.Change.ToInt(tbxMaxUsers.Text);
            item.ClientAdminAccount = tbxClientAdminAccount.Text;
            item.CompanyStatus = CompanyStatus.Enabled;
            item.CreateDatetime = DateTime.Now;
            item.CreateByID = UserInfo.Current.ID;
            item.CreateByName = UserInfo.Current.ChineseName;

            string clientAdminPwd = tbxClientAdminAccountPwd.Text.Trim();

            // 添加管理员
            item.Users = new List<Infobasis.Data.DataEntity.User>();
            item.Users.Add(new Infobasis.Data.DataEntity.User()
            {
                CompanyID = item.ID,
                Name = item.ClientAdminAccount,
                ChineseName = "系统管理员",
                IsClientAdmin = true,
                Password = PasswordUtil.CreateDbPassword(clientAdminPwd),
                DefaultPageSize = 20,
                Enabled = true,
                CreateByID = UserInfo.Current.ID,
                CreateByName = UserInfo.Current.ChineseName,
                CreateDatetime = DateTime.Now
            });

            DB.Companys.Add(item);
            DB.SaveChanges();

            int companyID = item.ID;
            IInfobasisDataSource db = InfobasisDataSource.Create();
            int userID = item.Users.FirstOrDefault().ID;
            //需要手动更新，因为CompanyID被直接赋为当前登录人
            db.ExecuteNonQuery("UPDATE SYtbUser SET CompanyID = @CompanyID WHERE ID = @UserID", companyID, userID);           
            db.ExecuteNonQuery("EXEC usp_SY_CreateNewComanyDefaultData @CompanyID, @UserID", companyID, userID);

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());

        }

        private bool checkCompanyCodeAvailable()
        {
            string companyCode = tbxCompanyCode.Text;
            if (string.IsNullOrEmpty(companyCode))
            {
                Alert.Show("请输入代号", MessageBoxIcon.Error);
                return false;
            }
            if (DB.Companys.Where(item => item.CompanyCode == companyCode).Count() > 0)
            {
                Alert.Show("代号已被使用", MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}
using FineUIPro;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.User
{
    public partial class UserSetting : PageBase
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
            tbxUserName.Text = UserInfo.Current.ChineseName;
            ddlGridPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            Infobasis.Data.DataEntity.User user = DB.Users.Find(UserInfo.Current.ID);
            user.ChineseName = tbxUserName.Text;
            user.DefaultPageSize = Change.ToInt(ddlGridPageSize.SelectedValue);
            DB.SaveChanges();

            //PageContext.RegisterStartupScript("top.window.location.reload(false);");

            Alert.ShowInTop("修改配置成功, 需要重新登录系统！", String.Empty, "refreshTopWindow();");

        }

    }
}
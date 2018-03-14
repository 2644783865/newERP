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

namespace Infobasis.Web.Pages.HR
{
    public partial class ChangeHireStatus : PageBase
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
            //btnClose.OnClientClick = ActiveWindow.GetHideReference();
            int eeID = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.User user = DB.Users
                .Where(u => u.ID == eeID).FirstOrDefault();
            if (user == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            labName.Text = user.ChineseName;
            labEECode.Text = user.EmployeeCode;
            DropDownChangeHireStatus.SelectedValue = user.HireStatus.HasValue ? user.HireStatus.Value.ToString() : "0";
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            int eeID = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.User user = DB.Users
                .Where(u => u.ID == eeID).FirstOrDefault();
            if (user == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }

            int newHireStatus = Change.ToInt(DropDownChangeHireStatus.SelectedValue);
            string adjustItemName = "";
            if (user.HireStatus.HasValue && user.HireStatus.Value != newHireStatus)
            {
                user.HireStatus = newHireStatus;
                if (!string.IsNullOrEmpty(tbxRemark.Text))
                    user.Remark = user.Remark + "," + tbxRemark.Text;

                if (newHireStatus == 0)
                    adjustItemName = "再入职";
                else if (newHireStatus == 1)
                    adjustItemName = "离职";

                EmployeeAdjust eeAdjust = new EmployeeAdjust()
                {
                    UserID = user.ID,
                    AdjustItemName = adjustItemName,
                    AdjustDate = DateTime.Now,
                    AllChangeData = user.ToString(),
                    isAdjusted = true,
                    CreateByID = UserInfo.Current.ID,
                    CreateByName = UserInfo.Current.ChineseName,
                    CreateDatetime = DateTime.Now
                };

                DB.EmployeeAdjusts.Add(eeAdjust);
            }

            SaveChanges();
            ShowNotify("修改成功");
            //PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            PageContext.RegisterStartupScript("refreshTopWindow();" + ActiveWindow.GetHideRefreshReference());
        }
    }
}
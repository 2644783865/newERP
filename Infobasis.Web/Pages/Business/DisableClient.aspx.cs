using FineUIPro;
using Infobasis.Web.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Business
{
    public partial class DisableClient : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                InitDropDownDisableClientReason();
            }
        }

        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            int clientID = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.Client client = DB.Clients
                .Where(u => u.ID == clientID).FirstOrDefault();
            if (client == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            labName.Text = client.Name;
            labTel.Text = client.Tel;
            labHousesName.Text = client.HousesName;
            labAddress.Text = client.DecorationAddress;
            labNo.Text = client.ProjectNo;
        }

        #region InitDropDownDisableClientReason
        private void InitDropDownDisableClientReason()
        {
            DataTable table = GetEntityListTable("FD");
            disableReason.DataSource = table;
            disableReason.DataTextField = "Name";
            disableReason.DataValueField = "ID";
            disableReason.DataBind();
            disableReason.Items[0].Selected = true;
        }
        #endregion

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            int clientID = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.Client client = DB.Clients
                .Where(u => u.ID == clientID).FirstOrDefault();
            if (client == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            client.Disabled = true;
            client.DisableDateTime = DateTime.Now;
            client.DisableReasonID = 0;
            client.DisableReasonName = disableReason.SelectedText;
            client.DisableReasonRemark = tbxRemark.Text;
            client.DisableByUserID = UserInfo.Current.ID;
            client.DisableByUserDisplayName = UserInfo.Current.ChineseName;

            SaveChanges();
            ShowNotify("废单成功");
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
        }
    }
}
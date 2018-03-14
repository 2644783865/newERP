using FineUIPro;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Business
{
    public partial class ClientTrace : PageBase
    {
        protected static readonly string DATALIST_ITEM_TEMPLATE = "<div class='leftUserInfo'> <div class='portraitImg'><img src='{0}' /></div> <div>{1}</div> <div>{2}</div> </div> <div class='rightDesc'>{3}</div> ";
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
            int clientID = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.Client client = DB.Clients
                .Include("ClientTraces")
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

            DataListClientTrace.DataSource = client.ClientTraces.OrderByDescending(item => item.CreateDatetime);
            DataListClientTrace.DataBind();
        }

        protected void DataList1_ItemDataBound(object sender, FineUIPro.DataListItemEventArgs e)
        {
            Infobasis.Data.DataEntity.ClientTrace clientTrace = e.DataItem as Infobasis.Data.DataEntity.ClientTrace;
            Infobasis.Data.DataEntity.User user = DB.Users.Find(clientTrace.UserID);
            string userPortraitPath = Global.Default_User_Portrait_Path;
            if (user != null && !string.IsNullOrEmpty(user.UserPortraitPath))
            {
                userPortraitPath = user.UserPortraitPath;
            }
            e.Item.Text = String.Format(DATALIST_ITEM_TEMPLATE,
                ResolveUrl(userPortraitPath),
                clientTrace.UserDisplayName,
                clientTrace.CreateDatetime.ToString(),
                 HtmlEncode(clientTrace.TraceDesc));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int clientID = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.Client client = DB.Clients.Find(clientID);
            if (client == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(HtmlEditorAddTrace.Text))
            {
                ShowNotify("请输入日志！");
                return;
            }
            Infobasis.Data.DataEntity.ClientTrace clientTrace = new Infobasis.Data.DataEntity.ClientTrace()
            {
                ClientID = clientID,
                CreateByID = UserInfo.Current.ID,
                UserID = UserInfo.Current.ID,
                UserDisplayName = UserInfo.Current.ChineseName,
                TraceDesc = HtmlEditorAddTrace.Text
            };
            client.LastTraceDate = DateTime.Now;

            DB.ClientTraces.Add(clientTrace);
            if (SaveChanges())
            {
                ShowNotify("保存成功！");
                PageContext.RegisterStartupScript("window.location.reload(false);");
            }
        }
    }
}
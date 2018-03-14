using FineUIPro;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.CRM
{
    public partial class SmsMgr : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                ddlGridPageSize.SelectedValue = GridSmsSend.PageSize.ToString();
            }
        }

        private void LoadData()
        {
            // 权限检查

            btnSMSTemplateAdd.OnClientClick = Window1.GetShowReference("~/Pages/CRM/SmsTemplateForm.aspx", "新增模版");

            // 每页记录数
            GridSmsSend.PageSize = UserInfo.Current.DefaultPageSize;
            ddlGridPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();

            GridSMSTemplateList.PageSize = UserInfo.Current.DefaultPageSize;
            ddlGridSMSTemplateListPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();

            BindGrid();
            BindGridSMSTemplateList();
            BindSMSGrid();
            BindSendHistoryGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnSMSTemplate_Click(object sender, EventArgs e)
        {
            BindGridSMSTemplateList();
        }

        private void BindGridSMSTemplateList()
        {
            IQueryable<Infobasis.Data.DataEntity.SMSTemplate> q = DB.SMSTemplates; //.Include(u => u.Dept);

            // 在用户名称中搜索
            string searchText = tbxSMSTemplateName.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText) || u.Content.Contains(searchText));
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            GridSMSTemplateList.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.SMSTemplate>(q, GridSMSTemplateList);

            GridSMSTemplateList.DataSource = q;
            GridSMSTemplateList.DataBind();
        }

        private void BindGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.Client> q = DB.Clients; //.Include(u => u.Dept);

            // 在用户名称中搜索
            string searchText = tbxName.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText) || u.QQ.Contains(searchText) || u.WeChat.Contains(searchText)
                    || u.Tel.Contains(searchText));
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            GridSmsSend.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.Client>(q, GridSmsSend);

            GridSmsSend.DataSource = q;
            GridSmsSend.DataBind();
        }

        protected void ddlGridSMSTemplateListPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridSMSTemplateList.PageSize = Convert.ToInt32(ddlGridSMSTemplateListPageSize.SelectedValue);
            BindGridSMSTemplateList();
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridSmsSend.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        protected void ddlGridSendHistoryPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridSendHistory.PageSize = Convert.ToInt32(ddlGridSendHistoryPageSize.SelectedValue);
            BindSendHistoryGrid();
        }

        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
            //ShowNotify("窗体被关闭了。参数：" + (String.IsNullOrEmpty(e.CloseArgument) ? "无" : e.CloseArgument));
        }

        protected void GridSmsSend_PageIndexChange(object sender, GridPageEventArgs e)
        {
            SyncDetails();
            BindGrid();
        }

        private void SyncDetails()
        {
            Dictionary<string, object[]> syncedDetails = ViewState["SyncedDetails"] as Dictionary<string, object[]>;
            if (syncedDetails == null)
            {
                syncedDetails = new Dictionary<string, object[]>();
            }

            List<string> selectedRowIDs = new List<string>(GridSmsSend.SelectedRowIDArray);
            for (int i = 0; i < GridSmsSend.DataKeys.Count; i++)
            {
                object[] rowDataKey = GridSmsSend.DataKeys[i];
                string rowId = rowDataKey[0].ToString();

                if (selectedRowIDs.Contains(rowId))
                {
                    if (!syncedDetails.ContainsKey(rowId))
                    {
                        syncedDetails[rowId] = rowDataKey;
                    }
                }
                else
                {
                    if (syncedDetails.ContainsKey(rowId))
                    {
                        syncedDetails.Remove(rowId);
                    }
                }
            }

            ViewState["SyncedDetails"] = syncedDetails;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            SyncDetails();

            List<string> selectedRowIDs = new List<string>(GridSmsSend.SelectedRowIDArray);

            StringBuilder sb = new StringBuilder();
            List<SMSReceiver> listReceiver = new List<SMSReceiver>();
            
            if (selectedRowIDs.Count > 0)
            {
                Dictionary<string, object[]> syncedDetails = ViewState["SyncedDetails"] as Dictionary<string, object[]>;

                foreach (string selectedRowID in selectedRowIDs)
                {
                    if (syncedDetails.ContainsKey(selectedRowID))
                    {
                        object[] rowDataKey = syncedDetails[selectedRowID];
                        string rowId = rowDataKey[0].ToString();
                        string rowName = rowDataKey[1].ToString();
                        string rowTel = rowDataKey[2].ToString();

                        listReceiver.Add(new SMSReceiver() { ID = Change.ToInt(rowId), Name = rowName, Tel = rowTel });
                    }
                }

                StringBuilder sbTels = new StringBuilder();
                foreach (var v in listReceiver)
                { 
                    sb.Append(v.Name + "(" + v.Tel + "), ");
                    sbTels.Append(v.Tel);
                }

                Window2.Title = "发送短信";
                tbxReceiverNames.Text = sb.ToString();
                tbxReceiverTels.Text = sbTels.ToString();
                Window2.Hidden = false;
            }
            else
            {
                Alert.Show("没有选择任何人");
            }
        }

        protected void GridSmsTemplate_PageIndexChange(object sender, GridPageEventArgs e)
        {
            BindSMSGrid();
        }

        protected void GridSendHistory_Sort(object sender, GridSortEventArgs e)
        {
            //Grid1.SortDirection = e.SortDirection;
            //Grid1.SortField = e.SortField;

            BindSendHistoryGrid();
        }

        protected void GridSmsTemplate_Sort(object sender, GridSortEventArgs e)
        {
            //Grid1.SortDirection = e.SortDirection;
            //Grid1.SortField = e.SortField;

            BindSMSGrid();
        }

        protected void ttbHistorySearch_Trigger1Click(object sender, EventArgs e)
        {
            ttbHistorySearch.Text = String.Empty;
            ttbHistorySearch.ShowTrigger1 = false;

            BindSendHistoryGrid();
        }

        protected void ttbHistorySearch_Trigger2Click(object sender, EventArgs e)
        {
            ttbHistorySearch.ShowTrigger1 = true;

            BindSendHistoryGrid();
        }


        protected void ttbSearch_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearch.Text = String.Empty;
            ttbSearch.ShowTrigger1 = false;

            BindSMSGrid();
        }

        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearch.ShowTrigger1 = true;

            BindSMSGrid();
        }

        private void BindSMSGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.SMSTemplate> q = DB.SMSTemplates; //.Include(u => u.Dept);

            // 在用户名称中搜索
            string searchText = ttbSearch.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText));
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            GridSmsTemplate.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.SMSTemplate>(q, GridSmsTemplate);

            GridSmsTemplate.DataSource = q;
            GridSmsTemplate.DataBind();
        }

        private void BindSendHistoryGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.SMSSendHistory> q = DB.SMSSendHistorys; //.Include(u => u.Dept);

            // 在用户名称中搜索
            string searchText = ttbHistorySearch.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText) || u.Tel.Contains(searchText));
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            GridSendHistory.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.SMSSendHistory>(q, GridSendHistory);

            GridSendHistory.DataSource = q;
            GridSendHistory.DataBind();
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void DropDownBoxSmsTemplate_TextChanged(object sender, EventArgs e)
        {
            if (DropDownBoxSmsTemplate.Value != null)
            {
                int id = Change.ToInt(DropDownBoxSmsTemplate.Values.First());
                SMSTemplate template = DB.SMSTemplates.Find(id);
                if (template != null)
                {
                    tbxSmsText.Text = template.Content;
                }
            }
            else
            {
                tbxSmsText.Text = "下拉框为空";
            }
        }

        static readonly Regex _telRegex = new Regex(@"([A-Za-z0-9_\-\u4e00-\u9fa5]+)(\(0?(13|14|15|18)[0-9]{9}\))|0?(13|14|15|18)[0-9]{9}");
        protected void btnSendSms_Click(object sender, EventArgs e)
        {
            string tels = tbxReceiverNames.Text.Trim();
            if (tbxSmsText.Text.Length == 0 || tels.Length == 0)
            {
                Alert.Show("请选择接收人或输入短信内容");
                return;
            }
            List<SMSReceiver> receiverList = new List<SMSReceiver>();

            StringBuilder output = new StringBuilder(); 
			foreach (Match m in _telRegex.Matches(tels))
			{
                string tel = "";
                string name = "";
                if (m.Groups.Count > 0)
                {
                    name = m.Groups[1].Value;
                    tel = m.Groups[2].Value;
                }
                else
                {
                    tel = m.Value;
                }

                tel = tel.Replace("(", "").Replace(")", "");

                receiverList.Add(new SMSReceiver() { Tel = tel, Name = name });
			}

            foreach (var rec in receiverList)
            {
                DB.SMSSendHistorys.Add(new SMSSendHistory() { 
                    SMSType = "短信",
                    Tel = rec.Tel,
                    Name = rec.Name,
                    Content = tbxSmsText.Text.Replace("%UserName%", rec.Name)
                });
            }

            SaveChanges();
            ShowNotify("发送成功");
        }

        protected void GridSendHistory_PageIndexChange(object sender, GridPageEventArgs e)
        {
            BindSendHistoryGrid();
        }
    }

    public class SMSReceiver
    {
        public int ID { get; set; }
        public string Tel { get; set; }
        public string Name { get; set; }
    }
}
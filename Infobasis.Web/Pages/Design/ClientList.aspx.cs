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

namespace Infobasis.Web.Pages.Design
{
    public partial class ClientList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();

                TriggerBoxInput.OnClientTriggerClick = Window1.GetSaveStateReference(TriggerBoxInput.ClientID, HiddenFieldInput.ClientID)
                    + Window1.GetShowReference("./triggerbox_iframe_iframe.aspx");
            }
        }

        protected string GetDesignProgressEditUrl(object id, object name)
        {
            JsObjectBuilder joBuilder = new JsObjectBuilder();
            joBuilder.AddProperty("id", "grid_newtab_designprogress_edit");
            joBuilder.AddProperty("title", "设计进度 - " + name);
            joBuilder.AddProperty("iframeUrl", String.Format("getEditWindowUrl('{0}','{1}')", id, HttpUtility.UrlEncode(name.ToString())), true); // ResolveUrl(String.Format("~/grid/grid_newtab_window.aspx?id={0}&name={1}", id, HttpUtility.UrlEncode(name.ToString()))));
            joBuilder.AddProperty("refreshWhenExist", true);
            joBuilder.AddProperty("iconFont", "edit");

            // addExampleTab函数定义在default.aspx，参数分别为：id, url, text, icon, refreshWhenExist
            return String.Format("parent.addTab({0});", joBuilder);
        }

        protected string GetDesignProgressLinkName(object id, object name)
        {
            return "设计进度";
        }

        private void LoadData()
        {
            // 权限检查

            //btnNew.OnClientClick = Window1.GetShowReference("~/Pages/Admin/User_Form.aspx", "新增用户");

            // 每页记录数
            Grid1.PageSize = UserInfo.Current.DefaultPageSize;
            ddlGridPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();

            BindGrid();
        }


        private void BindGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.Client> q = DB.Clients.Include("ClientTraces").Where(item => item.Disabled == null || item.Disabled == false); //.Include(u => u.Dept);

            // 在用户名称中搜索
            string searchText = tbxName.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText) || u.QQ.Contains(searchText) || u.WeChat.Contains(searchText)
                    || u.Tel.Contains(searchText));
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.Client>(q, Grid1);
            foreach (var vi in q)
            {
                vi.TraceNum = vi.ClientTraces.Count();
                vi.LastTraceDate = vi.ClientTraces.Last().CreateDatetime;
                vi.LastTraceMsg = vi.ClientTraces.Last().TraceDesc;
            }

            Grid1.DataSource = q;
            Grid1.DataBind();
        }

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查

        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }

        #endregion

        protected void btnRest_Click(object sender, EventArgs e)
        {
            SimpleForm1.Reset();
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            Infobasis.Data.DataEntity.Client client = e.DataItem as Infobasis.Data.DataEntity.Client;
            DateTime? lastTraceDate = Convert.ToDateTime(client.LastTraceDate);

            FineUIPro.BoundField bfTraceNum = Grid1.FindColumn("bfTraceNum") as FineUIPro.BoundField;
            int columnIndexTraceNum = bfTraceNum.ColumnIndex;
            int lastTraceDays = Infobasis.Web.Util.DateHelper.GetClientTraceDays(lastTraceDate, DateTime.Now);

            e.Values[columnIndexTraceNum] = String.Format("<span class=\"{0}\" data-qtip=\"{4}\">{1}-{2}-{3}</span>",
                lastTraceDays >= 10 || lastTraceDays == 0 ? "traceWarning" : "traceNormal",
                Change.ToInt(lastTraceDays), 0, 0,
                string.Format("最后跟进已过:{0} 天", lastTraceDays));
        }
    }
}
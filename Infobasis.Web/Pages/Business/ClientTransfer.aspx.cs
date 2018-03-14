using FineUIPro;
using Infobasis.Data.DataAccess;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Business
{
    public partial class ClientTransfer : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                BindGrid();
            }
        }

        #region BindGrid

        private void BindGrid()
        {
            // 1.设置总项数（特别注意：数据库分页一定要设置总记录数RecordCount）
            Grid1.RecordCount = GetTotalCount();

            // 2.获取当前分页数据
            DataTable table = GetPagedDataTable();

            // 3.绑定到Grid
            Grid1.DataSource = table;
            Grid1.DataBind();
        }

        /// <summary>
        /// 模拟返回总项数
        /// </summary>
        /// <returns></returns>
        private int GetTotalCount()
        {
            return GetSource().Rows.Count;
        }

        /// <summary>
        /// 模拟数据库分页（实际项目中请直接使用SQL语句返回分页数据！）
        /// </summary>
        /// <returns></returns>
        private DataTable GetPagedDataTable()
        {
            int pageIndex = Grid1.PageIndex;
            int pageSize = Grid1.PageSize;

            DataTable table = GetSource();

            DataTable paged = table.Clone();
            int rowbegin = pageIndex * pageSize;
            int rowend = (pageIndex + 1) * pageSize;
            if (rowend > table.Rows.Count)
            {
                rowend = table.Rows.Count;
            }
            for (int i = rowbegin; i < rowend; i++)
            {
                paged.ImportRow(table.Rows[i]);
            }

            return paged;
        }

        private DataTable GetSource()
        {
            string sortField = Grid1.SortField;
            string sortDirection = Grid1.SortDirection;
            IInfobasisDataSource db = InfobasisDataSource.Create();
             DataTable table2 = db.ExecuteTable ("SELECT ID, 'test' AS DeptName, ChineseName, Gender FROM SYtbUser WHERE CompanyID = @CompanyID", UserInfo.Current.CompanyID);

            DataView view2 = table2.DefaultView;
            view2.Sort = String.Format("{0} {1}", sortField, sortDirection);

            List<string> filters = new List<string>();

            string searchKeyword = ttbSearch.Text.Trim();
            if (!String.IsNullOrEmpty(searchKeyword) && ttbSearch.ShowTrigger1)
            {
                // RowFilter的用法：http://www.csharp-examples.net/dataview-rowfilter/
                filters.Add(String.Format("ChineseName LIKE '*{0}*'", EscapeLikeValue(searchKeyword)));
            }

            if (filters.Count > 0)
            {
                view2.RowFilter = String.Join(" AND ", filters.ToArray());
            }

            return view2.ToTable();
        }



        #endregion

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            //Grid1.PageIndex = e.NewPageIndex;

            BindGrid();
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            //Grid1.SortDirection = e.SortDirection;
            //Grid1.SortField = e.SortField;

            BindGrid();
        }


        protected void ttbSearch_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearch.Text = String.Empty;
            ttbSearch.ShowTrigger1 = false;

            BindGrid();
        }

        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearch.ShowTrigger1 = true;

            BindGrid();
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
            client.DesignDeptID = 0;
            client.DesignDeptName = "设计部";
            int designerID = Infobasis.Web.Util.Change.ToInt(DropDownBoxDesigner.Value);
            string designerName = "";
            if (designerID > 0)
            {
                designerName = DB.Users.Find(designerID).ChineseName;
            }
            client.DesignUserID = designerID;
            client.DesignUserDisplayName = designerName;
            client.AssignToDesignerDatetime = DateTime.Now;
            client.AssignToDesignerRemark = tbxRemark.Text;

            SaveChanges();
            ShowNotify("转部成功");
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
        }
    }
}
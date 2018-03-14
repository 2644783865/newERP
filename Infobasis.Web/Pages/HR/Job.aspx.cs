using FineUIPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EntityFramework.Extensions;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using Infobasis.Data.DataAccess;

namespace Infobasis.Web.Pages.HR
{
    public partial class Job : PageBase
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
            // 权限检查
            //CheckPowerDeleteWithButton(btnDeleteSelected);

            //ResolveDeleteButtonForGrid(btnDeleteSelected, Grid1);

            btnNew.OnClientClick = Window1.GetShowReference("~/Pages/HR/Job_Form.aspx", "新增职务");

            // 每页记录数
            Grid1.PageSize = UserInfo.Current.DefaultPageSize;

            BindGrid();
        }

        private void BindGrid()
        {
            IQueryable<JobRole> q = DB.JobRoles;

            // 在职务名称中搜索
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(t => t.Name.Contains(searchText));
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = q.Count();

            // 排列和分页
            q = SortAndPage<JobRole>(q, Grid1);

            Grid1.DataSource = q;
            Grid1.DataBind();
        }


        #region Events

        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }

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

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int titleID = GetSelectedDataKeyID(Grid1);

            if (e.CommandName == "Delete")
            {
                int userCount = DB.Users.Where(u => u.JobRole.ID == titleID).Count();
                if (userCount > 0)
                {
                    Alert.ShowInTop("删除失败！需要先清空拥有此职务的用户！");
                    return;
                }

                IInfobasisDataSource db = InfobasisDataSource.Create();
                if (db.ExecuteNonQuery("DELETE FROM SKtbJobRole WHERE ID = @ID AND CompanyID = @CompanyID", titleID, UserInfo.Current.CompanyID) == 0)
                {
                    Alert.ShowInTop("删除失败！");
                }

                BindGrid();
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion
    }
}
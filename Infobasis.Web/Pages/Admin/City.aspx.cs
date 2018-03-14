using FineUIPro;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Admin
{
    public partial class City : PageBase
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
            // 权限检查

            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid2, "确定要移除选中的{0}项记录吗？");


            BindGrid1();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            // 每页记录数
            Grid2.PageSize = UserInfo.Current.DefaultPageSize;
            ddlGridPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();

            BindGrid2();
        }

        private void BindGrid1()
        {
            IQueryable<Province> q = DB.Provinces.OrderBy(item => item.DisplayOrder);

            // 排列
            q = Sort<Province>(q, Grid1);

            Grid1.DataSource = q;
            Grid1.DataBind();
        }

        private void BindGrid2()
        {
            int provinceID = GetSelectedDataKeyID(Grid1);

            if (provinceID == -1)
            {
                Grid2.RecordCount = 0;

                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                IQueryable<Infobasis.Data.DataEntity.City> q = DB.Citys;

                // 在名称中搜索
                string searchText = ttbSearchUser.Text.Trim();
                if (!String.IsNullOrEmpty(searchText))
                {
                    q = q.Where(u => u.Name.Contains(searchText));
                }

                // 过滤选中
                q = q.Where(entity => entity.ProvinceID == provinceID);

                // 在查询添加之后，排序和分页之前获取总记录数
                Grid2.RecordCount = q.Count();

                // 排列和分页
                q = SortAndPage<Infobasis.Data.DataEntity.City>(q, Grid2);

                Grid2.DataSource = q;
                Grid2.DataBind();
            }

        }

        #endregion

        #region Events

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid2.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid2();
        }


        #endregion

        #region Grid1 Events

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid1();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            BindGrid2();
        }

        protected void Grid1_RowSelect(object sender, FineUIPro.GridRowSelectEventArgs e)
        {
            BindGrid2();
        }

        #endregion

        #region Grid2 Events

        protected void ttbSearchUser_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchUser.ShowTrigger1 = true;
            BindGrid2();
        }

        protected void ttbSearchUser_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchUser.Text = String.Empty;
            ttbSearchUser.ShowTrigger1 = false;
            BindGrid2();
        }

        protected void Grid2_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
        }

        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            Grid2.SortDirection = e.SortDirection;
            Grid2.SortField = e.SortField;
            BindGrid2();
        }

        protected void Grid2_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid2.PageIndex = e.NewPageIndex;
            BindGrid2();
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
        }


        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            int provinceID = GetSelectedDataKeyID(Grid1);
            object[] values = Grid2.DataKeys[e.RowIndex];
            int cityID = Convert.ToInt32(values[0]);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查

                Infobasis.Data.DataEntity.City tobeRemoved = DB.Citys.Where(item => item.ProvinceID == provinceID && item.ID == cityID).FirstOrDefault();

                if (tobeRemoved != null)
                {
                    DB.Citys.Remove(tobeRemoved);
                    DB.SaveChanges();
                }

                BindGrid2();

            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid2();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            int id = GetSelectedDataKeyID(Grid1);
            string addUrl = String.Format("~/Pages/Admin/City_Form.aspx?pid={0}", id);

            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加区域"));
        }

        #endregion

        protected void btnAddProvince_Click(object sender, EventArgs e)
        {
            string addUrl = "~/Pages/Admin/City_Form.aspx";

            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加城市"));
        }

        protected void btnRemoveProvince_Click(object sender, EventArgs e)
        {

        }
    }
}
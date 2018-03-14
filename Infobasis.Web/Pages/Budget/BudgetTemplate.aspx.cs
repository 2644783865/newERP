using FineUIPro;
using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EntityFramework.Extensions;

namespace Infobasis.Web.Pages.Budget
{
    public partial class BudgetTemplate : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();

            btnNew.OnClientClick = Window1.GetShowReference("~/Pages/Budget/Budget_Form.aspx", "新增预算模版");
        }

        protected string GetEditUrl(object id, object name)
        {
            JsObjectBuilder joBuilder = new JsObjectBuilder();
            joBuilder.AddProperty("id", "grid_newtab_budgetTemp_edit");
            joBuilder.AddProperty("title", "编辑 - " + name);
            joBuilder.AddProperty("iframeUrl", String.Format("getEditWindowUrl('{0}','{1}')", id, HttpUtility.UrlEncode(name.ToString())), true); // ResolveUrl(String.Format("~/grid/grid_newtab_window.aspx?id={0}&name={1}", id, HttpUtility.UrlEncode(name.ToString()))));
            joBuilder.AddProperty("refreshWhenExist", true);
            joBuilder.AddProperty("iconFont", "pencil");

            // addExampleTab函数定义在default.aspx，参数分别为：id, url, text, icon, refreshWhenExist
            return String.Format("parent.addTab({0});", joBuilder);
        }

        private void LoadData()
        {
            IInfobasisDataSource db = InfobasisDataSource.Create();

            // 权限检查

            //ResolveDeleteMenuButtonForGrid(mbDeleteRows, Grid1);
            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid1);
            ResolveDeleteMenuButtonForGrid(mbEnableRows, Grid1, "确定要启用选中的{0}项记录吗？");
            ResolveDeleteMenuButtonForGrid(mbDisableRows, Grid1, "确定要禁用选中的{0}项记录吗？");


            btnNew.OnClientClick = Window1.GetShowReference("~/Pages/Budget/Budget_Form.aspx", "新增报价模板");

            // 每页记录数
            Grid1.PageSize = UserInfo.Current.DefaultPageSize;
            ddlGridPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();

            BindGrid();
        }


        private void BindGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.BudgetTemplate> q = DB.BudgetTemplates; //.Include(u => u.Dept);

            // 在名称中搜索
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText) || u.Code.Contains(searchText));
            }

            // 过滤启用状态
            Infobasis.Data.DataEntity.BudgetTemplateStatus budgetSatus = BudgetTemplateStatus.Enabled;
            if (rblEnableStatus.SelectedValue != "all")
            {
                if (rblEnableStatus.SelectedValue == "enabled")
                    budgetSatus = BudgetTemplateStatus.Enabled;
                else if (rblEnableStatus.SelectedValue == "disabled")
                    budgetSatus = BudgetTemplateStatus.Disabled;

                q = q.Where(u => u.BudgetTemplateStatus == budgetSatus);
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.BudgetTemplate>(q, Grid1);

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

        // 超级管理员（admin）不可编辑，也不会检索出来
        protected void Grid1_PreRowDataBound(object sender, FineUIPro.GridPreRowEventArgs e)
        {
            Infobasis.Data.DataEntity.BudgetTemplate data = e.DataItem as Infobasis.Data.DataEntity.BudgetTemplate;

            FineUIPro.LinkButtonField deleteField = Grid1.FindColumn("deleteField") as FineUIPro.LinkButtonField;

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

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            // 在操作之前进行权限检查

            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);

            DB.BudgetTemplates.Where(u => ids.Contains(u.ID)).Delete();

            // 重新绑定表格
            BindGrid();
        }

        protected void btnEnable_Click(object sender, EventArgs e)
        {
            SetSelectedEnableStatus(true);
        }

        protected void btnDisable_Click(object sender, EventArgs e)
        {
            SetSelectedEnableStatus(false);
        }


        private void SetSelectedEnableStatus(bool enabled)
        {
            // 在操作之前进行权限检查

            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);

            // 执行数据库操作
            DB.BudgetTemplates.Where(u => ids.Contains(u.ID)).Update(u => new Infobasis.Data.DataEntity.BudgetTemplate { BudgetTemplateStatus = enabled ? BudgetTemplateStatus.Enabled : BudgetTemplateStatus.Disabled });

            // 重新绑定表格
            BindGrid();
        }

        protected void Grid1_RowDataBound(object sender, FineUIPro.GridRowEventArgs e)
        {
            // e.DataItem  -> System.Data.DataRowView or custom class.
            // e.RowIndex -> Current row index.
            // e.Values -> Rendered html for each column of this row.

            Infobasis.Data.DataEntity.BudgetTemplate data = e.DataItem as Infobasis.Data.DataEntity.BudgetTemplate;

            BudgetTemplateStatus status = (BudgetTemplateStatus)data.BudgetTemplateStatus;

        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int id = GetSelectedDataKeyID(Grid1);
            Infobasis.Data.DataEntity.BudgetTemplate data = DB.BudgetTemplates.Find(id);

            string name = data.Name;

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查

                if (data.Code == "system")
                {
                    Alert.ShowInTop("不能删除默认的数据！");
                }
                else
                {
                    DB.BudgetTemplates.Where(u => u.ID == id).Delete();

                    BindGrid();
                }
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void rblEnableStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }


        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }

        #endregion
    }
}
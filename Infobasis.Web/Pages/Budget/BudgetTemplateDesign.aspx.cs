using FineUIPro;
using Infobasis.Data.DataEntity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Budget
{
    public partial class BudgetTemplateDesign : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int budgetTemplateID = GetQueryIntValue("id");
                Infobasis.Data.DataEntity.BudgetTemplate budgetTemplateData = DB.BudgetTemplates.Find(budgetTemplateID);
                panelTopRegion.Title = budgetTemplateData.Name;

                btnAddItem.OnClientClick = Window1.GetShowReference("~/Pages/Budget/BudgetItem_Form.aspx?pid=" + budgetTemplateID.ToString(), "添加模版房间");
                LoadData();
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
            BindTree();
        }

        private void LoadData()
        {
            BindGrid();
            // 默认选中第一个
            Grid1.SelectedRowIndex = 0;
            BindTree();
        }

        private bool AppendToEnd = true;
        private void CreateNewData()
        {
            JObject defaultObj = new JObject();
            defaultObj.Add("Name", "费率小计");
            defaultObj.Add("Amount", 1200);
            defaultObj.Add("Size", 0);

            Grid1.AddNewRecord(defaultObj, AppendToEnd);
        }

        private void OutputSummaryData(IQueryable<BudgetTemplateItem> q)
        {
            /*
            decimal? amountTotal = 0;
            decimal? sizeTotal = 0;
            amountTotal = q.Select(item => item.Amount).Sum();
            sizeTotal = q.Select(item => item.Size).Sum();

            JObject summary = new JObject();
            summary.Add("Name", "全部合计");
            summary.Add("Amount", amountTotal.Value.ToString("F2"));
            summary.Add("Size", sizeTotal.Value.ToString("F2"));

            labTotalSize.Text = sizeTotal.Value.ToString("F2");
            labTotalFee.Text = amountTotal.Value.ToString("F2");

            Grid1.SummaryData = summary;
            */
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            BindTree();
        }

        protected void Grid1_RowSelect(object sender, FineUIPro.GridRowSelectEventArgs e)
        {
            BindTree();
        }

        private void BindGrid()
        {
            int budgetTemplateID = GetQueryIntValue("id");
            IQueryable<BudgetTemplateItem> q = DB.BudgetTemplateItems.Include("BudgetTemplateItemMaterials").Where(item => item.BudgetTemplateID == budgetTemplateID);

            // 排列
            //q = Sort<BudgetTemplateSpace>(q, Grid1);

            Grid1.DataSource = q;
            Grid1.DataBind();

            // 当前页的合计
            OutputSummaryData(q);
        }

        private void BindTree()
        {
            int spaceID = GetSelectedDataKeyID(Grid1);
            int budgetTemplateID = GetQueryIntValue("id");
            btnAddNewItemMaterial.OnClientClick = Window2.GetShowReference("~/Pages/Budget/Budget_AddMaterial.aspx?pid=" + budgetTemplateID.ToString() + "&itemid=" + spaceID, "添加定额项目");
            if (spaceID == -1)
            {
                Grid2.RecordCount = 0;

                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                IQueryable<Infobasis.Data.DataEntity.BudgetTemplateItemMaterial> q = DB.BudgetTemplateItemMaterials.Include("Material").Where(item => item.BudgetTemplateItemID == spaceID);

                // 在查询添加之后，排序和分页之前获取总记录数
                Grid2.RecordCount = q.Count();

                // 排列和分页
                q = SortAndPage<Infobasis.Data.DataEntity.BudgetTemplateItemMaterial>(q, Grid2);

                Grid2.DataSource = q;
                Grid2.DataBind();
            }
        }
    }
}
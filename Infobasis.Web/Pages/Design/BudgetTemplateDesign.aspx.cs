using FineUIPro;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Design
{
    public partial class BudgetTemplateDesign : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int budgetTemplateID = GetQueryIntValue("id");
                BudgetTemplateData budgetTemplateData = DB.BudgetTemplateDatas.Find(budgetTemplateID);
                panelTopRegion.Title = budgetTemplateData.Name;

                LoadData();
            }
        }

        private void LoadData()
        {
            BindGrid();
            // 默认选中第一个
            Grid1.SelectedRowIndex = 0;
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

        private void OutputSummaryData(IQueryable<BudgetTemplateSpace> q)
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
            IQueryable<BudgetTemplateSpace> q = DB.BudgetTemplateSpaces.Where(item => item.BudgetTemplateDataID == budgetTemplateID);

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
        }
    }
}
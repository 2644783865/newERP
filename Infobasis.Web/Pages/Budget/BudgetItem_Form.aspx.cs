using FineUIPro;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Budget
{
    public partial class BudgetItem_Form : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitDropDownPartType();

                int budgetID = GetQueryIntValue("pid");
                if (budgetID > 0)
                {
                    Infobasis.Data.DataEntity.BudgetTemplate data = DB.BudgetTemplates
                        .Where(u => u.ID == budgetID).FirstOrDefault();
                    if (data == null)
                    {
                        // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                        Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                        return;
                    }
                }
            }
        }

        #region
        private void InitDropDownPartType()
        {
            DataTable table = GetEntityListTable("FJBW");
            DropDownPartType.DataSource = table;
            DropDownPartType.DataTextField = "Name";
            DropDownPartType.DataValueField = "ID";
            DropDownPartType.DataBind();
        }
        #endregion

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            int budgetID = GetQueryIntValue("pid");
            if (budgetID > 0)
            {
                Infobasis.Data.DataEntity.BudgetTemplate template = DB.BudgetTemplates
                    .Where(u => u.ID == budgetID).FirstOrDefault();
                if (template == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }

                Infobasis.Data.DataEntity.BudgetTemplateItem data = new Infobasis.Data.DataEntity.BudgetTemplateItem();
                data.BudgetTemplateID = budgetID;
                data.CreateDatetime = DateTime.Now;
                data.DisplayOrder = Change.ToInt(tbxDisplayOrder.Text);

                if (Change.ToInt(DropDownPartType.SelectedValue) > 0)
                {
                    data.PartTypeID = Change.ToInt(DropDownPartType.SelectedValue);
                    data.PartTypeName = DropDownPartType.SelectedText;
                }
                data.LastUpdateDatetime = DateTime.Now;
                data.Remark = tbxRemark.Text;

                DB.BudgetTemplateItems.Add(data);
            }

            SaveChanges();
            ShowNotify("添加成功");
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
        }
    }
}
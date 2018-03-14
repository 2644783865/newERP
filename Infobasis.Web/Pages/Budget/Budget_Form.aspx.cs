using FineUIPro;
using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
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
    public partial class Budget_Form : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitDropDownBoxBudgetType();
                InitDropDownProvince();

                int budgetID = GetQueryIntValue("id");
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
                    tbxCode.Text = data.Code;
                    tbxName.Text = data.Name;
                    tbxRemark.Text = data.Remark;
                    tbxDisplayOrder.Text = data.DisplayOrder.ToString();
                    DropDownProvince.SelectedValue = data.ProvinceID.ToString();
                    DropDownBoxBudgetType.SelectedValue = data.BudgetTypeID.ToString();
                    tbxIsActive.Checked = data.BudgetTemplateStatus == BudgetTemplateStatus.Enabled ? true : false;
                }
            }
        }

        #region
        private void InitDropDownBoxBudgetType()
        {
            DataTable table = GetEntityListTable("YSLX");
            DropDownBoxBudgetType.DataSource = table;
            DropDownBoxBudgetType.DataTextField = "Name";
            DropDownBoxBudgetType.DataValueField = "ID";
            DropDownBoxBudgetType.DataBind();
        }
        #endregion

        #region InitDropDownProvince
        private void InitDropDownProvince()
        {
            IQueryable table = DB.Provinces.Where(item => item.IsActive == true).OrderBy(item => item.DisplayOrder);
            DropDownProvince.DataSource = table;
            DropDownProvince.DataTextField = "Name";
            DropDownProvince.DataValueField = "ID";
            DropDownProvince.DataBind();
        }
        #endregion

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            int budgetID = GetQueryIntValue("id");
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
                data.Code = tbxCode.Text.Trim();
                data.Name = tbxName.Text;
                data.DisplayOrder = Change.ToInt(tbxDisplayOrder.Text);

                if (Change.ToInt(DropDownProvince.SelectedValue) > 0)
                {
                    data.ProvinceID = Change.ToInt(DropDownProvince.SelectedValue);
                    data.ProvinceName = DropDownProvince.SelectedText;
                }

                if (Change.ToInt(DropDownBoxBudgetType.SelectedValue) > 0)
                {
                    data.BudgetTypeID = Change.ToInt(DropDownBoxBudgetType.SelectedValue);
                    data.BudgetTypeName = DropDownBoxBudgetType.SelectedText;
                }

                data.IsActive = tbxIsActive.Checked;
                data.BudgetTemplateStatus = tbxIsActive.Checked ? BudgetTemplateStatus.Enabled : BudgetTemplateStatus.Disabled;
                data.LastUpdateDatetime = DateTime.Now;
                data.Remark = tbxRemark.Text;
                data.LastUpdateByID = UserInfo.Current.ID;
                data.LastUpdateByName = UserInfo.Current.ChineseName;
            }
            else
            {
                Infobasis.Data.DataEntity.BudgetTemplate data = new Infobasis.Data.DataEntity.BudgetTemplate()
                {
                    CreateDatetime = DateTime.Now,
                    Code = tbxCode.Text.Trim(),
                    Name = tbxName.Text,
                    Remark = tbxRemark.Text,
                    BudgetTemplateStatus = Infobasis.Data.DataEntity.BudgetTemplateStatus.Enabled
                };

                data.DisplayOrder = Change.ToInt(tbxDisplayOrder.Text);

                if (Change.ToInt(DropDownProvince.SelectedValue) > 0)
                {
                    data.ProvinceID = Change.ToInt(DropDownProvince.SelectedValue);
                    data.ProvinceName = DropDownProvince.SelectedText;
                }

                if (Change.ToInt(DropDownBoxBudgetType.SelectedValue) > 0)
                {
                    data.BudgetTypeID = Change.ToInt(DropDownBoxBudgetType.SelectedValue);
                    data.BudgetTypeName = DropDownBoxBudgetType.Text;
                }

                data.IsActive = tbxIsActive.Checked;
                data.BudgetTemplateStatus = tbxIsActive.Checked ? BudgetTemplateStatus.Enabled : BudgetTemplateStatus.Disabled;

                DB.BudgetTemplates.Add(data);
            }

            SaveChanges();
            ShowNotify("添加成功");
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
        }
    }
}
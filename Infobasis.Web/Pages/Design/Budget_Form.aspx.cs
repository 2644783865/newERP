using FineUIPro;
using Infobasis.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Design
{
    public partial class Budget_Form : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int budgetID = GetQueryIntValue("id");
                if (budgetID > 0)
                {
                    Infobasis.Data.DataEntity.BudgetTemplateData data = DB.BudgetTemplateDatas
                        .Where(u => u.ID == budgetID).FirstOrDefault();
                    if (data == null)
                    {
                        // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                        Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                        return;
                    }
                    tbxName.Text = data.Name;
                    tbxRemark.Text = data.Remark;
                }
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            int budgetID = GetQueryIntValue("id");
            if (budgetID > 0)
            {
                Infobasis.Data.DataEntity.BudgetTemplateData data = DB.BudgetTemplateDatas
                    .Where(u => u.ID == budgetID).FirstOrDefault();
                if (data == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }
                data.Name = tbxName.Text;
                data.LastUpdateDatetime = DateTime.Now;
                data.Remark = tbxRemark.Text;
                data.LastUpdateByID = UserInfo.Current.ID;
                data.LastUpdateByName = UserInfo.Current.ChineseName;
            }
            else
            {
                Infobasis.Data.DataEntity.BudgetTemplateData data = new Infobasis.Data.DataEntity.BudgetTemplateData()
                {
                    CreateDatetime = DateTime.Now,
                    Name = tbxName.Text,
                    Remark = tbxRemark.Text,
                    UserID = UserInfo.Current.ID,
                    BudgetTemplateStatus = Infobasis.Data.DataEntity.BudgetTemplateStatus.Enabled
                };
                DB.BudgetTemplateDatas.Add(data);
            }

            SaveChanges();
            ShowNotify("添加成功");
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
        }
    }
}
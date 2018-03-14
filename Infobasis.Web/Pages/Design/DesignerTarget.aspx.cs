using FineUIPro;
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
    public partial class DesignerTarget : PageBase
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
            DateTime today = DateTime.Now;
            int year = today.Year;
            int month = today.Month;
            int day = today.Day;

            string yearStr = year.ToString();
            labTitle.Text = yearStr + " 年目标管理";
            labAll.Label = UserInfo.Current.ChineseName + " " + yearStr + " 年总目标";
            int userID = UserInfo.Current.ID;

            Infobasis.Data.DataEntity.UserGoal userGoal = DB.UserGoals.Where(item => item.UserID == userID && item.Year == year).FirstOrDefault();

            if (userGoal != null)
            {
                tbxInput1.Text = Change.ToString(userGoal.Month1);
                tbxInput2.Text = Change.ToString(userGoal.Month2);
                tbxInput3.Text = Change.ToString(userGoal.Month3);
                tbxInput4.Text = Change.ToString(userGoal.Month4);
                tbxInput5.Text = Change.ToString(userGoal.Month5);
                tbxInput6.Text = Change.ToString(userGoal.Month6);
                tbxInput7.Text = Change.ToString(userGoal.Month7);
                tbxInput8.Text = Change.ToString(userGoal.Month8);
                tbxInput9.Text = Change.ToString(userGoal.Month9);
                tbxInput10.Text = Change.ToString(userGoal.Month10);
                tbxInput11.Text = Change.ToString(userGoal.Month11);
                tbxInput12.Text = Change.ToString(userGoal.Month12);

                decimal group1 = userGoal.Month1.Value + userGoal.Month2.Value + userGoal.Month3.Value;
                decimal group2 = userGoal.Month4.Value + userGoal.Month5.Value + userGoal.Month6.Value;
                decimal group3 = userGoal.Month7.Value + userGoal.Month8.Value + userGoal.Month9.Value;
                decimal group4 = userGoal.Month10.Value + userGoal.Month11.Value + userGoal.Month12.Value;

                labGroup1.Text = Change.ToString(group1);
                labGroup2.Text = Change.ToString(group2);
                labGroup3.Text = Change.ToString(group3);
                labGroup4.Text = Change.ToString(group4);

                tbxRemark.Text = userGoal.Remark;
                if (userGoal.LastUpdateDatetime.HasValue)
                {
                    labLastUpdate.Text = userGoal.LastUpdateDatetime.Value.ToString("yyyy-MM-dd hh:mm:ss");
                }

                labAll.Text = Change.ToString(group1 + group2 + group3 + group4);
            }

            int groupVal = month / 3;
            if (groupVal == 0)
                GroupPanel1.CssClass = "activeGroupPanel";
            if (groupVal == 1)
                GroupPanel2.CssClass = "activeGroupPanel";
            if (groupVal == 2)
                GroupPanel3.CssClass = "activeGroupPanel";
            if (groupVal == 3)
                GroupPanel4.CssClass = "activeGroupPanel";

            NumberBox inputControl = (NumberBox)GetControl(PanelMain, "tbxInput" + month.ToString());
            if (inputControl != null)
                inputControl.CssClass = "activeInputBox";

            NumberBox[] numberBoxs = { tbxInput1, tbxInput2, tbxInput3, tbxInput4, tbxInput5, tbxInput6, tbxInput7, tbxInput8, tbxInput9, tbxInput10, tbxInput11, tbxInput12 };
            int monthCount = 1;
            foreach (NumberBox btn in numberBoxs)
            {
                btn.Label = yearStr + "-" + monthCount.ToString("00");
                monthCount++;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int year = DateTime.Now.Year;
            int userID = UserInfo.Current.ID;
            Infobasis.Data.DataEntity.UserGoal goal;

            Infobasis.Data.DataEntity.UserGoal oldGoal = DB.UserGoals.Where(item => item.UserID == userID && item.Year == year).FirstOrDefault();
            if (oldGoal == null)
            {
                goal = new Infobasis.Data.DataEntity.UserGoal();
            }
            else
                goal = oldGoal;

            goal.UserID = userID;
            goal.Year = year;
            goal.Month1 = Change.ToDecimal(tbxInput1.Text, 0);
            goal.Month2 = Change.ToDecimal(tbxInput2.Text, 0);
            goal.Month3 = Change.ToDecimal(tbxInput3.Text, 0);
            goal.Month4 = Change.ToDecimal(tbxInput4.Text, 0);
            goal.Month5 = Change.ToDecimal(tbxInput5.Text, 0);
            goal.Month6 = Change.ToDecimal(tbxInput6.Text, 0);
            goal.Month7 = Change.ToDecimal(tbxInput7.Text, 0);
            goal.Month8 = Change.ToDecimal(tbxInput8.Text, 0);
            goal.Month9 = Change.ToDecimal(tbxInput9.Text, 0);
            goal.Month10 = Change.ToDecimal(tbxInput10.Text, 0);
            goal.Month11 = Change.ToDecimal(tbxInput11.Text, 0);
            goal.Month12 = Change.ToDecimal(tbxInput12.Text, 0);

            goal.Group1 = goal.Month1 + goal.Month2 + goal.Month3;
            goal.Group2 = goal.Month4 + goal.Month5 + goal.Month6;
            goal.Group3 = goal.Month7 + goal.Month8 + goal.Month9;
            goal.Group4 = goal.Month10 + goal.Month11 + goal.Month12;
            goal.Remark = tbxRemark.Text;

            goal.Total = goal.Group1 + goal.Group2 + goal.Group3 + goal.Group4;
            if (oldGoal == null)
            {
                DB.UserGoals.Add(goal);
                goal.LastUpdateDatetime = DateTime.Now;
            }
            else
            {
                goal.LastUpdateByID = userID;
                goal.LastUpdateDatetime = DateTime.Now;
            }

            if (SaveChanges())
            {
                ShowNotify("目标保存成功");
                LoadData();
                PageContext.RegisterStartupScript("initChart();");
            }
            else
                ShowNotify("目标保存失败");
        }
    }
}
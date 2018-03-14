using FineUIPro;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.HR
{
    public partial class EE_Education_Form : PageBase
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
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            int id = GetQueryIntValue("id");
            if (id > 0)
            {
                EmployeeEducation current = DB.EmployeeEducations.Find(id);
                if (current == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }

                tbxEducationalInstitution.Text = current.EducationalInstitution;
                tbxMajor.Text = current.Major;
                tbxAcademicDegree.Text = current.AcademicDegree;
                DropDownEducation.SelectedValue = current.Education.Value.ToString();
                DropDownEducationType.SelectedValue = current.EducationType.Value.ToString();
                tbxStartDate.Text = current.StartDate.ToString();
                tbxEndDate.Text = current.EndDate.ToString();

                tbxRemark.Text = current.Remark;
                cbxIsHighest.Checked = current.IsHighest.Value;
            }
        }

        protected void btnEducation_Click(object sender, EventArgs e)
        {
            int id = GetQueryIntValue("id");
            int uid = GetQueryIntValue("uid");
            string activeTab = GetQueryValue("activeTab");
            Infobasis.Data.DataEntity.EmployeeEducation eeEd = DB.EmployeeEducations.Where(u => u.ID == id).FirstOrDefault();
            bool createNew = false;

            if (eeEd != null)
            {
                eeEd.LastUpdateDatetime = DateTime.Now;
                eeEd.LastUpdateByID = UserInfo.Current.ID;
                eeEd.LastUpdateByName = UserInfo.Current.ChineseName;
            }
            else
            {
                eeEd = new EmployeeEducation();
                eeEd.CreateDatetime = DateTime.Now;
                eeEd.CreateByID = UserInfo.Current.ID;
                eeEd.CreateByName = UserInfo.Current.ChineseName;

                createNew = true;
            }

            eeEd.UserID = uid;
            eeEd.EducationalInstitution = tbxEducationalInstitution.Text;
            eeEd.Major = tbxMajor.Text;
            eeEd.Education = Change.ToInt(DropDownEducation.SelectedValue);
            eeEd.EducationName = DropDownEducation.SelectedText;
            eeEd.EducationType = Change.ToInt(DropDownEducationType.SelectedValue);
            eeEd.EducationTypeName = DropDownEducationType.SelectedText;
            eeEd.AcademicDegree = tbxAcademicDegree.Text;

            DateTime startDate = Change.ToDateTime(tbxStartDate.Text);
            DateTime endDate = Change.ToDateTime(tbxEndDate.Text);
            if (startDate != DateTime.MinValue)
                eeEd.StartDate = Change.ToDateTime(tbxStartDate.Text);
            if (endDate != DateTime.MinValue)
                eeEd.EndDate = Change.ToDateTime(tbxEndDate.Text);

            eeEd.IsHighest = cbxIsHighest.Checked;
            eeEd.Remark = tbxRemark.Text;

            EmployeeAdjust eeAdjust = new EmployeeAdjust()
            {
                UserID = uid,
                AdjustItemName = "教育培训经历",
                AdjustDate = DateTime.Now,
                AllChangeData = eeEd.ToString(),
                isAdjusted = true,
                CreateByID = UserInfo.Current.ID,
                CreateByName = UserInfo.Current.ChineseName,
                CreateDatetime = DateTime.Now
            };

            DB.EmployeeAdjusts.Add(eeAdjust);

            if (createNew)
                DB.EmployeeEducations.Add(eeEd);

            SaveChanges();

            ShowNotify("保存成功！");
            PageContext.RegisterStartupScript("refreshTopWindow();");
        }
    }
}
using FineUIPro;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using AspNetUI = System.Web.UI.WebControls;
using Infobasis.Data.DataEntity;

namespace Infobasis.Web.Pages.HR
{
    public partial class EE_Form : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string innerActiveTab = GetQueryValue("innerActiveTab");
            if (!IsPostBack)
            {
                LoadData();
               
                int uid = GetQueryIntValue("id");

                btnNewEducation.OnClientClick = Window1.GetShowReference("~/Pages/HR/EE_Education_Form.aspx?uid=" + uid.ToString() + "&innerActiveTab=" + TabEducation.ClientID, "新增记录");
                btnNewContract.OnClientClick = Window1.GetShowReference("~/Pages/HR/EE_Contract_Form.aspx?uid=" + uid.ToString() + "&innerActiveTab=" + TabContract.ClientID, "新增记录");
                btnNewEmployeement.OnClientClick = Window1.GetShowReference("~/Pages/HR/EE_Employeement_Form.aspx?uid=" + uid.ToString() + "&innerActiveTab=" + TabEmployeement.ClientID, "新增记录");
                // 初始化用户所属角色
                InitUserRole();

                // 初始化用户所属部门
                InitUserDept();

                // 初始化用户所属职称
                InitUserTitle();
            }

            if (!string.IsNullOrEmpty(innerActiveTab))
            {
                PageContext.RegisterStartupScript("F.ready(function () {setActiveTabById('" + innerActiveTab + "');})");
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            LoadData();
        }

        #region InitUserRole

        private void InitUserDept()
        {
            gridDept.DataSource = DB.Departments.OrderBy(d => d.DisplayOrder).ToList();
            gridDept.DataBind();

        }

        #endregion

        #region InitUserRole

        private void InitUserRole()
        {
            cblRoles.DataSource = DB.PermissionRoles.OrderBy(p => p.DisplayOrder);
            cblRoles.DataBind();
        }
        #endregion

        #region InitUserJobTitle

        private void InitUserTitle()
        {
            cblTitles.DataSource = DB.JobRoles.OrderBy(j => j.DisplayOrder); ;
            cblTitles.DataBind();

        }
        #endregion

        private void LoadData()
        {
            //btnClose.OnClientClick = ActiveWindow.GetHideReference();
            //btnClose.OnClientClick = "parent.removeActiveTab();";

            int id = GetQueryIntValue("id");
            if (id > 0)
            {
                Infobasis.Data.DataEntity.User user = DB.Users.Include("Department")
                    .Include("ReportsToUser")
                    .Include("JobRole")
                    .Include("UserPermissionRoles")
                    .Where(d => d.ID == id).FirstOrDefault();
                if (user != null)
                {
                    tbxName.Text = user.ChineseName;
                    ddlGender.SelectedValue = user.Gender;
                    //ddlGender.Text = user.Gender;
                    tbxRemark.Text = user.Remark;
                    dpBirthDay.Text = user.DateOfBirth.ToString();
                    tbxTel.Text = user.MobileNumber;
                    tbxEmail.Text = user.Email;
                    tbxEmployeeNum.Text = user.EmployeeCode;
                    ddlEmploymentType.SelectedValue = user.EmploymentType.ToString();

                    if (!string.IsNullOrEmpty(user.UserPortraitPath))
                    {
                        imgUserPortal.ImageUrl = user.UserPortraitPath;
                        userPortraitUpload.ButtonText = "修改头像";
                    }
                    else
                    {
                        imgUserPortal.ImageUrl = Global.Default_User_Portrait_Path;
                    }

                    userPortraitUpload.Hidden = false;

                    ddlMarriage.SelectedValue = user.MaritalStatus;

                    ddlNation.SelectedValue = user.Ethnic;
                    tbxUserName.Text = user.Name;
                    tbxPassWord.Text = user.Password;
                    tbxPassWord.Hidden = true;
                    tbxUserName.Readonly = true;
                    tbxPassWord.Readonly = true;
                    tbxOnBoardDate.Text = user.OnBoardDate.HasValue ? user.OnBoardDate.ToString() : null;
                    tbxBecomeRegularDate.Text = user.BecomeRegularDate.HasValue ? user.BecomeRegularDate.ToString() : null;

                    if (user.ReportsTo != null)
                    {
                        tbxReportToID.Text = user.ReportsTo.Value.ToString();
                        tbxReportTo.Text = user.ReportsToUser.ChineseName;
                    }

                    if (user.Department != null)
                    {
                        ddbDept.Value = user.DepartmentID.ToString();
                        ddbDept.Text = user.Department.Name;
                    }

                    if (user.UserPermissionRoles != null)
                    {
                        ddbRoles.Values = user.UserPermissionRoles.Select(item => item.PermissionRoleID.ToString()).ToArray();
                        ddbRoles.Text = String.Join(",", DB.UserPermissionRoles.Where(item => item.UserID == id).Select(r => r.PermissionRole.Name).ToArray());
                    }

                    if (user.JobRole != null)
                    {
                        ddbTitles.Value = user.JobID.ToString();
                        ddbTitles.Text = user.JobRole.Name;
                    }

                    ddlCertificates.SelectedValue = user.IDType;
                    tbxIDCardEdit.Text = user.IDNumber;
                    ddlEducation.SelectedValue = user.Education;
                    ddlNativePlace.SelectedValue = user.NativePlace;
                    tbxNativePlace.Text = user.MailAddress;
                }
            }

            BindGridAdjust();
            LoadFixPay();
            LoadBankInfo();
            LoadEducation();
            LoadEmployeement();
            LoadContract();
        }

        private void LoadContract()
        {
            int id = GetQueryIntValue("id");
            IQueryable<Infobasis.Data.DataEntity.EmployeeContract> q = DB.EmployeeContracts;

            q = q.Where(item => item.UserID == id);

            // 在查询添加之后，排序和分页之前获取总记录数
            GridContract.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.EmployeeContract>(q, GridContract);

            GridContract.DataSource = q;
            GridContract.DataBind();
        }

        private void LoadEmployeement()
        {
            int id = GetQueryIntValue("id");
            IQueryable<Infobasis.Data.DataEntity.EmployeeWorkExperience> q = DB.EmployeeWorkExperiences;

            q = q.Where(item => item.UserID == id);

            // 在查询添加之后，排序和分页之前获取总记录数
            GridEmployeement.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.EmployeeWorkExperience>(q, GridEmployeement);

            GridEmployeement.DataSource = q;
            GridEmployeement.DataBind();
        }

        private void LoadEducation()
        {
            int id = GetQueryIntValue("id");
            IQueryable<Infobasis.Data.DataEntity.EmployeeEducation> q = DB.EmployeeEducations;

            q = q.Where(item => item.UserID == id);

            // 在查询添加之后，排序和分页之前获取总记录数
            GridEducation.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.EmployeeEducation>(q, GridEducation);

            GridEducation.DataSource = q;
            GridEducation.DataBind();
        }

        private void LoadFixPay()
        {
            int id = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.EmployeeFixPay fixPay = DB.EmployeeFixPays.Where(u => u.UserID == id).FirstOrDefault();
            if (fixPay != null)
            {
                tbxProbationFixPayValue.Text = fixPay.ProbationFixPayValue.ToString();
                tbxFixPayValue.Text = fixPay.FixPayValue.ToString();
                tbxJobAllowanceValue.Text = fixPay.JobAllowanceValue.ToString();
                tbxTrafficAllowanceValue.Text = fixPay.TrafficAllowanceValue.ToString();
                tbxDiningAllowanceValue.Text = fixPay.DiningAllowanceValue.ToString();
            }
        }

        private void LoadBankInfo()
        {
            int id = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.EmployeeBank eeBank = DB.EmployeeBanks.Where(u => u.UserID == id).FirstOrDefault();
            if (eeBank != null)
            {
                tbxBankAccount.Text = eeBank.BankAccount;
                tbxBankName.Text = eeBank.BankName;
                tbxBankUserName.Text = eeBank.AccountHolder;
            }
        }

        private void BindGridAdjust()
        {
            int id = GetQueryIntValue("id");
            IQueryable<Infobasis.Data.DataEntity.EmployeeAdjust> q = DB.EmployeeAdjusts;

            q = q.Where(item => item.UserID == id);

            // 在查询添加之后，排序和分页之前获取总记录数
            GridAdjust.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.EmployeeAdjust>(q, GridAdjust);

            GridAdjust.DataSource = q;
            GridAdjust.DataBind();
        }

        protected void GridAdjust_Sort(object sender, GridSortEventArgs e)
        {
            GridAdjust.SortDirection = e.SortDirection;
            GridAdjust.SortField = e.SortField;
            BindGridAdjust();
        }

        protected void dpBirthDay_TextChanged(object sender, EventArgs e)
        {
            if (dpBirthDay.Text != null)
            {
                int diffDays = DateTime.Now.Subtract(Change.ToDateTime(dpBirthDay.Text)).Days;
                tbxAge.Text = Change.ToString(diffDays / 365);
            }
        }

        protected void userPortraitUpload_FileSelected(object sender, EventArgs e)
        {
            if (userPortraitUpload.HasFile)
            {
                int companyID = UserInfo.Current.CompanyID;
                int userID = GetQueryIntValue("id");

                string fileOriginalName = userPortraitUpload.ShortFileName;

                if (!ValidateFileType(fileOriginalName))
                {
                    ShowNotify("无效的文件类型！");
                    return;
                }

                string uploadPath = Global.UploadFolderPath;
                if (uploadPath.StartsWith("~") || uploadPath.StartsWith(".")) //相对路径
                    uploadPath = HttpContext.Current.Server.MapPath(uploadPath + "/images/" + companyID.ToString());
                else
                    uploadPath = uploadPath + "/images/" + companyID;

                string originalFolderPath = Path.Combine(uploadPath, DateTime.Now.ToString("yyyyMM") + "/original");
                string thumbnailFolderPath = Path.Combine(uploadPath, DateTime.Now.ToString("yyyyMM") + "/thumbnail");

                bool folderExists = Directory.Exists(originalFolderPath);
                if (!folderExists)
                    Directory.CreateDirectory(originalFolderPath);

                folderExists = Directory.Exists(thumbnailFolderPath);
                if (!folderExists)
                    Directory.CreateDirectory(thumbnailFolderPath);

                string fileType = fileOriginalName.Substring(fileOriginalName.LastIndexOf("."));
                string fileName = DateTime.Now.Ticks.ToString();
                string fileOriginalSavePath = Path.Combine(originalFolderPath, fileName + fileType);

                userPortraitUpload.SaveAs(fileOriginalSavePath);

                System.Drawing.Image originalImage = StreamHelper.ImagePath2Img(fileOriginalSavePath);
                string fileThumbnailSavePath = Path.Combine(thumbnailFolderPath, fileName + fileType);
                System.Drawing.Image newImage = ImageHelper.GetThumbNailImage(originalImage, 160, 160);
                newImage.Save(fileThumbnailSavePath);

                string savedPath = Global.UploadFolderVirualPath + "/images/" + companyID.ToString() + "/" + DateTime.Now.ToString("yyyyMM") + "/thumbnail/" + fileName + fileType;
                Infobasis.Data.DataEntity.User user = DB.Users.Find(userID);
                user.UserPortraitPath = savedPath;
                DB.SaveChanges();

                imgUserPortal.ImageUrl = savedPath;

                // 清空文件上传组件（上传后要记着清空，否则点击提交表单时会再次上传！！）
                userPortraitUpload.Reset();
            }

        }

        private void SaveItem()
        {
            int id = GetQueryIntValue("id");
            List<EmployeeAdjustItem> adjustItems = new List<EmployeeAdjustItem>();
            int companyID = UserInfo.Current.CompanyID;
            Infobasis.Data.DataEntity.User item = null;
            if (id == 0)
            {
                item = new Infobasis.Data.DataEntity.User();
                if (string.IsNullOrEmpty(tbxUserName.Text))
                {
                    item.Name = tbxTel.Text.Trim();
                }

                if (string.IsNullOrEmpty(tbxPassWord.Text))
                {
                    item.Password = PasswordUtil.CreateDbPassword(tbxPassWord.Text.Trim());
                }
                item.CreateDatetime = DateTime.Now;
                item.HireStatus = 0; 

                adjustItems.Add(EmployeeAdjustItem.Create);
            }
            else
            {
                item = DB.Users.Include("UserPermissionRoles").Where(u => u.ID == id).FirstOrDefault();
                item.LastUpdateByID = UserInfo.Current.ID;
                item.LastUpdateByName = UserInfo.Current.ChineseName;
                item.LastUpdateDatetime = DateTime.Now;

                if (item.EmploymentType != (ddlEmploymentType.SelectedValue != null ? Change.ToInt(ddlEmploymentType.SelectedValue) : 1))
                    adjustItems.Add(EmployeeAdjustItem.EmploymentType);

                if (Change.ValueIsChanged(item.ReportsTo, Change.ToInt(tbxReportToID.Text)))
                    adjustItems.Add(EmployeeAdjustItem.ReportsTo);

                if (Change.ValueIsChanged(item.JobID, Change.ToInt(ddbTitles.Value)))
                    adjustItems.Add(EmployeeAdjustItem.Job);

                if (Change.ValueIsChanged(item.DepartmentID, Change.ToInt(ddbDept.Value)))
                    adjustItems.Add(EmployeeAdjustItem.Department);

                if (item.OnBoardDate != Change.ToDateTime(tbxOnBoardDate.Text))
                    adjustItems.Add(EmployeeAdjustItem.OnBoard);

            }

            item.ChineseName = tbxName.Text.Trim();
            item.EmployeeSpellCode = ChinesePinyin.GetFirstPinyin(tbxName.Text.Trim());
            item.EnglishName = ChinesePinyin.GetPinyin(tbxName.Text.Trim());
            item.EmployeeCode = tbxEmployeeNum.Text.Trim();
            item.Email = tbxEmail.Text.Trim();
            item.Remark = tbxRemark.Text.Trim();
            item.Enabled = true;
            item.CompanyID = companyID;
            item.UserType = UserType.Employee;
            item.Gender = ddlGender.SelectedValue;
            item.DateOfBirth = Change.ToDateTime(dpBirthDay.Text);

            item.EmployeeCode = tbxEmployeeNum.Text.Trim();
            item.EmploymentType = ddlEmploymentType.SelectedValue != null ? Change.ToInt(ddlEmploymentType.SelectedValue) : 1;
            item.MaritalStatus = ddlMarriage.SelectedValue;
            item.Ethnic = ddlNation.SelectedValue;
            item.OnBoardDate = Change.ToDateTime(tbxOnBoardDate.Text);

            if (tbxBecomeRegularDate.Text != null)
                item.BecomeRegularDate = Change.ToDateTime(tbxBecomeRegularDate.Text);

            if (tbxReportToID.Text != null && Change.ToInt(tbxReportToID.Text) > 0)
                item.ReportsTo = Change.ToInt(tbxReportToID.Text);

            item.IDType = ddlCertificates.SelectedValue;
            item.IDNumber = tbxIDCardEdit.Text;
            item.Education = ddlEducation.Text;
            item.NativePlace = ddlNativePlace.SelectedValue;
            item.MailAddress = tbxNativePlace.Text;
            item.MobileNumber = tbxTel.Text.Trim();

            // 添加角色
            int[] newEntityIDs = ddbRoles.Values != null ? ddbRoles.Values.Select(r => Convert.ToInt32(r)).ToArray() : new int[1];
            if (item.UserPermissionRoles == null)
                item.UserPermissionRoles = new List<UserPermissionRole>();

            ICollection<UserPermissionRole> existEntities = DB.UserPermissionRoles.Where(up => up.UserID == item.ID).ToList();
            int[] tobeAdded = newEntityIDs.Except(existEntities.Select(x => x.PermissionRoleID)).ToArray();
            int[] tobeRemoved = existEntities.Select(x => x.PermissionRoleID).Except(newEntityIDs).ToArray();

            foreach (int pid in tobeAdded)
            {
                UserPermissionRole newEntity = new UserPermissionRole()
                {
                    CompanyID = companyID,
                    UserID = item.ID,
                    PermissionRoleID = pid,
                    CreateDatetime = DateTime.Now
                };
                //moduleRoleRepository.Insert(newEntity, out msg, false);
                item.UserPermissionRoles.Add(newEntity);
            }

            foreach (int pid in tobeRemoved)
            {
                var removeEntity = DB.UserPermissionRoles.Find(pid);
                item.UserPermissionRoles.Remove(removeEntity);
            }

            if (tobeAdded.Length > 0 || tobeRemoved.Length > 0)
                adjustItems.Add(EmployeeAdjustItem.UserPermissionRole);

            // 添加所有部门
            if (ddbDept.Value != null && Change.ToInt(ddbDept.Value) > 0)
            {
                item.DepartmentID = Change.ToInt(ddbDept.Value);
            }

            // 添加所有职称
            if (ddbTitles.Values != null && Change.ToInt(ddbTitles.Value) > 0)
            {
                item.JobID = Change.ToInt(ddbTitles.Value);
            }

            if (id == 0)
            {
                DB.Users.Add(item);
            }

            string adjustItemNames = "";
            if (adjustItems.Count == 0)
                adjustItems.Add(EmployeeAdjustItem.Others);

            foreach(var adjustItem in adjustItems)
            {
                adjustItemNames += ", " + EnumHelper.GetDescription(adjustItem);
            }

            EmployeeAdjust eeAdjust = new EmployeeAdjust()
            {
                UserID = item.ID,
                AdjustItemName = adjustItemNames,
                AdjustDate = DateTime.Now,
                AllChangeData = item.ToString(),
                isAdjusted = true,
                CreateByID = UserInfo.Current.ID,
                CreateByName = UserInfo.Current.ChineseName,
                CreateDatetime = DateTime.Now
            };

            DB.EmployeeAdjusts.Add(eeAdjust);

            SaveChanges();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string inputUserNum = tbxEmployeeNum.Text.Trim();
            int id = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.User user = DB.Users.Where(u => u.EmployeeCode == inputUserNum && u.ID != id).FirstOrDefault();

            if (user != null)
            {
                Alert.Show("用户员工编号 " + inputUserNum + " 已经存在！");
                return;
            }

            string inputUserName = tbxName.Text.Trim();

            Infobasis.Data.DataEntity.User userToName = DB.Users.Where(u => u.Name == inputUserName && u.ID != id).FirstOrDefault();

            if (userToName != null)
            {
                Alert.Show("用户 " + inputUserName + " 已经存在！");
                return;
            }

            SaveItem();
            BindGridAdjust();
            ShowNotify("保存成功！");

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void btnBank_Click(object sender, EventArgs e)
        {
            int id = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.EmployeeBank eeBank = DB.EmployeeBanks.Where(u => u.UserID == id).FirstOrDefault();
            bool createNew = false;

            if (eeBank != null)
            {
                eeBank.LastUpdateDatetime = DateTime.Now;
                eeBank.LastUpdateByID = UserInfo.Current.ID;
                eeBank.LastUpdateByName = UserInfo.Current.ChineseName;
            }
            else
            {
                eeBank = new EmployeeBank();
                eeBank.CreateDatetime = DateTime.Now;
                eeBank.CreateByID = UserInfo.Current.ID;
                eeBank.CreateByName = UserInfo.Current.ChineseName;
                eeBank.Default = true;

                createNew = true;
            }

            eeBank.UserID = id;
            eeBank.BankName = tbxBankName.Text.Trim();
            eeBank.AccountHolder = tbxBankUserName.Text.Trim();
            eeBank.BankAccount = tbxBankAccount.Text.Trim();
            eeBank.Remark = tbxBankRemark.Text;

            EmployeeAdjust eeAdjust = new EmployeeAdjust()
            {
                UserID = id,
                AdjustItemName = "银行账号",
                AdjustDate = DateTime.Now,
                AllChangeData = eeBank.ToString(),
                isAdjusted = true,
                CreateByID = UserInfo.Current.ID,
                CreateByName = UserInfo.Current.ChineseName,
                CreateDatetime = DateTime.Now
            };

            DB.EmployeeAdjusts.Add(eeAdjust);

            if (createNew)
                DB.EmployeeBanks.Add(eeBank);

            SaveChanges();

            ShowNotify("保存成功！");
        }

        protected void btnFixPay_Click(object sender, EventArgs e)
        {
            int id = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.EmployeeFixPay fixPay = DB.EmployeeFixPays.Where(u => u.UserID == id).FirstOrDefault();
            bool createNew = false;

            if (fixPay != null)
            {
                fixPay.LastUpdateDatetime = DateTime.Now;
                fixPay.LastUpdateByID = UserInfo.Current.ID;
                fixPay.LastUpdateByName = UserInfo.Current.ChineseName;
            }
            else
            {
                fixPay = new EmployeeFixPay();
                fixPay.CreateDatetime = DateTime.Now;
                fixPay.CreateByID = UserInfo.Current.ID;
                fixPay.CreateByName = UserInfo.Current.ChineseName;
                createNew = true;
            }

            fixPay.UserID = id;
            fixPay.ProbationFixPayValue = Change.ToDecimal(tbxProbationFixPayValue.Text);
            fixPay.FixPayValue = Change.ToDecimal(tbxFixPayValue.Text);
            fixPay.JobAllowanceValue = Change.ToDecimal(tbxJobAllowanceValue.Text);
            fixPay.TrafficAllowanceValue = Change.ToDecimal(tbxTrafficAllowanceValue.Text);
            fixPay.DiningAllowanceValue = Change.ToDecimal(tbxDiningAllowanceValue.Text);

            EmployeeAdjust eeAdjust = new EmployeeAdjust()
            {
                UserID = id,
                AdjustItemName = "固定薪酬",
                AdjustDate = DateTime.Now,
                AllChangeData = fixPay.ToString(),
                isAdjusted = true,
                CreateByID = UserInfo.Current.ID,
                CreateByName = UserInfo.Current.ChineseName,
                CreateDatetime = DateTime.Now
            };

            DB.EmployeeAdjusts.Add(eeAdjust);

            if (createNew)
                DB.EmployeeFixPays.Add(fixPay);

            SaveChanges();

            ShowNotify("保存成功！");
        }
    }
}
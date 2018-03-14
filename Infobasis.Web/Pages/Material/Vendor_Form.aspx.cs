using FineUIPro;
using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Material
{
    public partial class Vendor_Form : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();

                string paramName = Request.QueryString["name"];
                if (!String.IsNullOrEmpty(paramName))
                {
                    //labUserName.Text = paramName;
                }
            }

        }

        protected void logoImgUpload_FileSelected(object sender, EventArgs e)
        {
            if (logoImgUpload.HasFile)
            {
                int companyID = UserInfo.Current.CompanyID;
                int userID = UserInfo.Current.ID;

                string fileOriginalName = logoImgUpload.ShortFileName;

                if (!ValidateFileType(fileOriginalName))
                {
                    ShowNotify("无效的文件类型！");
                    return;
                }

                string uploadPath = Global.UploadFolderPath;
                if (uploadPath.StartsWith("~") || uploadPath.StartsWith(".")) //相对路径
                    uploadPath = HttpContext.Current.Server.MapPath(uploadPath + "/images/vendor/" + companyID.ToString());
                else
                    uploadPath = uploadPath + "/images/vendor/" + companyID;

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

                logoImgUpload.SaveAs(fileOriginalSavePath);

                System.Drawing.Image originalImage = Infobasis.Web.Util.StreamHelper.ImagePath2Img(fileOriginalSavePath);
                string fileThumbnailSavePath = Path.Combine(thumbnailFolderPath, fileName + fileType);
                System.Drawing.Image newImage = ImageHelper.GetThumbNailImage(originalImage, 160, 160);
                newImage.Save(fileThumbnailSavePath);

                string savedPath = Global.UploadFolderVirualPath + "/images/vendor/" + companyID.ToString() + "/" + DateTime.Now.ToString("yyyyMM") + "/thumbnail/" + fileName + fileType;
                Infobasis.Data.DataEntity.User user = DB.Users.Find(userID);
                user.UserPortraitPath = savedPath;
                DB.SaveChanges();

                logoImg.ImageUrl = savedPath;

                // 清空文件上传组件（上传后要记着清空，否则点击提交表单时会再次上传！！）
                logoImgUpload.Reset();
            }

        }

        private void LoadData()
        {
            btnClose.OnClientClick = "parent.removeActiveTab();";
            //btnCloseRefresh.OnClientClick = "parent.removeActiveTab();parent.activeTabAndRefresh('" + Request.QueryString["parenttabid"] + "')";

            InitDropDownMainMaterialType();
            InitDropDownMaterialType(null);
            InitDropDownProvince();
            LoadFormData();

            Grid1.PageSize = UserInfo.Current.DefaultPageSize;
            ddlGridPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();

            BindGrid();

        }

        private void LoadFormData()
        {
            int id = GetQueryIntValue("id");

            if (id > 0)
            {
                Infobasis.Data.DataEntity.Vendor vendor = DB.Vendors.Find(id);
                if (vendor == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！");
                    return;
                }

                tbxCode.Text = vendor.Code;
                tbxName.Text = vendor.Name;
                tbxFullName.Text = vendor.BrandName;
                tbxLocation.Text = vendor.Location;
                tbxDisplayOrder.Text = vendor.DisplayOrder.ToString();
                tbxBandAccount.Text = vendor.BankAccount;
                tbxBandAccountName.Text = vendor.BankAccountName.Trim();
                tbxPaymentNum.Text = vendor.PaymentNum.ToString();
                tbxOpenERPAccount.Checked = vendor.OpenERPAccount;
                tbxBrand.Text = vendor.BrandName;

                DropDownVendorStatus.SelectedValue = ((int)vendor.VendorStatus).ToString();

                if (vendor.ProvinceID.HasValue)
                {
                    DropDownProvince.SelectedValue = vendor.ProvinceID.Value.ToString();
                    DropDownProvince.Text = vendor.ProvinceName;
                }

                if (vendor.MainMaterialTypeID.HasValue)
                {
                    DropDownMainMaterialType.SelectedValue = vendor.MainMaterialTypeID.Value.ToString();
                    DropDownMainMaterialType.Text = vendor.MainMaterialTypeName;
                }

                if (vendor.MaterialTypeID.HasValue)
                {
                    DropDownMaterialType.SelectedValue = vendor.MaterialTypeID.Value.ToString();
                    DropDownMaterialType.Text = vendor.MaterialTypeName;
                }

                tbxRemark.Text = vendor.Remark;

                if (!string.IsNullOrEmpty(vendor.LogoPicPath))
                {
                    logoImg.ImageUrl = vendor.LogoPicPath;
                    logoImgUpload.ButtonText = "修改Logo";
                }
                else
                    logoImgUpload.ButtonText = "上传Logo";

                InitDropDownMaterialType(vendor.MaterialTypeID);
            }
            else
            {
                tbxCode.Text = GenerateNum("Vendor-", false);
            }
        }


        #region
        private void InitDropDownMaterialType(int? selectedVal)
        {
            int mainID = Change.ToInt(DropDownMainMaterialType.SelectedValue);
            string listCode = "";
            EntityList list = DB.EntityLists.Find(mainID);
            if (list != null)
                listCode = list.Code;

            DataTable table = GetEntityListTable(listCode);
            DropDownMaterialType.DataSource = table;
            DropDownMaterialType.DataTextField = "Name";
            DropDownMaterialType.DataValueField = "ID";
            DropDownMaterialType.DataBind();

            DropDownMaterialType.Items.Insert(0, new FineUIPro.ListItem("", "-1"));
            if (selectedVal.HasValue)
                DropDownMaterialType.SelectedValue = selectedVal.ToString();
            //DropDownMaterialType.Items[0].Selected = true;
        }
        #endregion

        #region
        private void InitDropDownMainMaterialType()
        {
            IInfobasisDataSource db = InfobasisDataSource.Create();
            int companyID = UserInfo.Current.CompanyID;
            DataTable table = db.ExecuteTable("SELECT * FROM SYtbEntityList WHERE GroupCode = 'Material' AND CompanyID = @CompanyID ORDER BY DisplayOrder", companyID);
            DropDownMainMaterialType.DataSource = table;
            DropDownMainMaterialType.DataTextField = "Name";
            DropDownMainMaterialType.DataValueField = "ID";
            DropDownMainMaterialType.DataBind();

            DropDownMainMaterialType.Items.Insert(0, new FineUIPro.ListItem("", "-1"));
            //DropDownMainMaterialType.Items[0].Selected = true;
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
            DropDownProvince.Items.Insert(0, new FineUIPro.ListItem("请选择一项", "0"));
        }
        #endregion

        private void BindGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.VendorContact> q = DB.VendorContacts; //.Include(u => u.Dept);

            // 在名称中搜索

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.VendorContact>(q, Grid1);

            Grid1.DataSource = q;
            Grid1.DataBind();
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查

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

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }


        protected string GetEmployeeStatus(object employeeStatus)
        {
            if (Change.ToInt(employeeStatus) == 1)
                return "离职";

            return "在职";            
        }

        private void SaveItem()
        {
            int userID = UserInfo.Current.ID;
            string userName = UserInfo.Current.ChineseName;

            int id = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.Vendor vendor = null;
            if (id > 0)
            {
                vendor = DB.Vendors.Find(id);
                saveFormData(vendor);
                vendor.LastUpdateDatetime = DateTime.Now;
                vendor.LastUpdateByID = userID;
                vendor.LastUpdateByName = userName;
            }
            else
            {
                vendor = new Infobasis.Data.DataEntity.Vendor();

                vendor.CreateDatetime = DateTime.Now;
                vendor.CreateByID = userID;
                vendor.CreateByName = userName;
                saveFormData(vendor);
                DB.Vendors.Add(vendor);
            }

            if (!string.IsNullOrEmpty(tbxERPAccount.Text) && !string.IsNullOrEmpty(tbxERPAccountPwd.Text) && tbxOpenERPAccount.Checked)
            {
                if (!DB.Users.Where(item => item.UserType == UserType.Vendor && item.Name == tbxERPAccount.Text.Trim()).Any())
                {
                    Infobasis.Data.DataEntity.User user = new Infobasis.Data.DataEntity.User();
                    user.Name = tbxERPAccount.Text.Trim();
                    user.Password = PasswordUtil.CreateDbPassword(tbxERPAccountPwd.Text.Trim());
                    user.EmployeeSpellCode = Infobasis.Web.Util.ChinesePinyin.GetPinyin(user.Name);
                    user.FirstSpellCode = Infobasis.Web.Util.ChinesePinyin.GetFirstPinyin(user.Name);
                    user.Enabled = true;
                    user.UserType = UserType.Vendor;
                    user.CreateDatetime = DateTime.Now;

                    DB.Users.Add(user);
                }
                else
                {
                    Infobasis.Data.DataEntity.User user = DB.Users.Where(u => u.UserType == UserType.Vendor && u.Name == tbxERPAccount.Text.Trim()).FirstOrDefault();
                    user.Password = PasswordUtil.CreateDbPassword(tbxERPAccountPwd.Text.Trim());
                    user.Enabled = true;
                    user.LastUpdateDatetime = DateTime.Now;
                }
            }
            else if (!string.IsNullOrEmpty(tbxERPAccount.Text) && !string.IsNullOrEmpty(tbxERPAccountPwd.Text) && !tbxOpenERPAccount.Checked)
            {
                Infobasis.Data.DataEntity.User user = DB.Users.Where(u => u.UserType == UserType.Vendor && u.Name == tbxERPAccount.Text.Trim()).FirstOrDefault();
                if (user != null)
                {
                    user.Password = PasswordUtil.CreateDbPassword(tbxERPAccountPwd.Text.Trim());
                    user.Enabled = false;
                    user.LastUpdateDatetime = DateTime.Now;
                }
            }
            DB.SaveChanges();
        }

        private void saveFormData(Infobasis.Data.DataEntity.Vendor vendor)
        {
            vendor.Name = tbxName.Text.Trim();
            vendor.FullName = tbxFullName.Text.Trim();
            vendor.Location = tbxLocation.Text.Trim();
            vendor.Code = tbxCode.Text.Trim();
            vendor.BankAccount = tbxBandAccount.Text.Trim();
            vendor.BankAccountName = tbxBandAccountName.Text.Trim();
            vendor.PaymentNum = Change.ToInt(tbxPaymentNum.Text);
            vendor.ERPAccount = tbxERPAccount.Text.Trim();
            vendor.ERPPassword = tbxERPAccountPwd.Text.Trim();
            vendor.OpenERPAccount = tbxOpenERPAccount.Checked;
            vendor.BrandName = tbxBrand.Text.Trim();

            vendor.VendorStatus = (VendorStatus)Enum.Parse(typeof(VendorStatus), DropDownVendorStatus.SelectedValue);

            vendor.MainMaterialTypeName = DropDownMainMaterialType.SelectedText;

            if (Infobasis.Web.Util.Change.ToInt(DropDownMainMaterialType.SelectedValue) > 0)
                vendor.MainMaterialTypeID = Infobasis.Web.Util.Change.ToInt(DropDownMainMaterialType.SelectedValue);

            vendor.MaterialTypeName = DropDownMaterialType.SelectedText;

            if (Infobasis.Web.Util.Change.ToInt(DropDownMaterialType.SelectedValue) > 0)
                vendor.MaterialTypeID = Infobasis.Web.Util.Change.ToInt(DropDownMaterialType.SelectedValue);

            if (Infobasis.Web.Util.Change.ToInt(DropDownProvince.SelectedValue) > 0)
            {
                vendor.ProvinceID = Infobasis.Web.Util.Change.ToInt(DropDownProvince.SelectedValue);
                vendor.ProvinceName = DropDownProvince.SelectedText;
            }

            vendor.Remark = tbxRemark.Text;
            vendor.SpellCode = ChinesePinyin.GetPinyin(vendor.Name);
            vendor.FirstSpellCode = ChinesePinyin.GetFirstPinyin(vendor.Name);
            vendor.LogoPicPath = logoImg.ImageUrl;
            vendor.CompanySize = DropDownCompanySize.SelectedText;
            vendor.DisplayOrder = Change.ToInt(tbxDisplayOrder.Text);
            vendor.IsActive = vendor.VendorStatus == VendorStatus.Qualified || vendor.VendorStatus == VendorStatus.Temporary ? true : false;
        }

        protected void btnCloseRefresh_Click(object sender, EventArgs e)
        {
            SaveItem();
            ShowNotify("保存成功！");
            PageContext.RegisterStartupScript("parent.removeActiveTab();parent.activeTabAndRefresh('" + Request.QueryString["parenttabid"] + "')");
        }

        protected void DropDownMainMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDropDownMaterialType(null);
            DropDownMaterialType.SelectedValue = null;
            DropDownMaterialType.Text = "";

        }

        protected void btnContinueToAdd_Click(object sender, EventArgs e)
        {
            SaveItem();
            ShowNotify("保存成功！");
            PageContext.RegisterStartupScript("refreshWindow();");
        }

    }
}
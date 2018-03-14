using Infobasis.Web.Data;
using WebUtil = Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Drawing;
using Infobasis.Web.Util;
using System.Data;
using Infobasis.Data.DataAccess;
using FineUIPro;
using Infobasis.Data.DataEntity;

namespace Infobasis.Web.Pages.Material
{
    public partial class Material_Form : PageBase
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

        protected void materialImgUpload_FileSelected(object sender, EventArgs e)
        {
            if (materialImgUpload.HasFile)
            {
                int companyID = UserInfo.Current.CompanyID;
                int userID = UserInfo.Current.ID;

                string fileOriginalName = materialImgUpload.ShortFileName;

                if (!ValidateFileType(fileOriginalName))
                {
                    ShowNotify("无效的文件类型！");
                    return;
                }

                string uploadPath = Global.UploadFolderPath;
                if (uploadPath.StartsWith("~") || uploadPath.StartsWith(".")) //相对路径
                    uploadPath = HttpContext.Current.Server.MapPath(uploadPath + "/images/material/" + companyID.ToString());
                else
                    uploadPath = uploadPath + "/images/material/" + companyID;

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

                materialImgUpload.SaveAs(fileOriginalSavePath);

                System.Drawing.Image originalImage = Infobasis.Web.Util.StreamHelper.ImagePath2Img(fileOriginalSavePath);
                string fileThumbnailSavePath = Path.Combine(thumbnailFolderPath, fileName + fileType);
                System.Drawing.Image newImage = ImageHelper.GetThumbNailImage(originalImage, 160, 160);
                newImage.Save(fileThumbnailSavePath);

                string savedPath = Global.UploadFolderVirualPath + "/images/material/" + companyID.ToString() + "/" + DateTime.Now.ToString("yyyyMM") + "/thumbnail/" + fileName + fileType;
                Infobasis.Data.DataEntity.User user = DB.Users.Find(userID);
                user.UserPortraitPath = savedPath;
                DB.SaveChanges();

                materialImg.ImageUrl = savedPath;

                // 清空文件上传组件（上传后要记着清空，否则点击提交表单时会再次上传！！）
                materialImgUpload.Reset();
            }

        }

        private void LoadData()
        {
            btnClose.OnClientClick = "parent.removeActiveTab();";
            //btnCloseRefresh.OnClientClick = "parent.removeActiveTab();parent.activeTabAndRefresh('" + Request.QueryString["parenttabid"] + "')";

            InitDropDownMainMaterialType();
            InitDropDownMaterialType(null);
            InitCheckBoxListBudgetType();
            InitCheckBoxListRoomType();
            InitDropDownUnit();
            InitDropDownBrand();
            InitDropDownProvince();

            BindVendorGrid();
            LoadFormData();
        }

        private void LoadFormData()
        {
            int id = GetQueryIntValue("id");

            if (id > 0)
            {
                Infobasis.Data.DataEntity.Material material = DB.Materials.Find(id);
                if (material == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！");
                    return;
                }

                tbxCode.Text = material.Code;
                tbxName.Text = material.Name;
                tbxBrand.Text = material.BrandName;
                tbxBrandHidden.Text = material.BrandID.HasValue ? material.BrandID.ToString() : "";
                tbxModel.Text = material.Model;
                tbxSpec.Text = material.Spec;
                tbxIsActive.Checked = material.IsActive;
                tbxPurchasePrice.Text = material.PurchasePrice.ToString();
                tbxEarningFactor.Text = material.EarningFactor.ToString();
                tbxSalePrice.Text = material.SalePrice.ToString();
                tbxNoSalePrice.Checked = material.NoSalePrice;
                tbxReturnFactor.Text = material.ReturnFactor.ToString();
                tbxUpgradePrice.Text = material.UpgradePrice.ToString();

                if (material.ProvinceID.HasValue)
                {
                    DropDownProvince.SelectedValue = material.ProvinceID.Value.ToString();
                    DropDownProvince.Text = material.ProvinceName;
                }

                if (material.UnitID.HasValue)
                {
                    DropDownUnit.SelectedValue = material.UnitID.Value.ToString();
                    DropDownUnit.Text = material.UnitName;
                }

                if (material.CustomizationTypeID.HasValue)
                {
                    DropDownCustomizationType.SelectedValue = material.CustomizationTypeID.Value.ToString();
                    DropDownCustomizationType.Text = material.CustomizationTypeName;
                }

                if (material.MainMaterialTypeID.HasValue)
                {
                    DropDownMainMaterialType.SelectedValue = material.MainMaterialTypeID.Value.ToString();
                    DropDownMainMaterialType.Text = material.MainMaterialTypeName;
                }

                if (material.MaterialTypeID.HasValue)
                {
                    DropDownMaterialType.SelectedValue = material.MaterialTypeID.Value.ToString();
                    DropDownMaterialType.Text = material.MaterialTypeName;
                }

                if (!string.IsNullOrEmpty(material.BudgetTypeIDs))
                {
                    DropDownBoxBudgetType.Values = material.BudgetTypeIDs.Split(',');
                    DropDownBoxBudgetType.Text = material.BudgetTypeNames;

                    CheckBoxListBudgetType.SelectedValueArray = DropDownBoxBudgetType.Values;
                }

                if (!string.IsNullOrEmpty(material.RoomTypeIDs))
                {
                    DropDownBoxRoomType.Values = material.RoomTypeIDs.Split(',');
                    DropDownBoxRoomType.Text = material.RoomTypeNames;
                    CheckBoxListRoomType.SelectedValueArray = DropDownBoxRoomType.Values;
                }

                if (material.VendorID.HasValue)
                {
                    ddbVendor.Value = material.VendorID.ToString();
                    ddbVendor.Text = material.VendorName;
                }

                tbxRemark.Text = material.Remark;

                if (material.VendorID != null)
                {
                    ddbVendor.Value = material.VendorID.ToString();
                    ddbVendor.Text = material.VendorName;
                }

                if (!string.IsNullOrEmpty(material.PicPath))
                {
                    materialImg.ImageUrl = material.PicPath;
                    materialImgUpload.ButtonText = "修改图片";
                }
                else
                    materialImgUpload.ButtonText = "上传图片";

                InitDropDownMaterialType(material.MaterialTypeID);
            }
            else
            {
                tbxIsActive.Checked = true;
                tbxCode.Text = GenerateNum("MA-", false);
            }
        }

        private void BindVendorGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.Vendor> q = DB.Vendors; //.Include(u => u.Dept);

            // 在用户名称中搜索
            string searchText = ttbSearch.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText) || u.Code.Contains(searchText));
            }

            // 过滤启用状态
            if (rblEnableStatus.SelectedValue != "all")
            {
                q = q.Where(u => u.IsActive == (rblEnableStatus.SelectedValue == "enabled" ? true : false));
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid2.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.Vendor>(q, Grid2);

            Grid2.DataSource = q;
            Grid2.DataBind();
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

        #region
        private void InitCheckBoxListBudgetType()
        {
            IInfobasisDataSource db = InfobasisDataSource.Create();
            int companyID = UserInfo.Current.CompanyID;
            DataTable table = GetEntityListTable("YSLX");
            CheckBoxListBudgetType.DataSource = table;
            CheckBoxListBudgetType.DataTextField = "Name";
            CheckBoxListBudgetType.DataValueField = "ID";
            CheckBoxListBudgetType.DataBind();
        }
        #endregion

        #region
        private void InitCheckBoxListRoomType()
        {
            IInfobasisDataSource db = InfobasisDataSource.Create();
            int companyID = UserInfo.Current.CompanyID;
            DataTable table = GetEntityListTable("FJBW");
            CheckBoxListRoomType.DataSource = table;
            CheckBoxListRoomType.DataTextField = "Name";
            CheckBoxListRoomType.DataValueField = "ID";
            CheckBoxListRoomType.DataBind();
        }
        #endregion

        #region
        private void InitDropDownUnit()
        {
            DataTable table = GetEntityListTable("Unit");
            DropDownUnit.DataSource = table;
            DropDownUnit.DataTextField = "Name";
            DropDownUnit.DataValueField = "ID";
            DropDownUnit.DataBind();

            DropDownUnit.Items.Insert(0, new FineUIPro.ListItem("", "-1"));
            DropDownUnit.Items[0].Selected = true;
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

        #region
        private void InitDropDownBrand()
        {

        }
        #endregion

        private void SaveItem()
        {
            int userID = UserInfo.Current.ID;
            string userName = UserInfo.Current.ChineseName;

            int id = GetQueryIntValue("id");
            if (id > 0)
            {
                Infobasis.Data.DataEntity.Material material = DB.Materials.Find(id);
                material.Name = tbxName.Text.Trim();
                material.Code = tbxCode.Text.Trim();
                material.IsActive = tbxIsActive.Checked;
                material.BrandName = tbxBrand.Text;
                if (Infobasis.Web.Util.Change.ToInt(tbxBrandHidden.Text) > 0)
                    material.BrandID = Infobasis.Web.Util.Change.ToInt(tbxBrandHidden.Text);

                material.Model = tbxModel.Text;
                material.Spec = tbxSpec.Text;
                material.UnitName = DropDownUnit.SelectedText;
                if (Infobasis.Web.Util.Change.ToInt(DropDownUnit.SelectedValue) > 0)
                    material.UnitID = Infobasis.Web.Util.Change.ToInt(DropDownUnit.SelectedValue);

                material.PurchasePrice = Infobasis.Web.Util.Change.ToDecimal(tbxPurchasePrice.Text);
                material.SalePrice = Infobasis.Web.Util.Change.ToDecimal(tbxSalePrice.Text);
                material.NoSalePrice = tbxNoSalePrice.Checked;
                material.ReturnFactor = Infobasis.Web.Util.Change.ToDecimal(tbxReturnFactor.Text);
                material.UpgradePrice = Infobasis.Web.Util.Change.ToDecimal(tbxUpgradePrice.Text);
                material.CustomizationTypeName = DropDownCustomizationType.SelectedText;

                if (Infobasis.Web.Util.Change.ToInt(DropDownCustomizationType.SelectedValue) > 0)
                    material.CustomizationTypeID = Infobasis.Web.Util.Change.ToInt(DropDownCustomizationType.SelectedValue);

                material.EarningFactor = Infobasis.Web.Util.Change.ToDecimal(tbxEarningFactor.Text);
                material.MainMaterialTypeName = DropDownMainMaterialType.SelectedText;

                if (Infobasis.Web.Util.Change.ToInt(DropDownMainMaterialType.SelectedValue) > 0)
                    material.MainMaterialTypeID = Infobasis.Web.Util.Change.ToInt(DropDownMainMaterialType.SelectedValue);
                
                material.MaterialTypeName = DropDownMaterialType.SelectedText;

                if (Infobasis.Web.Util.Change.ToInt(DropDownMaterialType.SelectedValue) > 0)
                    material.MaterialTypeID = Infobasis.Web.Util.Change.ToInt(DropDownMaterialType.SelectedValue);

                if (Infobasis.Web.Util.Change.ToInt(DropDownProvince.SelectedValue) > 0)
                {
                    material.ProvinceID = Infobasis.Web.Util.Change.ToInt(DropDownProvince.SelectedValue);
                    material.ProvinceName = DropDownProvince.SelectedText;
                }


                material.BudgetTypeIDs = string.Join(",", DropDownBoxBudgetType.Values);
                material.BudgetTypeNames = string.Join(",", CheckBoxListBudgetType.SelectedItemArray.Where(item => item.Selected).Select(item => item.Text).ToArray());

                material.RoomTypeIDs = string.Join(",", DropDownBoxRoomType.Values);
                material.RoomTypeNames = string.Join(",", CheckBoxListRoomType.SelectedItemArray.Where(item => item.Selected).Select(item => item.Text).ToArray());

                material.Remark = tbxRemark.Text;
                material.SpellCode = ChinesePinyin.GetPinyin(material.Name);
                material.FirstSpellCode = ChinesePinyin.GetFirstPinyin(material.Name);
                material.PicPath = materialImg.ImageUrl;

                if (Infobasis.Web.Util.Change.ToInt(ddbVendor.Value) > 0)
                {
                    material.VendorID = Infobasis.Web.Util.Change.ToInt(ddbVendor.Value);
                    material.VendorName = ddbVendor.Text;
                }

                material.LastUpdateDatetime = DateTime.Now;
                material.LastUpdateByID = userID;
                material.LastUpdateByName = userName;
            }
            else
            {
                Infobasis.Data.DataEntity.Material material = new Infobasis.Data.DataEntity.Material();
                material.Name = tbxName.Text.Trim();
                material.Code = tbxCode.Text.Trim();
                material.IsActive = tbxIsActive.Checked;
                material.BrandName = tbxBrand.Text;
                if (Infobasis.Web.Util.Change.ToInt(tbxBrandHidden.Text) > 0)
                    material.BrandID = Infobasis.Web.Util.Change.ToInt(tbxBrandHidden.Text);

                material.Model = tbxModel.Text;
                material.Spec = tbxSpec.Text;
                material.UnitName = DropDownUnit.SelectedText;
                if (Infobasis.Web.Util.Change.ToInt(DropDownUnit.SelectedValue) > 0)
                    material.UnitID = Infobasis.Web.Util.Change.ToInt(DropDownUnit.SelectedValue);

                material.PurchasePrice = Infobasis.Web.Util.Change.ToDecimal(tbxPurchasePrice.Text);
                material.SalePrice = Infobasis.Web.Util.Change.ToDecimal(tbxSalePrice.Text);
                material.NoSalePrice = tbxNoSalePrice.Checked;
                material.ReturnFactor = Infobasis.Web.Util.Change.ToDecimal(tbxReturnFactor.Text);
                material.UpgradePrice = Infobasis.Web.Util.Change.ToDecimal(tbxUpgradePrice.Text);
                material.CustomizationTypeName = DropDownCustomizationType.SelectedText;

                if (Infobasis.Web.Util.Change.ToInt(DropDownCustomizationType.SelectedValue) > 0)
                    material.CustomizationTypeID = Infobasis.Web.Util.Change.ToInt(DropDownCustomizationType.SelectedValue);

                material.EarningFactor = Infobasis.Web.Util.Change.ToDecimal(tbxEarningFactor.Text);
                material.MainMaterialTypeName = DropDownMainMaterialType.SelectedText;

                if (Infobasis.Web.Util.Change.ToInt(DropDownMainMaterialType.SelectedValue) > 0)
                    material.MainMaterialTypeID = Infobasis.Web.Util.Change.ToInt(DropDownMainMaterialType.SelectedValue);

                material.MaterialTypeName = DropDownMaterialType.SelectedText;

                if (Infobasis.Web.Util.Change.ToInt(DropDownMaterialType.SelectedValue) > 0)
                    material.MaterialTypeID = Infobasis.Web.Util.Change.ToInt(DropDownMaterialType.SelectedValue);

                if (Infobasis.Web.Util.Change.ToInt(DropDownProvince.SelectedValue) > 0)
                {
                    material.ProvinceID = Infobasis.Web.Util.Change.ToInt(DropDownProvince.SelectedValue);
                    material.ProvinceName = DropDownProvince.SelectedText;
                }

                material.BudgetTypeIDs = string.Join(",", DropDownBoxBudgetType.Values);
                material.BudgetTypeNames = string.Join(",", CheckBoxListBudgetType.SelectedItemArray.Where(item => item.Selected).Select(item => item.Text).ToArray());

                material.RoomTypeIDs = string.Join(",", DropDownBoxRoomType.Values);
                material.RoomTypeNames = string.Join(",", CheckBoxListRoomType.SelectedItemArray.Where(item => item.Selected).Select(item => item.Text).ToArray());

                material.Remark = tbxRemark.Text;
                material.SpellCode = ChinesePinyin.GetPinyin(material.Name);
                material.FirstSpellCode = ChinesePinyin.GetFirstPinyin(material.Name);
                material.PicPath = materialImg.ImageUrl;

                if (Infobasis.Web.Util.Change.ToInt(ddbVendor.Value) > 0)
                {
                    material.VendorID = Infobasis.Web.Util.Change.ToInt(ddbVendor.Value);
                    material.VendorName = ddbVendor.Text;
                }

                material.CreateDatetime = DateTime.Now;
                material.CreateByID = userID;
                material.CreateByName = userName;
                DB.Materials.Add(material);
            }
            if (!string.IsNullOrEmpty(tbxBrand.Text))
            {
                if (!DB.Brands.Where(item => item.Name == tbxBrand.Text.Trim()).Any())
                {
                    Infobasis.Data.DataEntity.Brand brand = new Infobasis.Data.DataEntity.Brand();
                    brand.Code = GenerateNum("brand");
                    brand.Name = tbxBrand.Text.Trim();
                    brand.SpellCode = Infobasis.Web.Util.ChinesePinyin.GetPinyin(brand.Name);
                    brand.FirstSpellCode = Infobasis.Web.Util.ChinesePinyin.GetFirstPinyin(brand.Name);
                    brand.IsActive = true;
                    brand.CreateDatetime = DateTime.Now;

                    DB.Brands.Add(brand);
                }
            }
            DB.SaveChanges();
        }

        protected void btnCloseRefresh_Click(object sender, EventArgs e)
        {
            SaveItem();
            ShowNotify("保存成功！");
            PageContext.RegisterStartupScript("parent.removeActiveTab();parent.activeTabAndRefresh('" + Request.QueryString["parenttabid"] + "')");
        }


        protected void Grid2_PageIndexChange(object sender, GridPageEventArgs e)
        {
            //Grid1.PageIndex = e.NewPageIndex;

            BindVendorGrid();
        }

        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            //Grid1.SortDirection = e.SortDirection;
            //Grid1.SortField = e.SortField;

            BindVendorGrid();
        }


        protected void ttbSearch_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearch.Text = String.Empty;
            ttbSearch.ShowTrigger1 = false;

            BindVendorGrid();
        }

        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearch.ShowTrigger1 = true;

            BindVendorGrid();
        }

        protected void rblEnableStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindVendorGrid();
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
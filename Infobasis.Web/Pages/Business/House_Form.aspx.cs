using FineUIPro;
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

namespace Infobasis.Web.Pages.Business
{
    public partial class House_Form : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #region InitDropDownProvince
        private void InitDropDownProvince()
        {
            IQueryable table = DB.Provinces.Where(item => item.IsActive == true).OrderBy(item => item.DisplayOrder);
            DropDownProvince.DataSource = table;
            DropDownProvince.DataTextField = "Name";
            DropDownProvince.DataValueField = "ID";
            DropDownProvince.DataBind();
            DropDownProvince.Items.Insert(0, new FineUIPro.ListItem("请选择城市", "-1"));
        }
        #endregion

        #region InitDropDownCity
        private void InitDropDownCity()
        {
            int provinceID = 0;
            if (DropDownProvince.SelectedValue != null)
                provinceID = Infobasis.Web.Util.Change.ToInt(DropDownProvince.SelectedValue);

            IQueryable table = DB.Citys.Where(item => item.ProvinceID == provinceID && item.IsActive == true).OrderBy(item => item.DisplayOrder);
            DropDownCity.DataSource = table;
            DropDownCity.DataTextField = "Name";
            DropDownCity.DataValueField = "ID";
            DropDownCity.DataBind();
            DropDownCity.Items.Insert(0, new FineUIPro.ListItem("请选择区域", "-1"));
        }
        #endregion

        #region InitDropDownHouseType
        private void InitDropDownHouseType()
        {
            DataTable table = GetEntityListTable("HomeType");
            DropDownHouseType.DataSource = table;
            DropDownHouseType.DataTextField = "Name";
            DropDownHouseType.DataValueField = "ID";
            DropDownHouseType.DataBind();
            DropDownHouseType.Items.Insert(0, new FineUIPro.ListItem("请选择一项", "-1"));
        }
        #endregion

        private void LoadData()
        {
            btnClose.OnClientClick = "closeAndRefreshTopWindow();";

            int id = GetQueryIntValue("id");
            if (id > 0)
            {
                HouseInfo house = DB.HouseInfos.Where(d => d.ID == id).FirstOrDefault();
                if (house != null)
                {
                    tbxName.Text = house.Name;
                    tbxHouseNum.Text = house.HouseNum.ToString();
                    tbxPrice.Text = house.Price.ToString();
                    tbxRemark.Text = house.Remark;
                    tbxLocation.Text = house.Location;
                    DropDownProvince.SelectedValue = house.ProvinceID.ToString();
                    DropDownCity.SelectedValue = house.CityID.ToString();
                    tbxStartDate.Text = house.StartDate.HasValue ? house.StartDate.Value.ToString() : "";
                    tbxCompletionDate.Text = house.CompletionDate.HasValue ? house.CompletionDate.Value.ToString() : "";

                    if (house.HouseTypeID != null)
                    {
                        DropDownHouseType.SelectedValue = house.HouseTypeID.ToString();
                    }

                    cbxIsImportant.Checked = house.IsImportant;
                }
            }

            InitDropDownProvince();
            InitDropDownCity();
            InitDropDownHouseType();
        }

        #region Events

        private void SaveItem()
        {
            HouseInfo item = null;
            int id = GetQueryIntValue("id");
            if (id > 0)
            {
                item = DB.HouseInfos.Find(id);
                item.LastUpdateByID = UserInfo.Current.ID;
                item.LastUpdateByName = UserInfo.Current.ChineseName;
                item.LastUpdateDatetime = DateTime.Now;
            }
            else
            {
                item = new HouseInfo();
                item.CreateByID = UserInfo.Current.ID;
                item.CreateByName = UserInfo.Current.ChineseName;
                item.CreateDatetime = DateTime.Now;
            }

            item.Name = tbxName.Text.Trim();
            item.NameSpellCode = Util.ChinesePinyin.GetFirstPinyin(tbxName.Text.Trim());
            item.Price = Convert.ToInt32(tbxPrice.Text.Trim());
            item.HouseNum = Convert.ToInt32(tbxHouseNum.Text.Trim());
            item.Location = tbxLocation.Text.Trim();

            int provinceID = Change.ToInt(DropDownProvince.SelectedValue);
            int cityID = Change.ToInt(DropDownCity.SelectedValue);
            int houseTypeID = Change.ToInt(DropDownHouseType.SelectedValue);

            if (provinceID > 0)
            {
                item.ProvinceID = Change.ToInt(DropDownProvince.SelectedValue);
                item.ProvinceName = DropDownProvince.SelectedText;
            }
            else
            {
                item.ProvinceID = null;
                item.ProvinceName = "";
            }

            if (cityID > 0)
            {
                item.CityID = Change.ToInt(DropDownCity.SelectedValue);
                item.CityName = DropDownCity.SelectedText;
            }
            else
            {
                item.CityID = null;
                item.CityName = "";
            }

            if (houseTypeID > 0)
            {
                item.HouseTypeID = Change.ToInt(DropDownHouseType.SelectedValue);
                item.HouseTypeName = DropDownHouseType.SelectedText;
            }
            else
            {
                item.ProvinceID = null;
                item.ProvinceName = "";
            }

            item.IsImportant = cbxIsImportant.Checked;
            item.StartDate = Change.ToDateTime(tbxStartDate.Text);
            item.CompletionDate = Change.ToDateTime(tbxCompletionDate.Text);

            if (item.StartDate == DateTime.MinValue)
                item.StartDate = null;

            if (item.CompletionDate == DateTime.MinValue)
                item.CompletionDate = null;

            if (id == 0)
            {
                DB.HouseInfos.Add(item);
            }
            SaveChanges();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();

            //Alert.Show("添加成功！", String.Empty, ActiveWindow.GetHidePostBackReference());
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void btnSaveContinue_Click(object sender, EventArgs e)
        {
            // 1. 这里放置保存窗体中数据的逻辑
            SaveItem();

            // 2. 关闭本窗体，然后回发父窗体
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());

            PageContext.RegisterStartupScript("F.notify({message:'添加成功！',messageIcon:'information',target:'_top',header:false,displayMilliseconds:3000,positionX:'center',positionY:'top'}); window.location.reload(false);");
        }

        #endregion

        protected void DropDownProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDropDownCity();
        }
    }
}
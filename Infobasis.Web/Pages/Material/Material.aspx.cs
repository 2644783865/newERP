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

namespace Infobasis.Web.Pages.Material
{
    public partial class Material : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                btnReset.OnClientClick = SimpleForm1.GetResetReference();

                JsObjectBuilder joBuilder = new JsObjectBuilder();
                joBuilder.AddProperty("id", "grid_newtab_material_addnew");
                joBuilder.AddProperty("title", "新增材料");
                joBuilder.AddProperty("iframeUrl", "getNewWindowUrl()", true);
                joBuilder.AddProperty("refreshWhenExist", true);
                joBuilder.AddProperty("iconFont", "plus");
                btnNew.OnClientClick = String.Format("parent.addTab({0});", joBuilder);
            }
        }

        private void LoadData()
        {
            // 权限检查

            //btnNew.OnClientClick = Window1.GetShowReference("~/Pages/Admin/User_Form.aspx", "新增用户");

            // 每页记录数
            Grid1.PageSize = UserInfo.Current.DefaultPageSize;
            ddlGridPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();

            InitDropDownProvince();
            InitDropDownMainMaterialType();
            InitDropDownMaterialType(null);
            InitCheckBoxListBudgetType();

            BindGrid();
        }


        private void BindGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.Material> q = DB.Materials.Include("Brand"); //.Include(u => u.Dept);

            // 在用户名称中搜索
            string searchText = tbxName.Text.Trim();
            int provinceID = Change.ToInt(DropDownProvince.SelectedValue);
            int mainMaterialTypeID = Change.ToInt(DropDownMainMaterialType.SelectedValue);
            int materialTypeID = Change.ToInt(DropDownMaterialType.SelectedValue);
            List<string> exceptList = new List<string>();
            exceptList.Add("0");
            string[] selectedCustomTypes = DropDownCustomType.SelectedValueArray;
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText));
            }

            if (provinceID > 0)
                q = q.Where(u => u.ProvinceID == provinceID);

            if (mainMaterialTypeID > 0)
                q = q.Where(u => u.MainMaterialTypeID == mainMaterialTypeID);

            if (materialTypeID > 0)
                q = q.Where(u => u.MaterialTypeID == materialTypeID);

            if (selectedCustomTypes != null && selectedCustomTypes.Length > 0)
            {
                foreach (string k in selectedCustomTypes)
                {
                    if (k != "0")
                        q = q.Where(u => u.BudgetTypeIDs == k || u.BudgetTypeIDs.StartsWith(k + ",") || u.BudgetTypeIDs.Contains("," + k + ",") || u.BudgetTypeIDs.EndsWith("," + k));
                }
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.Material>(q, Grid1);

            Grid1.DataSource = q;
            Grid1.DataBind();
        }

        #region Events

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

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }

        #region
        private void InitCheckBoxListBudgetType()
        {
            IInfobasisDataSource db = InfobasisDataSource.Create();
            int companyID = UserInfo.Current.CompanyID;
            DataTable table = GetEntityListTable("YSLX");
            DropDownCustomType.DataSource = table;
            DropDownCustomType.DataTextField = "Name";
            DropDownCustomType.DataValueField = "ID";
            DropDownCustomType.DataBind();
            DropDownCustomType.Items.Insert(0, new FineUIPro.ListItem("", "0"));
        }
        #endregion

        #endregion

        protected void DropDownMainMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDropDownMaterialType(null);
            DropDownMaterialType.SelectedValue = null;
            DropDownMaterialType.Text = "";

        }


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

        protected void btnRest_Click(object sender, EventArgs e)
        {
            SimpleForm1.Reset();
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            Infobasis.Data.DataEntity.Material material = e.DataItem as Infobasis.Data.DataEntity.Material;
        }

        protected string GetEditUrl(object id, object name)
        {
            JsObjectBuilder joBuilder = new JsObjectBuilder();
            joBuilder.AddProperty("id", "grid_newtab_material_edit");
            joBuilder.AddProperty("title", "编辑 - " + name);
            joBuilder.AddProperty("iframeUrl", String.Format("getEditWindowUrl('{0}','{1}')", id, HttpUtility.UrlEncode(name.ToString())), true); // ResolveUrl(String.Format("~/grid/grid_newtab_window.aspx?id={0}&name={1}", id, HttpUtility.UrlEncode(name.ToString()))));
            joBuilder.AddProperty("refreshWhenExist", true);
            joBuilder.AddProperty("iconFont", "pencil");

            // addExampleTab函数定义在default.aspx，参数分别为：id, url, text, icon, refreshWhenExist
            return String.Format("parent.addTab({0});", joBuilder);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {

        }
    }
}
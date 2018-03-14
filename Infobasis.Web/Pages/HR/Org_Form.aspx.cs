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

namespace Infobasis.Web.Pages.HR
{
    public partial class Org_Form : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                BindEmployeeGrid();
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
            DropDownProvince.Items.Insert(0, new FineUIPro.ListItem("请选择区域", "-1"));
        }
        #endregion

        private void LoadData()
        {
            btnClose.OnClientClick = "closeAndRefreshTopWindow();";

            int id = GetQueryIntValue("id");
            if (id > 0)
            {
                Department dept = DB.Departments.Include("ParentDepartment").Where(d => d.ID == id).FirstOrDefault();
                if (dept != null)
                {
                    tbxName.Text = dept.Name;
                    tbxDisplayOrder.Text = dept.DisplayOrder.ToString();
                    tbxRemark.Text = dept.Description;
                    if (dept.ParentID != null)
                    {
                        ddbParent.Value = dept.ParentID.ToString();
                        ddbParent.Text = dept.ParentDepartment.Name;
                    }

                    if (dept.LeaderID != null)
                    {
                        ddbLeader.Value = dept.LeaderID.ToString();
                        ddbLeader.Text = dept.LeaderName;
                    }

                    if (dept.DepartmentControlType != null)
                    {
                        ddbControlType.Value = dept.DepartmentControlType.ToString();
                        ddbControlType.Text = dept.DepartmentControlTypeName;
                    }

                    if (dept.ProvinceID != null)
                    {
                        DropDownProvince.SelectedValue = dept.ProvinceID.ToString();
                    }

                    cbxEnabled.Checked = dept.Enabled;
                }
            }

            BindDDL();
            BindEmployeeGrid();
            InitDropDownProvince();
        }


        private void BindDDL()
        {
            Grid1.DataSource = DB.Departments.OrderBy(d => d.DisplayOrder).ToList();
            Grid1.DataBind();
        }

        private void BindEmployeeGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.User> q = DB.Users; //.Include(u => u.Dept);

            // 在用户名称中搜索
            string searchText = ttbSearch.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.ChineseName.Contains(searchText) || u.EnglishName.Contains(searchText));
            }

            // 过滤启用状态
            if (rblEnableStatus.SelectedValue != "all")
            {
                q = q.Where(u => u.Enabled == (rblEnableStatus.SelectedValue == "enabled" ? true : false));
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid2.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.User>(q, Grid2);

            Grid2.DataSource = q;
            Grid2.DataBind();        
        }

        #region Events

        protected void Grid2_PageIndexChange(object sender, GridPageEventArgs e)
        {
            //Grid1.PageIndex = e.NewPageIndex;

            BindEmployeeGrid();
        }

        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            //Grid1.SortDirection = e.SortDirection;
            //Grid1.SortField = e.SortField;

            BindEmployeeGrid();
        }


        protected void ttbSearch_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearch.Text = String.Empty;
            ttbSearch.ShowTrigger1 = false;

            BindEmployeeGrid();
        }

        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearch.ShowTrigger1 = true;

            BindEmployeeGrid();
        }

        protected void rblEnableStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEmployeeGrid();
        }

        private void SaveItem()
        {
            Department item = null;
            int id = GetQueryIntValue("id");
            if (id > 0)
            {
                item = DB.Departments.Find(id);
                item.LastUpdateByID = UserInfo.Current.ID;
                item.LastUpdateByName = UserInfo.Current.ChineseName;
                item.LastUpdateDatetime = DateTime.Now;
            }
            else
            {
                item = new Department();
            }

            item.Name = tbxName.Text.Trim();
            item.DisplayOrder = Convert.ToInt32(tbxDisplayOrder.Text.Trim());
            if (ddbLeader.Value != null)
            {
                item.LeaderID = Change.ToInt(ddbLeader.Value);
                item.LeaderName = DB.Users.Find(item.LeaderID).ChineseName;
            }
            item.Description = tbxRemark.Text.Trim();
            item.Enabled = cbxEnabled.Checked;

            if (ddbParent.Value != null)
            {
                int parentID = Convert.ToInt32(ddbParent.Value);
                item.ParentID = parentID;
            }
            else
            {
                item.ParentID = null;
            }

            if (!string.IsNullOrEmpty(DropDownProvince.SelectedValue))
            {
                item.ProvinceID = Change.ToInt(DropDownProvince.SelectedValue);
            }
            else
            {
                item.ProvinceID = null;
            }

            if (ddbControlType.Value != null)
            {
                item.DepartmentControlType = (DepartmentControlType)Enum.Parse(typeof(DepartmentControlType), ddbControlType.Value);
                item.DepartmentControlTypeName = EnumHelper.GetDescription(item.DepartmentControlType);
            }

            if (id == 0)
            {
                DB.Departments.Add(item);
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

            PageContext.RegisterStartupScript("F.notify({message:'添加成功！',messageIcon:'information',target:'_top',header:false,displayMilliseconds:3000,positionX:'center',positionY:'top'}); window.location.reload(false);" );
        }

        #endregion
    }
}
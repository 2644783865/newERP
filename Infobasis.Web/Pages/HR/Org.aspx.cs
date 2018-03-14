using FineUIPro;
using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EntityFramework.Extensions;
using Infobasis.Web.Data;

namespace Infobasis.Web.Pages.HR
{
    public partial class Org : PageBase
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

            btnNew.OnClientClick = Window1.GetShowReference("~/Pages/HR/Org_Form.aspx", "新增部门");

            BindGrid();

        }

        private void BindGrid()
        {
            List<Department> list = DB.Departments.OrderBy(d => d.DisplayOrder).ToList();
            foreach (Department dept in list)
            {
                if (dept.ProvinceID.HasValue)
                {
                    int id = dept.ProvinceID.Value;
                    Province province = DB.Provinces.Find(id);
                    dept.ProvinceName = province != null ? province.Name : "";
                }
            }
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int deptID = GetSelectedDataKeyID(Grid1);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查

                int userCount = DB.Users.Where(u => u.Department.ID == deptID).Count();
                if (userCount > 0)
                {
                    Alert.ShowInTop("删除失败！需要先清空属于此部门的员工！");
                    return;
                }

                int childCount = DB.Departments.Where(d => d.ParentDepartment.ID == deptID).Count();
                if (childCount > 0)
                {
                    Alert.ShowInTop("删除失败！请先删除子部门！");
                    return;
                }

                IInfobasisDataSource db = InfobasisDataSource.Create();
                if (db.ExecuteNonQuery("DELETE FROM SYtbDepartment WHERE ID = @ID AND CompanyID = @CompanyID", deptID, UserInfo.Current.CompanyID) == 0) 
                {
                    Alert.ShowInTop("删除失败！");
                }
                BindGrid();
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion
    }
}
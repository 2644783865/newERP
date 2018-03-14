using FineUIPro;
using Infobasis.Data.DataAccess;
using Infobasis.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.HR
{
    public partial class EE : PageBase
    {
        GenericRepository<Infobasis.Data.DataEntity.User> _repository = UnitOfWork.Repository<Infobasis.Data.DataEntity.User>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected string GetEditUrl(object id, object name)
        {
            JsObjectBuilder joBuilder = new JsObjectBuilder();
            joBuilder.AddProperty("id", "grid_newtab_employee_edit");
            joBuilder.AddProperty("title", "编辑 - " + name);
            joBuilder.AddProperty("iframeUrl", String.Format("getEditWindowUrl('{0}','{1}')", id, HttpUtility.UrlEncode(name.ToString())), true); // ResolveUrl(String.Format("~/grid/grid_newtab_window.aspx?id={0}&name={1}", id, HttpUtility.UrlEncode(name.ToString()))));
            joBuilder.AddProperty("refreshWhenExist", true);
            joBuilder.AddProperty("iconFont", "pencil");

            // addExampleTab函数定义在default.aspx，参数分别为：id, url, text, icon, refreshWhenExist
            return String.Format("parent.addTab({0});", joBuilder);
        }

        private void LoadData()
        {

            btnNew.OnClientClick = Window1.GetShowReference("~/Pages/HR/EE_Form.aspx", "新增员工");

            // 每页记录数
            Grid1.PageSize = UserInfo.Current.DefaultPageSize;
            ddlGridPageSize.SelectedValue = Grid1.PageSize.ToString();

            BindGrid();
        }

        private void BindGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.User> q = DB.Users.Include("Department").Include("ReportsToUser").Include("JobRole");

            // 在用户名称中搜索
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText) || u.ChineseName.Contains(searchText)
                    || u.EnglishName.Contains(searchText) || u.EmployeeSpellCode.Contains(searchText));
            }

            // 过滤启用状态
            if (rblEnableStatus.SelectedValue != "all")
            {
                q = q.Where(u => u.HireStatus == (rblEnableStatus.SelectedValue == "enabled" ? 0 : 1));
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.User>(q, Grid1);
            var qList = q.ToList();
            foreach (var ee in qList)
            {
                ee.ReportsToName = ee.ReportsToUser == null ? "" : ee.ReportsToUser.ChineseName;
                ee.DepartmentName = ee.Department == null ? "" : ee.Department.Name;
                ee.JobName = ee.JobRole == null ? "" : ee.JobRole.Name;
                ee.EmploymentTypeName = getEmploymentTypeName(ee.EmploymentType);
                ee.HireStatusName = getHireStatusName(ee.HireStatus);
            }

            Grid1.DataSource = qList;
            Grid1.DataBind();
        }

        #region Events

        private string getEmploymentTypeName(int? employmentType)
        {
            if (!employmentType.HasValue)
                return "全职";

            if (employmentType.Value == 1)
                return "全职";
            else if (employmentType.Value == 2)
                return "兼职";
            else if (employmentType.Value == 3)
                return "实习生";
            else if (employmentType.Value == 4)
                return "临时工";

            return "";
        }

        private string getHireStatusName(int? hireStatus)
        {
            if (!hireStatus.HasValue)
                return "在职";

            if (hireStatus.Value == 0)
                return "在职";
            else
                return "离职";
        }

        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查

        }

        // 超级管理员（admin）不可编辑，也不会检索出来
        protected void Grid1_PreRowDataBound(object sender, FineUIPro.GridPreRowEventArgs e)
        {
            Infobasis.Data.DataEntity.User user = e.DataItem as Infobasis.Data.DataEntity.User;

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

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            // 在操作之前进行权限检查

            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);

            foreach (int id in ids)
            {
                _repository.Delete(id, out msg, false);
            }
            if (!UnitOfWork.Save(out msg))
            {
                Alert.ShowInTop("删除失败！");
            }

            //DB.Users.Where(u => ids.Contains(u.ID)).Delete();

            // 重新绑定表格
            BindGrid();
        }

        protected void btnEnableUsers_Click(object sender, EventArgs e)
        {
            SetSelectedUsersEnableStatus(true);
        }

        protected void btnDisableUsers_Click(object sender, EventArgs e)
        {
            SetSelectedUsersEnableStatus(false);
        }


        private void SetSelectedUsersEnableStatus(bool enabled)
        {
            // 在操作之前进行权限检查

            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);

            // 执行数据库操作
            DB.Users.Where(u => ids.Contains(u.ID)).ToList().ForEach(u => u.Enabled = enabled);
            DB.SaveChanges();
            //DB.Users.Where(u => ids.Contains(u.ID)).Update(u => new Infobasis.Data.DataEntity.User { Enabled = enabled });

            // 重新绑定表格
            BindGrid();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int userID = GetSelectedDataKeyID(Grid1);
            Infobasis.Data.DataEntity.User user = DB.Users.Where(item => item.ID == userID).FirstOrDefault();
            string userName = user.Name;

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查

                if (user.IsClientAdmin)
                {
                    Alert.ShowInTop("不能删除默认的系统管理员（admin）！");
                }
                else
                {
                    //DB.Users.Where(u => u.ID == userID).Delete();
                    if (!_repository.Delete(user, out msg))
                    {
                        Alert.ShowInTop("删除失败！");
                    }

                    BindGrid();
                }
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void rblEnableStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }


        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }

        #endregion
    }
}
using FineUIPro;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using User = Infobasis.Data.DataEntity.User;

namespace Infobasis.Web.Pages.Admin
{
    public partial class UserRole : PageBase
    {
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            // 权限检查

            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid2, "确定要从当前角色移除选中的{0}项记录吗？");


            BindGrid1();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            // 每页记录数
            Grid2.PageSize = UserInfo.Current.DefaultPageSize;
            ddlGridPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();

            BindGrid2();
        }

        private void BindGrid1()
        {
            IQueryable<PermissionRole> q = DB.PermissionRoles;

            // 排列
            q = Sort<PermissionRole>(q, Grid1);
            List<PermissionRole> roleList = q.ToList();
            foreach (var item in roleList)
            {
                item.CountofUsers = DB.UserPermissionRoles.Where(up => up.PermissionRoleID == item.ID).Count();
            }

            Grid1.DataSource = roleList;
            Grid1.DataBind();
        }

        private void BindGrid2()
        {
            int roleID = GetSelectedDataKeyID(Grid1);

            if (roleID == -1)
            {
                Grid2.RecordCount = 0;

                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                IQueryable<Infobasis.Data.DataEntity.User> q = DB.Users;

                // 在用户名称中搜索
                string searchText = ttbSearchUser.Text.Trim();
                if (!String.IsNullOrEmpty(searchText))
                {
                    q = q.Where(u => u.Name.Contains(searchText));
                }

                // 过滤选中角色下的所有用户
                q = q.Where(u => u.UserPermissionRoles.Any(r => r.PermissionRoleID == roleID));

                // 在查询添加之后，排序和分页之前获取总记录数
                Grid2.RecordCount = q.Count();

                // 排列和分页
                q = SortAndPage<Infobasis.Data.DataEntity.User>(q, Grid2);

                Grid2.DataSource = q;
                Grid2.DataBind();
            }

        }

        #endregion

        #region Events

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid2.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid2();
        }


        #endregion

        #region Grid1 Events

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid1();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            BindGrid2();
        }

        protected void Grid1_RowSelect(object sender, FineUIPro.GridRowSelectEventArgs e)
        {
            BindGrid2();
        }

        #endregion

        #region Grid2 Events

        protected void ttbSearchUser_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchUser.ShowTrigger1 = true;
            BindGrid2();
        }

        protected void ttbSearchUser_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchUser.Text = String.Empty;
            ttbSearchUser.ShowTrigger1 = false;
            BindGrid2();
        }

        protected void Grid2_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
        }

        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            Grid2.SortDirection = e.SortDirection;
            Grid2.SortField = e.SortField;
            BindGrid2();
        }

        protected void Grid2_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid2.PageIndex = e.NewPageIndex;
            BindGrid2();
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            // 在操作之前进行权限检查

            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            int roleID = GetSelectedDataKeyID(Grid1);
            List<int> userIDs = GetSelectedDataKeyIDs(Grid2);

            PermissionRole role = DB.PermissionRoles.Where(r => r.ID == roleID)
                .FirstOrDefault();

            //role.Users.Where(u => userIDs.Contains(u.ID)).ToList().ForEach(u => role.Users.Remove(u));
            foreach (int userID in userIDs)
            {
                UserPermissionRole serPermissionRole = DB.UserPermissionRoles.Where(u => u.UserID == userID && u.PermissionRoleID == roleID).FirstOrDefault();
                if (serPermissionRole != null)
                {
                    DB.UserPermissionRoles.Remove(serPermissionRole);
                }
            }

            DB.SaveChanges();

            // 清空当前选中的项
            Grid2.SelectedRowIndexArray = null;

            // 重新绑定表格
            BindGrid2();
        }


        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            int roleID = GetSelectedDataKeyID(Grid1);
            object[] values = Grid2.DataKeys[e.RowIndex];
            int userID = Convert.ToInt32(values[0]);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查

                PermissionRole role = DB.PermissionRoles.Where(r => r.ID == roleID)
                    .FirstOrDefault();

                UserPermissionRole tobeRemoved = DB.UserPermissionRoles.Where(u => u.UserID == userID && u.PermissionRoleID == roleID).FirstOrDefault();

                if (role != null && tobeRemoved != null)
                {
                    DB.UserPermissionRoles.Remove(tobeRemoved);
                    DB.SaveChanges();
                }

                BindGrid2();

            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid2();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            int roleID = GetSelectedDataKeyID(Grid1);
            string addUrl = String.Format("~/Pages/Admin/UserRole_AddNew.aspx?id={0}", roleID);

            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加用户到当前角色"));
        }

        #endregion

    }
}
using FineUIPro;
using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml;
using EntityFramework.Extensions;
using AspNet = System.Web.UI.WebControls;
using Infobasis.Web.Data;

namespace Infobasis.Web.Pages.Admin
{
    public partial class User : PageBase
    {
        GenericRepository<Infobasis.Data.DataEntity.User> _repository = UnitOfWork.Repository<Infobasis.Data.DataEntity.User>();

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

            //ResolveDeleteMenuButtonForGrid(mbDeleteRows, Grid1);
            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid1);
            ResolveDeleteMenuButtonForGrid(mbEnableRows, Grid1, "确定要启用选中的{0}项记录吗？");
            ResolveDeleteMenuButtonForGrid(mbDisableRows, Grid1, "确定要禁用选中的{0}项记录吗？");


            btnNew.OnClientClick = Window1.GetShowReference("~/Pages/Admin/User_Form.aspx", "新增用户");

            // 每页记录数
            Grid1.PageSize = UserInfo.Current.DefaultPageSize;
            ddlGridPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();

            BindGrid();
        }


        private void BindGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.User> q = DB.Users; //.Include(u => u.Dept);

            // 在用户名称中搜索
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText) || u.ChineseName.Contains(searchText) || u.EnglishName.Contains(searchText));
            }

            // 过滤启用状态
            if (rblEnableStatus.SelectedValue != "all")
            {
                q = q.Where(u => u.Enabled == (rblEnableStatus.SelectedValue == "enabled" ? true : false));
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.User>(q, Grid1);

            Grid1.DataSource = q;
            Grid1.DataBind();
        }

        #endregion

        #region Events

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

            FineUIPro.LinkButtonField deleteField = Grid1.FindColumn("deleteField") as FineUIPro.LinkButtonField;

            // 不能删除超级管理员
            if (user.IsClientAdmin)
            {
                deleteField.Enabled = false;
                deleteField.ToolTip = "不能删除管理员！";
            }
            else
            {
                deleteField.Enabled = true;
                deleteField.ToolTip = "删除";
            }

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
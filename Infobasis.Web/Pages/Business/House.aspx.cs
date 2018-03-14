using FineUIPro;
using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Business
{
    public partial class House : PageBase
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
            // 权限检查
            btnNew.OnClientClick = Window1.GetShowReference("~/Pages/Business/House_Form.aspx", "新增楼盘");

            // 每页记录数
            Grid1.PageSize = UserInfo.Current.DefaultPageSize;


            BindGrid();
        }

        private void BindGrid()
        {
            IQueryable<HouseInfo> q = DB.HouseInfos.OrderBy(p => p.CompletionDate.Value);//.Where(item => item.CompanyID == UserInfo.Current.CompanyID);

            // 在名称中搜索
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(r => r.Name.Contains(searchText));
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<HouseInfo>(q, Grid1);

            Grid1.DataSource = q;
            Grid1.DataBind();
        }


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

        protected void Grid1_PreRowDataBound(object sender, GridPreRowEventArgs e)
        {
            Infobasis.Data.DataEntity.HouseInfo houseInfo = e.DataItem as Infobasis.Data.DataEntity.HouseInfo;
            LinkButtonField deleteField = Grid1.FindColumn("deleteField") as LinkButtonField;
            // ForbidDelete

            deleteField.Enabled = true;
            deleteField.ToolTip = "删除";
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            //CheckPowerWithWindowField("CoreRoleEdit", Grid1, "editField");
            //CheckPowerWithLinkButtonField("CoreRoleDelete", Grid1, "deleteField");

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

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int houseID = GetSelectedDataKeyID(Grid1);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查

                // 执行数据库操作
                //DB.PermissionRoles.Where(item => item.ID == roleID).Delete<PermissionRole>();
                HouseInfo houseInfo = DB.HouseInfos.Where(item => item.ID == houseID).FirstOrDefault();
                GenericRepository<HouseInfo> repository = UnitOfWork.Repository<HouseInfo>();
                if (!repository.Delete(houseInfo, out msg))
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
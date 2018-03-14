using FineUIPro;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using EntityFramework.Extensions;
using AspNet = System.Web.UI.WebControls;
using System.Data;
using Infobasis.Web.Util;
using Infobasis.Data.DataAccess;

namespace Infobasis.Web.Pages.Admin
{
    public partial class Client : PageBase
    {
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                string shengScript = "window._SHENG=[[\"001\",\"北京\"],[\"002\",\"河南\"],[\"003\",\"河北\"],[\"004\",\"湖南\"],[\"005\",\"湖北\"],[\"006\",\"广西\"],[\"007\",\"安徽\"]];";
                string shiScript = "window._SHI={\"001\":[[\"001001\",\"北京市\"]],\"002\":[[\"002001\",\"郑州市\"],[\"002002\",\"开封市\"],[\"002003\",\"洛阳市\"],[\"002004\",\"平顶山市\"],[\"002005\",\"安阳市\"],[\"002006\",\"鹤壁市\"],[\"002007\",\"新乡市\"],[\"002008\",\"焦作市\"],[\"002009\",\"濮阳市\"],[\"002010\",\"许昌市\"],[\"002011\",\"漯河市\"],[\"002012\",\"三门峡市\"],[\"002013\",\"南阳市\"],[\"002014\",\"商丘市\"],[\"002015\",\"信阳市\"],[\"002016\",\"周口市\"],[\"002017\",\"驻马店市\"],[\"002018\",\"济源市\"]],\"003\":[[\"003001\",\"石家庄市\"],[\"003002\",\"唐山市\"],[\"003003\",\"秦皇岛市\"],[\"003004\",\"邯郸市\"],[\"003005\",\"邢台市\"],[\"003006\",\"保定市\"],[\"003007\",\"张家口市\"],[\"003008\",\"承德市\"],[\"003009\",\"沧州市\"],[\"003010\",\"廊坊市\"],[\"003011\",\"衡水市\"]],\"004\":[[\"004001\",\"长沙市\"],[\"004002\",\"株洲市\"],[\"004003\",\"湘潭市\"],[\"004004\",\"衡阳市\"],[\"004005\",\"邵阳市\"],[\"004006\",\"岳阳市\"],[\"004007\",\"常德市\"],[\"004008\",\"张家界市\"],[\"004009\",\"益阳市\"],[\"004010\",\"郴州市\"],[\"004011\",\"永州市\"],[\"004012\",\"怀化市\"],[\"004013\",\"娄底市\"],[\"004014\",\"湘西土家族苗族自治州\"]],\"005\":[[\"005001\",\"武汉市\"],[\"005002\",\"黄石市\"],[\"005003\",\"十堰市\"],[\"005004\",\"荆州市\"],[\"005005\",\"宜昌市\"],[\"005006\",\"襄樊市\"],[\"005007\",\"鄂州市\"],[\"005008\",\"荆门市\"],[\"005009\",\"孝感市\"],[\"005010\",\"黄冈市\"],[\"005011\",\"咸宁市\"],[\"005012\",\"随州市\"],[\"005013\",\"恩施土家族苗族自治州\"],[\"005014\",\"仙桃市\"],[\"005015\",\"天门市\"],[\"005016\",\"潜江市\"],[\"005017\",\"神农架林区\"]],\"006\":[[\"006001\",\"南宁市\"],[\"006002\",\"柳州市\"],[\"006003\",\"桂林市\"],[\"006004\",\"梧州市\"],[\"006005\",\"北海市\"],[\"006006\",\"防城港市\"],[\"006007\",\"钦州市\"],[\"006008\",\"贵港市\"],[\"006009\",\"玉林市\"],[\"006010\",\"百色市\"],[\"006011\",\"贺州市\"],[\"006012\",\"河池市\"],[\"006013\",\"来宾市\"],[\"006014\",\"崇左市\"]],\"007\":[[\"007001\",\"合肥市\"],[\"007002\",\"芜湖市\"],[\"007003\",\"蚌埠市\"],[\"007004\",\"淮南市\"],[\"007005\",\"马鞍山市\"],[\"007006\",\"淮北市\"],[\"007007\",\"铜陵市\"],[\"007008\",\"安庆市\"],[\"007009\",\"黄山市\"],[\"007010\",\"滁州市\"],[\"007011\",\"阜阳市\"],[\"007012\",\"宿州市\"],[\"007013\",\"巢湖市\"],[\"007014\",\"六安市\"],[\"007015\",\"亳州市\"],[\"007016\",\"池州市\"],[\"007017\",\"宣城市\"]]};";

                PageContext.RegisterPreStartupScript("Client_shengshiscript", shengScript + shiScript, true);
            }
        }

        private void LoadData()
        {
            //TODO backend
            IInfobasisDataSource db = InfobasisDataSource.Create();
            db.ExecuteNonQuery("UPDATE SYtbCompany SET CompanyStatus = @CompanyStatus WHERE ExpiredDatetime IS NOT NULL AND DATEDIFF(dd, GETDATE(), ExpiredDatetime) <= 0", CompanyStatus.Expired);
            
            // 权限检查

            //ResolveDeleteMenuButtonForGrid(mbDeleteRows, Grid1);
            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid1);
            ResolveDeleteMenuButtonForGrid(mbEnableRows, Grid1, "确定要启用选中的{0}项记录吗？");
            ResolveDeleteMenuButtonForGrid(mbDisableRows, Grid1, "确定要禁用选中的{0}项记录吗？");


            btnNew.OnClientClick = Window1.GetShowReference("~/Pages/Admin/Client_Form.aspx", "新增客户");

            // 每页记录数
            Grid1.PageSize = UserInfo.Current.DefaultPageSize; 
            ddlGridPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();

            BindGrid();
        }


        private void BindGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.Company> q = DB.Companys; //.Include(u => u.Dept);

            // 在客户名称中搜索
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText) || u.CompanyCode.Contains(searchText) || u.ClientAdminAccount.Contains(searchText));
            }

            // 过滤启用状态
            CompanyStatus companyStatus;
            if (rblEnableStatus.SelectedValue != "all")
            {
                if (rblEnableStatus.SelectedValue == "enabled")
                    companyStatus = CompanyStatus.Enabled;
                else if (rblEnableStatus.SelectedValue == "disabled")
                    companyStatus = CompanyStatus.Disabled;
                else
                    companyStatus = CompanyStatus.Expired;
                q = q.Where(u => u.CompanyStatus == companyStatus);
            }

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = q.Count();

            // 排列和数据库分页
            q = SortAndPage<Infobasis.Data.DataEntity.Company>(q, Grid1);

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
            Infobasis.Data.DataEntity.Company company = e.DataItem as Infobasis.Data.DataEntity.Company;

            FineUIPro.LinkButtonField deleteField = Grid1.FindColumn("deleteField") as FineUIPro.LinkButtonField;

            // 不能删除超级管理员
            if (company.IsSystemAdminCompany.HasValue && company.IsSystemAdminCompany.Value)
            {
                deleteField.Enabled = false;
                deleteField.ToolTip = "不能删除此数据！";
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

            DB.Companys.Where(u => ids.Contains(u.ID)).Delete();

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
            DB.Companys.Where(u => ids.Contains(u.ID)).Update(u => new Infobasis.Data.DataEntity.Company { CompanyStatus = enabled ? CompanyStatus.Enabled : CompanyStatus.Disabled });

            // 重新绑定表格
            BindGrid();
        }

        protected void Grid1_RowDataBound(object sender, FineUIPro.GridRowEventArgs e)
        {
            // e.DataItem  -> System.Data.DataRowView or custom class.
            // e.RowIndex -> Current row index.
            // e.Values -> Rendered html for each column of this row.

            Infobasis.Data.DataEntity.Company company = e.DataItem as Infobasis.Data.DataEntity.Company;

            CompanyStatus companyStatus = (CompanyStatus)company.CompanyStatus;
            RenderField rfCompnyStatus = Grid1.FindColumn("CompanyStatus") as RenderField;
            if (companyStatus == CompanyStatus.Expired)
            {
                //e.RowCssClass = "Expired-Company";
                e.CellAttributes[rfCompnyStatus.ColumnIndex]["data-color"] = "Expired-Company";
            }
            else if (companyStatus == CompanyStatus.Disabled)
            {
                //e.RowCssClass = "Disabled-Company";
                e.CellAttributes[rfCompnyStatus.ColumnIndex]["data-color"] = "Disabled-Company";
            }

        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int companyID = GetSelectedDataKeyID(Grid1);
            Company company = DB.Companys.Find(companyID);

            string companyName = company.Name;

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查

                if (company.CompanyCode == "system")
                {
                    Alert.ShowInTop("不能删除默认的系统管理员（admin）！");
                }
                else
                {
                    DB.Companys.Where(u => u.ID == companyID).Delete();

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
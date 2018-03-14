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
using Infobasis.Web.Util;

namespace Infobasis.Web.Pages.Admin
{
    public partial class ModuleRole : PageBase
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
            //CheckPowerWithButton("CoreRolePowerEdit", btnGroupUpdate);


            // 每页记录数
            Grid1.PageSize = UserInfo.Current.DefaultPageSize;
            BindGrid();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            // 每页记录数
            //Grid2.PageSize = 30;// ConfigHelper.PageSize;
            BindTree();
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            BindTree();
        }

        protected void Grid1_RowSelect(object sender, FineUIPro.GridRowSelectEventArgs e)
        {
            BindTree();
        }

        private void BindGrid()
        {
            IQueryable<PermissionRole> q = DB.PermissionRoles.Where(item => item.IsActive == true);

            // 排列
            q = Sort<PermissionRole>(q, Grid1);

            Grid1.DataSource = q;
            Grid1.DataBind();
        }

        private void BindTree()
        {
            int roleId = GetSelectedDataKeyID(Grid1);
            IInfobasisDataSource db = InfobasisDataSource.Create();
            XmlDocument xmlDoc = db.ExecuteXmlDoc("Tree", "EXEC usp_SY_GetModuleTreeSetupXML @CompanyID, @UserID, @PermissionRoleID",
                UserInfo.Current.CompanyID, UserInfo.Current.ID, roleId);

            XmlNodeList xmlNodes = xmlDoc.SelectNodes("/Tree/TreeNode");
            TreeModule.DataSource = xmlDoc;
            TreeModule.DataBind();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // 在操作之前进行权限检查

            int roleId = GetSelectedDataKeyID(Grid1);
            if (roleId == -1)
            {
                return;
            }

            FineUIPro.TreeNode[] nodes = TreeModule.GetCheckedNodes();
            List<int> newPowerIDs = new List<int>();
            if (nodes.Length > 0)
            {
                foreach (FineUIPro.TreeNode node in nodes)
                {
                    if (node.Checked)
                        newPowerIDs.Add(Change.ToInt(node.NodeID));
                }
            }
            else
            {
                Alert.ShowInTop("没有选择数据！");
                return;
            }

            // 当前角色新的权限列表
            PermissionRole role = DB.PermissionRoles.Include("ModulePermissionRoles").Where(r => r.ID == roleId).FirstOrDefault();
            int[] newEntityIDs = newPowerIDs.ToArray();
            ICollection<ModulePermissionRole> existEntities = role.ModulePermissionRoles;
            int[] tobeAdded = newEntityIDs.Except(existEntities.Select(x => x.ModuleID)).ToArray();
            int[] tobeRemoved = existEntities.Select(x => x.ModuleID).Except(newEntityIDs).ToArray();
            GenericRepository<ModulePermissionRole> moduleRoleRepository = UnitOfWork.Repository<ModulePermissionRole>();
            int companyID = UserInfo.Current.CompanyID;

            foreach (int id in tobeAdded)
            {
                ModulePermissionRole newEntity = new ModulePermissionRole()
                {
                    ModuleID = id,
                    CompanyID = companyID,
                    PermissionRoleID = roleId,
                    CreateDatetime = DateTime.Now
                };
                moduleRoleRepository.Insert(newEntity, out msg, false);
                //role.ModulePermissionRoles.Add(newEntity);
            }

            foreach (int id in tobeRemoved)
            {

                //role.ModulePermissionRoles.Remove(existEntities.Single(r => r.ModuleID == id && r.PermissionRoleID == roleId));
                moduleRoleRepository.Delete(existEntities.Single(r => r.ModuleID == id && r.PermissionRoleID == roleId), out msg, false);
            }

            if (!UnitOfWork.Save(out msg))
            {
                Alert.ShowInTop("当前角色的权限更新成功！", MessageBoxIcon.Error);
                //DB.SaveChanges();
            }
            else
                Alert.ShowInTop("当前角色的权限更新成功！");
        }

        #endregion

    }
}
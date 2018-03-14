using FineUIPro;
using Infobasis.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Admin
{
    public partial class Module : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                btnAdd.OnClientClick = Window1.GetShowReference("~/Pages/Admin/Module_Add.aspx", "弹出窗口二");
                btnEdit.OnClientClick = Window1.GetShowReference("~/Pages/Admin/Module_Add.aspx", "弹出窗口二");
            }
        }

        #region BindGrid

        private void BindGrid()
        {
            IInfobasisDataSource db = InfobasisDataSource.Create();
            DataTable table = db.ExecuteTable("SELECT * FROM SYtbModule ORDER BY DisplayOrder");
            ModuleGrid.DataSource = table;
            ModuleGrid.DataBind();
        }

        #endregion

        protected void btnAdd_Click(object sender, EventArgs e)
        { 
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {

        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            Alert alert = new Alert();
            String ID = ModuleGrid.SelectedRow.DataKeys.First().ToString();
            alert.Message = ID;
            alert.Show();
            
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            Alert alert = new Alert();
            alert.Show();
        }
    }
}
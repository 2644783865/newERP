using FineUIPro;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Admin
{
    public partial class SystemData_Form : PageBase
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

            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            int id = GetQueryIntValue("id");
            int pid = GetQueryIntValue("pid");
            if (pid > 0)
            {
                Infobasis.Data.DataEntity.EntityList entityList = DB.EntityLists.Find(pid);
                if (entityList == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }
            }

            if (id > 0)
            {
                Infobasis.Data.DataEntity.EntityListValue entityListValue = DB.EntityListValues.Find(id);
                if (pid > 0)
                {
                    tbxName.Text = entityListValue.Name;
                    tbxCode.Text = entityListValue.Code;
                    tbxIsActive.Checked = entityListValue.IsActive;
                    tbxDisplayOrder.Text = entityListValue.DisplayOrder.ToString();
                }
                else
                {
                    Infobasis.Data.DataEntity.EntityList entityList = DB.EntityLists.Find(pid);
                    tbxName.Text = entityList.Name;
                    tbxCode.Text = entityList.Code;
                    tbxIsActive.Checked = entityList.IsActive;
                    tbxDisplayOrder.Text = entityList.DisplayOrder.ToString();
                }
            }
            else
            {
                tbxCode.Text = tbxCode.Text = GenerateNum("Item-", false);
            }

        }

        #region Events

        private void SaveItem()
        {
            int id = GetQueryIntValue("id");
            int pid = GetQueryIntValue("pid");
            string type = GetQueryValue("type");
            if (id > 0 && pid > 0)
            {
                Infobasis.Data.DataEntity.EntityListValue entityListValue = DB.EntityListValues.Find(id);
                entityListValue.Name = tbxName.Text.Trim();
                entityListValue.Code = tbxCode.Text.Trim();
                entityListValue.IsActive = tbxIsActive.Checked;
                entityListValue.DisplayOrder = Change.ToInt(tbxDisplayOrder.Text);
                entityListValue.CreateDatetime = DateTime.Now;
            }
            else if (pid > 0)
            {
                Infobasis.Data.DataEntity.EntityListValue item = new Infobasis.Data.DataEntity.EntityListValue();
                item.EntityListID = pid;
                item.Name = tbxName.Text.Trim();
                item.Code = tbxCode.Text.Trim();
                item.IsActive = tbxIsActive.Checked;
                item.DisplayOrder = Change.ToInt(tbxDisplayOrder.Text);
                item.LastUpdateDatetime = DateTime.Now;

                DB.EntityListValues.Add(item);
            }
            else
            {
                Infobasis.Data.DataEntity.EntityList item = new Infobasis.Data.DataEntity.EntityList();
                item.Name = tbxName.Text.Trim();
                item.Code = tbxCode.Text.Trim();
                item.IsActive = tbxIsActive.Checked;
                item.LastUpdateDatetime = DateTime.Now;
                item.DisplayOrder = Change.ToInt(tbxDisplayOrder.Text);
                item.GroupCode = type;
                DB.EntityLists.Add(item);
            }
            DB.SaveChanges();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();

            //Alert.Show("添加成功！", String.Empty, ActiveWindow.GetHidePostBackReference());
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            ShowNotify("保存成功！");
            int scrollid = GetQueryIntValue("scrollid");
            PageContext.RegisterStartupScript(string.Format("closeAndRefreshTopWindow('scrollid={0}');", scrollid));
        }
        #endregion
    }
}
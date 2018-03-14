using FineUIPro;
using Infobasis.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.CRM
{
    public partial class SmsTemplateForm : PageBase
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
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            int id = GetQueryIntValue("id");
            if (id > 0)
            {
                Infobasis.Data.DataEntity.SMSTemplate data = DB.SMSTemplates.Find(id);
                if (data != null)
                {
                    tbxName.Text = data.Name;
                    tbxContent.Text = data.Content;
                    cbxEnabled.Checked = data.IsActive;
                }
            }

        }

        #endregion

        #region Events


        private void SaveItem()
        {
            int companyID = UserInfo.Current.CompanyID;
                        int id = GetQueryIntValue("id");
            Infobasis.Data.DataEntity.SMSTemplate item = null;
            if (id == 0)
            {
                item = new Infobasis.Data.DataEntity.SMSTemplate();
                item.CreateByID = UserInfo.Current.ID;
                item.CreateByName = UserInfo.Current.ChineseName;
                item.CreateDatetime = DateTime.Now;
            }
            else
            {
                item.LastUpdateByID = UserInfo.Current.ID;
                item.LastUpdateByName = UserInfo.Current.ChineseName;
                item.LastUpdateDatetime = DateTime.Now;
            }
            item.Name = tbxName.Text.Trim();
            item.Content = tbxContent.Text.Trim();
            item.IsActive = cbxEnabled.Checked;
            item.CompanyID = companyID;
            item.TemplateType = DropDownTemplateType.SelectedValue;
            item.TemplateTypeName = DropDownTemplateType.SelectedText;

            if (id == 0)
            {
                DB.SMSTemplates.Add(item);
            }

            DB.SaveChanges();

        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            string inputUserName = tbxName.Text.Trim();

            SaveItem();

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion
    }
}
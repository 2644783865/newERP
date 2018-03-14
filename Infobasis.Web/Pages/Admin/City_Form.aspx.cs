using FineUIPro;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Admin
{
    public partial class City_Form : PageBase
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
                Province province = DB.Provinces.Find(pid);
                if (province == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }
            }
            
            if (id > 0)
            {
                Infobasis.Data.DataEntity.City city = DB.Citys.Find(id);
                if (pid > 0)
                {
                    tbxName.Text = city.Name;
                    tbxCode.Text = city.Code;
                    tbxIsActive.Checked = city.IsActive;
                    tbxDisplayOrder.Text = city.DisplayOrder.ToString();
                }
                else
                {
                    Province province = DB.Provinces.Find(pid);
                    tbxName.Text = province.Name;
                    tbxCode.Text = province.Code;
                    tbxIsActive.Checked = province.IsActive;
                    tbxDisplayOrder.Text = province.DisplayOrder.ToString();
                }
            }

        }

        #region Events

        private void SaveItem()
        {
            int id = GetQueryIntValue("id");
            int pid = GetQueryIntValue("pid");
            if (id > 0 && pid > 0)
            {
                Infobasis.Data.DataEntity.City city = DB.Citys.Find(id);
                city.Name = tbxName.Text.Trim();
                city.Code = ChinesePinyin.GetFirstPinyin(city.Name);
                city.IsActive = tbxIsActive.Checked;
                city.DisplayOrder = Change.ToInt(tbxDisplayOrder.Text);
                city.CreateDatetime = DateTime.Now;
            }
            else if (pid > 0)
            {
                Infobasis.Data.DataEntity.City item = new Infobasis.Data.DataEntity.City();
                item.ProvinceID = pid;
                item.Name = tbxName.Text.Trim();
                item.Code = ChinesePinyin.GetFirstPinyin(item.Name);
                item.IsActive = tbxIsActive.Checked;
                item.DisplayOrder = Change.ToInt(tbxDisplayOrder.Text);
                item.LastUpdateDatetime = DateTime.Now;

                DB.Citys.Add(item);
            }
            else
            {
                Province item = new Province();
                item.Name = tbxName.Text.Trim();
                item.Code = ChinesePinyin.GetFirstPinyin(item.Name);
                item.IsActive = tbxIsActive.Checked;
                item.LastUpdateDatetime = DateTime.Now;
                item.DisplayOrder = Change.ToInt(tbxDisplayOrder.Text);
                DB.Provinces.Add(item);
            }
            DB.SaveChanges();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();

            //Alert.Show("添加成功！", String.Empty, ActiveWindow.GetHidePostBackReference());
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            ShowNotify("保存成功！");
            PageContext.RegisterStartupScript("closeAndRefreshTopWindow();");
        }
        #endregion
    }
}
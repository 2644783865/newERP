using FineUIPro;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.OA
{
    public partial class Announcement_Form : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbxPublishDate.Text = DateTime.Now.ToString();
                tbxPublisherDisplayName.Text = UserInfo.Current.ChineseName;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxTitle.Text))
            {
                ShowNotify("请输入公告主题");
                return;
            }
            int announceID = Change.ToInt(tbxAnnounceID.Text);
            Infobasis.Data.DataEntity.Announcement announcement = null;
            if (announceID > 0)
            {
                announcement = DB.Announcements.Find(announceID);
            }
            else
            {
                announcement = new Infobasis.Data.DataEntity.Announcement();
                announcement.Code = GenerateNum("Ann");
            }

            announcement.Title = tbxTitle.Text;
            announcement.PublisherID = UserInfo.Current.ID;
            announcement.PublishDate = Change.ToDateTime(tbxPublishDate.Text);
            if (Change.ToDateTime(tbxEndDate.Text) != DateTime.MinValue)
                announcement.EndDate = Change.ToDateTime(tbxEndDate.Text);
            announcement.Publisher = UserInfo.Current.ChineseName;
            announcement.Note = tbxContentHtml.Text;
            announcement.CreateByID = UserInfo.Current.ID;
            announcement.CreateByName = UserInfo.Current.ChineseName;
            announcement.AnnounceTypeID = DropDownAnnounceType.SelectedValue;
            announcement.AnnounceTypeName = DropDownAnnounceType.SelectedText;

            if (announceID == 0)
            {
                DB.Announcements.Add(announcement);
            }

            if (SaveChanges())
            {
                ShowNotify("保存成功");
                tbxAnnounceID.Text = Change.ToString(announcement.ID);
            }
            else
                ShowNotify("保存失败");

        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            this.btnSave_Click(sender, e);
            PageContext.RegisterStartupScript("closeAndRefreshTopWindow();");
        }
    }
}
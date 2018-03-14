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
    public partial class Meeting_Form : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbxMeetingDate.Text = DateTime.Now.ToString();
                tbxHostUserDisplayName.Text = UserInfo.Current.ChineseName;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxTopic.Text))
            {
                ShowNotify("请输入会议主题");
                return;
            }
            int meetingID = Change.ToInt(tbxMeetingID.Text);
            Infobasis.Data.DataEntity.Meeting meeting = null;
            if (meetingID > 0)
            {
                meeting = DB.Meetings.Find(meetingID);
            }
            else
            {
                meeting = new Infobasis.Data.DataEntity.Meeting();
                meeting.Code = GenerateNum("M");
            }

            meeting.AttendanceNames = tbxAttendanceIDs.Text;
            meeting.Topic = tbxTopic.Text;
            meeting.HostUserID = UserInfo.Current.ID;
            meeting.HostUserDisplayName = UserInfo.Current.ChineseName;
            meeting.Content = tbxContentHtml.Text;
            meeting.CreateByID = UserInfo.Current.ID;
            meeting.CreateByName = UserInfo.Current.ChineseName;
            meeting.MeetingTypeName = DropDownMeetingType.SelectedText;

            if (meetingID == 0)
            {
                DB.Meetings.Add(meeting);
            }
            
            if (SaveChanges())
            {
                ShowNotify("保存成功");
                tbxMeetingID.Text = Change.ToString(meeting.ID);
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
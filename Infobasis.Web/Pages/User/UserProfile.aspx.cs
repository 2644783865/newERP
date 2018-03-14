using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using AspNetUI = System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.User
{
    public partial class UserProfile : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void userPortraitUpload_FileSelected(object sender, EventArgs e)
        {
            if (userPortraitUpload.HasFile)
            {
                int companyID = UserInfo.Current.CompanyID;
                int userID = UserInfo.Current.ID;

                string fileOriginalName = userPortraitUpload.ShortFileName;

                if (!ValidateFileType(fileOriginalName))
                {
                    ShowNotify("无效的文件类型！");
                    return;
                }

                string uploadPath = Global.UploadFolderPath;
                if (uploadPath.StartsWith("~") || uploadPath.StartsWith(".")) //相对路径
                    uploadPath = HttpContext.Current.Server.MapPath(uploadPath + "/images/" + companyID.ToString());
                else
                    uploadPath = uploadPath + "/images/" + companyID;

                string originalFolderPath = Path.Combine(uploadPath, DateTime.Now.ToString("yyyyMM") + "/original");
                string thumbnailFolderPath = Path.Combine(uploadPath, DateTime.Now.ToString("yyyyMM") + "/thumbnail");

                bool folderExists = Directory.Exists(originalFolderPath);
                if (!folderExists)
                    Directory.CreateDirectory(originalFolderPath);

                folderExists = Directory.Exists(thumbnailFolderPath);
                if (!folderExists)
                    Directory.CreateDirectory(thumbnailFolderPath);

                string fileType = fileOriginalName.Substring(fileOriginalName.LastIndexOf("."));
                string fileName = DateTime.Now.Ticks.ToString();
                string fileOriginalSavePath = Path.Combine(originalFolderPath, fileName + fileType);

                userPortraitUpload.SaveAs(fileOriginalSavePath);

                Image originalImage = StreamHelper.ImagePath2Img(fileOriginalSavePath);
                string fileThumbnailSavePath = Path.Combine(thumbnailFolderPath, fileName + fileType);
                Image newImage = ImageHelper.GetThumbNailImage(originalImage, 160, 160);
                newImage.Save(fileThumbnailSavePath);

                string savedPath = Global.UploadFolderVirualPath + "/images/" + companyID.ToString() + "/" + DateTime.Now.ToString("yyyyMM") + "/thumbnail/" + fileName + fileType;
                Infobasis.Data.DataEntity.User user = DB.Users.Find(userID);
                user.UserPortraitPath = savedPath;
                DB.SaveChanges();

                userPortrait.ImageUrl = savedPath;

                // 清空文件上传组件（上传后要记着清空，否则点击提交表单时会再次上传！！）
                userPortraitUpload.Reset();
            }

        }

        private void LoadData()
        { 
            int userID = UserInfo.Current.ID;
            Infobasis.Data.DataEntity.User user = DB.Users.Find(userID);
            tbxName.Text = user.ChineseName;
            tbxUserName.Text = user.Name;
            if (string.IsNullOrEmpty(user.UserPortraitPath))
                userPortrait.ImageUrl = Global.Default_User_Portrait_Path;
            else
            {
                userPortrait.ImageUrl = user.UserPortraitPath;
                userPortraitUpload.ButtonText = "修改头像";
            }
        }

        protected void imageUpload_FileSelected(object sender, EventArgs e)
        {

        }
    }
}
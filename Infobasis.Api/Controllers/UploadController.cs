using Infobasis.Api.Data;
using Infobasis.Api.Utils;
using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Infobasis.Api.Controllers
{
    public class UploadController : BaseApiController
    {
        [HttpPost]
        [Route("~/api/upload/{uploadFolderCode}")]
        public IHttpActionResult UploadFile(string uploadFolderCode)
        {
            int companyID = UserInfo.GetCurrentCompanyID();
            UploadMessage returnMsg = _uploadFile(companyID.ToString(), "oa");
            if (returnMsg.Status > 0)
                return BadRequest(returnMsg.ErrorMsg);
            else
            {
                CloudFolder cloudFolder = DB.CloudFolders.Where(item => item.Code == uploadFolderCode && item.CompanyID == companyID).FirstOrDefault();
                if (cloudFolder != null)
                {
                    CloudFile cf = new CloudFile();
                    cf.CloudFolderID = cloudFolder.ID;
                    cf.CompanyID = companyID;
                    cf.EntityCode = "Cloud";
                    cf.OriginName = returnMsg.OriginName;
                    cf.FileName = returnMsg.FileName;
                    cf.FileTypeName = returnMsg.FileType;
                    cf.FileSize = returnMsg.FileSize;
                    cf.FilePath = returnMsg.UploadFilePath;
                    cf.DiskUploadPath = returnMsg.DiskUploadPath;
                    cf.CreateDatetime = DateTime.Now;
                    cf.CreateByID = UserInfo.GetCurrentUserID();
                    cf.IsValidImage = returnMsg.IsImage;
                    cf.HasThumbnail = returnMsg.HasThumbnail;
                    cf.OriginDiskUploadPath = returnMsg.OriginDiskUploadPath;
                    cf.OriginFilePath = returnMsg.OriginFilePath;

                    DB.CloudFiles.Add(cf);
                    DB.SaveChanges();
                }
                return Ok(returnMsg);
            }
        }

        //上传头像
        [HttpPost]
        [Route("~/api/Upload/MyAvatar")]
        public IHttpActionResult UploadMyAvatar(string companyID)
        {
            int currentID = UserInfo.GetCurrentUserID();
            GenericRepository<User> _repository = unitOfWork.Repository<User>();

            UploadMessage returnMsg = _uploadImage(companyID, true);
            if (returnMsg.Status > 0)
            {
                return BadRequest(returnMsg.ErrorMsg);
            }
            else
            {
                User userEntity = _repository.GetByID(currentID);
                userEntity.UserPortraitPath = returnMsg.UploadFilePath;
                _repository.PartialUpdate(userEntity, out msg, ee => ee.UserPortraitPath);
                unitOfWork.Save(out msg);
                return Ok(returnMsg);
            }

        }

        //上传头像
        [HttpPost]
        [Route("~/api/Upload/Avatar")]
        public IHttpActionResult UploadAvatar(string companyID)
        {
            UploadMessage returnMsg = _uploadImage(companyID, true);
            if (returnMsg.Status > 0)
                return BadRequest(returnMsg.ErrorMsg);
            else
                return Ok(returnMsg);
        }

        //上传CompanyLogo
        [HttpPost]
        [Route("~/api/Upload/CompanyLogo")]
        public IHttpActionResult UploadCompanyLogo(string companyID)
        {
            UploadMessage returnMsg = _uploadImage(companyID, true);
            if (returnMsg.Status > 0)
                return BadRequest(returnMsg.ErrorMsg);
            else
                return Ok(returnMsg);
        }

        private UploadMessage successfulMsg(string originName, string fileName, int fileSize, string fileType, string diskUploadPath, string uploadFilePath)
        {
            return new UploadMessage() { Status = 0, ErrorMsg = "", OriginName = originName, FileName = fileName, FileSize = fileSize, FileType = fileType,
                DiskUploadPath = diskUploadPath,
                UploadFilePath = uploadFilePath };
        }

        private UploadMessage errorMsg(string msg)
        {
            return new UploadMessage() { Status = 1, ErrorMsg = msg, UploadFilePath = null };
        }

        private UploadMessage _uploadFile(string companyID, string fileFolderName)
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                string fileSubFolder = "/files/" + fileFolderName + "/";
                FileExtension[] allowFileExtension = { FileExtension.BMP, FileExtension.DOC, 
                                                         FileExtension.DOCX, FileExtension.XLS, FileExtension.XLSX,
                                                         FileExtension.PDF,
                                                     FileExtension.GIF, FileExtension.JPG, FileExtension.PNG};

                if (!FileValidation.IsAllowedExtension(HttpContext.Current.Request.Files["file"], allowFileExtension))
                    return errorMsg("不接受的文件格式");

                bool isImage = FileValidation.IsImageExtension(HttpContext.Current.Request.Files["file"]);

                // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
                string folderPath = WebApiApplication.UploadFolderPath;

                if (folderPath.StartsWith("~") || folderPath.StartsWith(".")) //相对路径
                    folderPath = HttpContext.Current.Server.MapPath(folderPath + "/files/" + companyID);
                else
                    folderPath = folderPath + fileSubFolder + companyID;

                string yearMonth = DateTime.Now.ToString("yyyyMM");
                folderPath = Path.Combine(folderPath, yearMonth);

                //folderPath = folderPath.Replace(".WebAPI", ".Web").Replace("UploadedDocuments", "content/images");

                bool folderExists = Directory.Exists(folderPath);
                if (!folderExists)
                    Directory.CreateDirectory(folderPath);

                HttpPostedFile httpPostedFile = HttpContext.Current.Request.Files["file"];
                string fileType = httpPostedFile.FileName.Substring(httpPostedFile.FileName.LastIndexOf("."));
                string fileName = Guid.NewGuid() + "-" + DateTime.Now.ToString("ddHHmmssff");
                string fileSavePath = Path.Combine(folderPath, fileName + fileType);
                try
                {
                    httpPostedFile.SaveAs(fileSavePath);
                }
                catch (ApplicationException ex)
                {
                    return errorMsg(ex.Message);
                }

                if (isImage)
                {
                    string originalFolderPath = folderPath;
                    string thumbnailFolderPath = Path.Combine(folderPath, "thumbnail");

                    Image originalImage = StreamHelper.ImagePath2Img(fileSavePath);
                    string fileThumbnailSavePath = Path.Combine(thumbnailFolderPath, fileName + fileType);

                    folderExists = Directory.Exists(thumbnailFolderPath);
                    if (!folderExists)
                        Directory.CreateDirectory(thumbnailFolderPath);

                    Image newImage = ImageHelper.GetThumbNailImage(originalImage, 320, 320);
                    newImage.Save(fileThumbnailSavePath);

                    UploadMessage uploadMessageImg = new UploadMessage() {
                        OriginName = httpPostedFile.FileName,
                        FileName = fileName,
                        FileSize = httpPostedFile.ContentLength,
                        FileType = fileType,
                        DiskUploadPath = fileThumbnailSavePath,
                        UploadFilePath = WebApiApplication.UploadFolderVirualPath + fileSubFolder + companyID + "/" + yearMonth + "/" + "/thumbnail/" + fileName + fileType,
                        IsImage = true,
                        HasThumbnail = true,
                        OriginDiskUploadPath = fileSavePath,
                        OriginFilePath = WebApiApplication.UploadFolderVirualPath + fileSubFolder + companyID + "/" + yearMonth + "/" + fileName + fileType,
                    };

                    return uploadMessageImg;
                }

                UploadMessage uploadMessage = new UploadMessage()
                {
                    OriginName = httpPostedFile.FileName,
                    FileName = fileName,
                    FileSize = httpPostedFile.ContentLength,
                    FileType = fileType,
                    DiskUploadPath = fileSavePath,
                    UploadFilePath = WebApiApplication.UploadFolderVirualPath + fileSubFolder + companyID + "/" + yearMonth + "/" + fileName + fileType,
                    IsImage = false,
                    HasThumbnail = false,
                    OriginDiskUploadPath = fileSavePath,
                    OriginFilePath = WebApiApplication.UploadFolderVirualPath + fileSubFolder + companyID + "/" + yearMonth + "/" + fileName + fileType,
                };

                return uploadMessage;
            }

            return errorMsg("没有上传文件");
        }

        private UploadMessage _uploadImage(string companyID, bool convertToThumbnail = true)
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                FileExtension[] allowFileExtension = { FileExtension.BMP, FileExtension.GIF, FileExtension.JPG, FileExtension.PNG };

                if (!FileValidation.IsAllowedExtension(HttpContext.Current.Request.Files["file"], allowFileExtension))
                    return errorMsg("不接受的文件格式");

                // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
                string folderPath = WebApiApplication.UploadFolderPath;

                if (folderPath.StartsWith("~") || folderPath.StartsWith(".")) //相对路径
                    folderPath = HttpContext.Current.Server.MapPath(folderPath + "/images/" + companyID);
                else
                    folderPath = folderPath + "/images/" + companyID;

                string originalFolderPath = Path.Combine(folderPath, DateTime.Now.ToString("yyyyMM") + "/original");
                string thumbnailFolderPath = Path.Combine(folderPath, DateTime.Now.ToString("yyyyMM") + "/thumbnail");

                bool folderExists = Directory.Exists(originalFolderPath);
                if (!folderExists)
                    Directory.CreateDirectory(originalFolderPath);

                folderExists = Directory.Exists(thumbnailFolderPath);
                if (!folderExists)
                    Directory.CreateDirectory(thumbnailFolderPath);

                HttpPostedFile httpPostedFile = HttpContext.Current.Request.Files["file"];
                string fileType = httpPostedFile.FileName.Substring(httpPostedFile.FileName.LastIndexOf("."));
                string fileName = DateTime.Now.ToString("ddHHmmssff");
                string fileOriginalSavePath = Path.Combine(originalFolderPath, fileName + fileType);
                try
                {
                    httpPostedFile.SaveAs(fileOriginalSavePath);
                }
                catch (ApplicationException ex)
                {
                    return errorMsg(ex.Message);
                }

                if (!convertToThumbnail)
                    return successfulMsg(httpPostedFile.FileName, fileName, httpPostedFile.ContentLength, fileType,
                        fileOriginalSavePath,
                        WebApiApplication.UploadFolderVirualPath + "/images/" + companyID + "/" + DateTime.Now.ToString("yyyyMM") + "/original/" + fileName + fileType);
                try
                {
                    Image originalImage = StreamHelper.ImagePath2Img(fileOriginalSavePath);
                    string fileThumbnailSavePath = Path.Combine(thumbnailFolderPath, fileName + fileType);
                    Image newImage = ImageHelper.GetThumbNailImage(originalImage, 160, 160);
                    newImage.Save(fileThumbnailSavePath);

                    return successfulMsg(httpPostedFile.FileName, fileName, httpPostedFile.ContentLength, fileType,
                        fileOriginalSavePath,
                        WebApiApplication.UploadFolderVirualPath + "/images/" + companyID + "/" + DateTime.Now.ToString("yyyyMM") + "/thumbnail/" + fileName + fileType);

                }
                catch (ApplicationException ex)
                {
                    return errorMsg(ex.Message);
                }
            }

            return errorMsg("没有上传文件");
        }
    }

    public class UploadMessage
    {
        public int Status { get; set; }
        public string ErrorMsg { get; set; }
        public string OriginName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public string DiskUploadPath { get; set; }
        public string UploadFilePath { get; set; }
        public bool HasThumbnail { get; set; }
        public bool IsImage { get; set; }
        public string OriginFilePath { get; set; }
        public string OriginDiskUploadPath { get; set; }
    }
}

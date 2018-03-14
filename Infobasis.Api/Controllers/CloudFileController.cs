using Infobasis.Api.Data;
using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Infobasis.Api.Controllers
{
    [RoutePrefix("api/cloudfile")]
    public class CloudFileController : BaseApiController
    {
        private GenericRepository<CloudFolder> _repository;
        public CloudFileController()
        {
            _repository = unitOfWork.Repository<CloudFolder>();
        }

        [Route("filelist/{folderCode}")]
        [HttpGet]
        public IEnumerable<CloudFile> listFileList(string folderCode, string folderType)
        {
            int userID = UserInfo.GetCurrentUserID();
            int companyID = UserInfo.GetCurrentCompanyID();
            IQueryable<Infobasis.Data.DataEntity.CloudFile> q = DB.CloudFiles;
            q = q.Where(item => item.CloudFolder.Code == folderCode);

            if (folderType == "my")
            {
                q = q.Where(item => item.CreateByID == userID);
            }
            else
            {
                q = q.Where(item => item.CloudFolder.IsPublic == true);
            }

            return q;
        }

        [Route("folder/{folderCode}")]
        [HttpGet]
        public CloudFolder getFolderInfo(string folderCode)
        {
            int userID = UserInfo.GetCurrentUserID();
            int companyID = UserInfo.GetCurrentCompanyID();
            IQueryable<Infobasis.Data.DataEntity.CloudFolder> q = DB.CloudFolders;

            return q.Where(item => item.Code == folderCode).FirstOrDefault();
        }

        [Route("folderlist/{parentFolderCode}")]
        [HttpGet]
        public IEnumerable<CloudFolder> listFolderList(string parentFolderCode, string folderType)
        {
            int userID = UserInfo.GetCurrentUserID();
            int companyID = UserInfo.GetCurrentCompanyID();

            IQueryable<Infobasis.Data.DataEntity.CloudFolder> q = DB.CloudFolders;

            if (parentFolderCode != null && parentFolderCode != "0")
                q = q.Where(item => item.ParentCloudFolder.Code == parentFolderCode);

            if (folderType == "my")
            {
                q = q.Where(item => item.CreateByID == userID && item.IsPublic == false);
            }
            else
            {
                q = q.Where(item => item.IsPublic == true);
            }

            return q;
        }

        [Route("file/{fileID:int}")]
        [HttpPut]
        public IHttpActionResult updateCloudFile([FromBody] CloudFileUpdateDTO cloudFileUpdateDTO, int fileID)
        {
            if (cloudFileUpdateDTO == null)
                return BadRequest("Invalid Data");

            CloudFile cloudFile = DB.CloudFiles.Find(fileID);
            if (cloudFile == null)
            {
                return BadRequest("Invalid Data");
            }

            if (cloudFileUpdateDTO.FileName == null || cloudFileUpdateDTO.FileName.Length == 0)
            {
                cloudFileUpdateDTO.FileName = cloudFile.OriginName;
            }

            cloudFile.ClientName = cloudFileUpdateDTO.FileName;
            cloudFile.ClientDesc = cloudFileUpdateDTO.FileDesc;
            DB.SaveChanges();

            return Ok(cloudFile);
        }

        [Route("folderlist/{parentFolderCode}")]
        [HttpPost]
        public IHttpActionResult addCloudFolder([FromBody] CloudFolder cloudFolder, string parentFolderCode)
        {
            if (cloudFolder == null)
                return BadRequest("Invalid Data");

            if (cloudFolder.Name == null || cloudFolder.Name.Length == 0)
            {
                return BadRequest("");
            }

            int? parentID = null;
            if (parentFolderCode != null && parentFolderCode != "0")
            {
                var parent = DB.CloudFolders.Where(item => item.Code == parentFolderCode).FirstOrDefault();
                if (parent != null)
                    parentID = parent.ID;
            }

            int userID = UserInfo.GetCurrentUserID();
            int companyID = UserInfo.GetCurrentCompanyID();

            cloudFolder.Code = Guid.NewGuid().ToString("N");
            cloudFolder.CreateByID = userID;
            cloudFolder.CreateDatetime = DateTime.Now;
            cloudFolder.CompanyID = companyID;
            cloudFolder.ParentID = parentID;

            if (!_repository.Insert(cloudFolder, out msg, true))
                return BadRequest(msg);

            return Ok(cloudFolder);
        }

    }

    public class CloudFileUpdateDTO
    {
        public string FileName { get; set; }
        public string FileDesc { get; set; }
        public string FileTags { get; set; }
    }
}

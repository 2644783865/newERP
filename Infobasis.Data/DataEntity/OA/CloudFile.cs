using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbCloudFile")]
    public class CloudFile : TenantEntity
    {
        public int CloudFolderID { get; set; }
        [StringLength(300)]
        public string OriginName { get; set; }
        [StringLength(300)]
        public string FileName { get; set; }
        public int FileSize { get; set; }
        [StringLength(30)]
        public string EntityCode { get; set; }
        public bool HasThumbnail { get; set; }
        /// <summary>
        /// 显示路径，可能为缩略图
        /// </summary>
        [StringLength(300)]
        public string FilePath { get; set; }
        [StringLength(300)]
        public string DiskUploadPath { get; set; }

        /// <summary>
        /// 原始大小图位置
        /// </summary>
        [StringLength(300)]
        public string OriginFilePath { get; set; }
        [StringLength(300)]
        public string OriginDiskUploadPath { get; set; }

        [StringLength(300)]
        public string FileTypeName { get; set; }
        public bool IsValidImage { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        [StringLength(300)]
        public string SHA1Hash { get; set; }
        [StringLength(300)]
        public string ClientName { get; set; }
        public string ClientDesc { get; set; }
        public string ClientTags { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("CloudFolderID")]
        public virtual CloudFolder CloudFolder { get; set; }
    }
}

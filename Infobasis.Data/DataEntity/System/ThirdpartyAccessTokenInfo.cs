using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    /// <summary>
    /// 外部第三方接口
    /// </summary>
    [Table("SYtbThirdpartyAccessTokenInfo")]
    public class ThirdpartyAccessTokenInfo : TenantEntity
    {
        [Key]
        public int ID { get; set; }
        public ThirdpartyType ThirdpartyType { get; set; }
        [MaxLength(100)]
        public string AppKey { get; set; }
        [MaxLength(100)]
        public string Secret { get; set; }
        public bool? IsActive { get; set; }
        [MaxLength(100)]
        public string AccessToken { get; set; }
        public DateTime? LastGetAccessTokenTime { get; set; }
        public int? AccessTokenInvalidSeconds { get; set; } //失效秒数
        [MaxLength(1000)]
        public string ErrorMsg { get; set; }
        public DateTime? LastFetchTime { get; set; }
        public bool? SyncDept { get; set; }
        public bool? SyncPerson { get; set; }
        public bool? SyncAttendance { get; set; }

    }

    public enum ThirdpartyType
    { 
        [Description("钉钉")]
        DingTalkApp = 0
    }
}

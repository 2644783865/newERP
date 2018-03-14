using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbCompany")]
    public class Company
    {        
        public int ID { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }
        [MaxLength(60)]
        public string CompanyCode { get; set; }

        //CoreHR Properties.
        [MaxLength(200)]
        public string CompanyLogo { get; set; }
        [MaxLength(60)]
        public string FullName { get; set; }
        public int IndustryID { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(60)]
        public string Email { get; set; }
        [MaxLength(60)]
        public string Tel { get; set; }
        [MaxLength(60)]
        public string WebSite { get; set; }
        [MaxLength(60)]
        public string Fax { get; set; }
        [MaxLength(200)]
        public string Notes { get; set; }

        /// <summary>
        /// 最大员工数
        /// </summary>
        public int? MaxEmployees { get; set; }
        public int? MaxUsers { get; set; }
        public bool? StrongPasswords { get; set; }
        public byte? MinPasswordLength { get; set; }
        public byte? MaxPasswordLength { get; set; }
        /// <summary>
        /// 登录错误尝试次数
        /// </summary>
        public byte? MaxLogonAttempts { get; set; }
        /// <summary>
        /// 登录错误达到次数，锁定多少分钟
        /// </summary>
        public int? AccountLockoutMinutes { get; set; }
        public bool? MustAcceptLegal { get; set; }
        /// <summary>
        /// 系统管理员的公司
        /// </summary>
        public bool? IsSystemAdminCompany { get; set; }

        /// <summary>
        /// 状态，可用来关闭公司
        /// </summary>
        public CompanyStatus? CompanyStatus { get; set; }
        [MaxLength(200)]
        public string CompanyStatusDesc { get; set; }
        [MaxLength(20)]
        public string Sheng { get; set; }
        [MaxLength(20)]
        public string Shi { get; set; }
        [MaxLength(60)]
        public string ClientAdminAccount { get; set; }

        public int? CreateByID { get; set; }
        public string CreateByName { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public int? LastUpdateByID { get; set; }
        public string LastUpdateByName { get; set; }
        public DateTime? LastUpdateDatetime { get; set; }
        public DateTime? ExpiredDatetime { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<User> Users { get; set; }
    }

    public enum CompanyStatus
    {
        [Description("正常")]
        Enabled = 0,
        [Description("关闭")]
        Disabled = 1,
        [Description("过期")]
        Expired = 3
    }
}
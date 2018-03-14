using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Infobasis.Data.DataEntity
{
    [Table("SMtbVendor")]
    public class Vendor : TenantEntity
    {
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(300)]
        public string FullName { get; set; }
        [StringLength(200)]
        public string SpellCode { get; set; }
        [StringLength(30)]
        public string FirstSpellCode { get; set; }
        [StringLength(30)]
        public string Code { get; set; }
        public int? BrandID { get; set; }
        [StringLength(200)]
        public string BrandName { get; set; }

        public int? ProvinceID { get; set; }
        [StringLength(30)]
        public string ProvinceName { get; set; }

        [StringLength(200)]
        public string Location { get; set; }
        [StringLength(100)]
        public string ContactName { get; set; }
        [StringLength(100)]
        public string ContactTel { get; set; }
        [StringLength(100)]
        public string ContactCellPhone { get; set; }
        [StringLength(100)]
        public string Fax { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(200)]
        public string WebSite { get; set; }
        [StringLength(100)]
        public string QQ { get; set; }
        [StringLength(100)]
        public string WeChat { get; set; }
        [StringLength(30)]
        public string ERPAccount { get; set; }
        [StringLength(100)]
        public string ERPPassword { get; set; }
        public bool OpenERPAccount { get; set; }
        [StringLength(100)]
        public string BankAccount { get; set; }
        [StringLength(100)]
        public string BankAccountName { get; set; }

        /// <summary>
        /// 主，辅材
        /// </summary>
        public int? MainMaterialTypeID { get; set; }
        [StringLength(100)]
        public string MainMaterialTypeName { get; set; }
        /// <summary>
        /// 材料分类  木材类等
        /// </summary>
        public int? MaterialTypeID { get; set; }
        [StringLength(100)]
        public string MaterialTypeName { get; set; }

        /// <summary>
        /// 账期
        /// </summary>
        public int? PaymentNum { get; set; }

        [StringLength(100)]
        public string CompanySize { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [StringLength(100)]
        public string LogoPicPath { get; set; }

        public string Desc { get; set; }
        public string Remark { get; set; }
        public VendorStatus VendorStatus { get; set; }
        [StringLength(100)]
        public string VendorStatusName { get; set; }
        public int DisplayOrder { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("ProvinceID")]
        public virtual Province Province { get; set; }
        [JsonIgnoreAttribute]
        public virtual ICollection<VendorContact> VendorContacts { get; set; }

    }

    public enum VendorStatus
    {
        None = 0, 
        [Description("合格")]
        Qualified = 1,
        [Description("临时")]
        Temporary = 2,
        [Description("停止")]
        Closed = 3

    }
}

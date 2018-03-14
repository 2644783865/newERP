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
    [Table("SMtbMaterial")]
    public class Material : TenantEntity
    {
        public int? ProvinceID { get; set; }
        [StringLength(200)]
        public string ProvinceName { get; set; }

        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        public string SpellCode { get; set; }
        [StringLength(30)]
        public string FirstSpellCode { get; set; }
        [StringLength(30)]
        public string Code { get; set; }
        public int? BrandID { get; set; }
        [StringLength(200)]
        public string BrandName { get; set; }

        [StringLength(300)]
        public string PicPath { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        [StringLength(200)]
        public string Model { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        [StringLength(200)]
        public string Spec { get; set; }

        public int? UnitID { get; set; }
        [StringLength(30)]
        public string UnitName { get; set; }

        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public bool NoSalePrice { get; set; }
        public decimal UpgradePrice { get; set; }
        /// <summary>
        /// 利润系数
        /// </summary>
        public decimal EarningFactor { get; set; }
        /// <summary>
        /// 退货系数
        /// </summary>
        public decimal ReturnFactor { get; set; }
        /// <summary>
        /// 定制配置：标配，增配
        /// </summary>
        public int? CustomizationTypeID { get; set; }
        [StringLength(30)]
        public string CustomizationTypeName { get; set; }

        public int? VendorID { get; set; }
        public string VendorName { get; set; }

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
        /// 预算类型 (适用范围), 多选
        /// </summary>
        [StringLength(100)]
        public string BudgetTypeIDs { get; set; }
        [StringLength(200)]
        public string BudgetTypeNames { get; set; }
        /// <summary>
        /// 适用房间：厨房
        /// </summary>
        [StringLength(100)]
        public string RoomTypeIDs { get; set; }
        [StringLength(200)]
        public string RoomTypeNames { get; set; }

        public int DisplayOrder { get; set; }
        public string Remark { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("VendorID")]
        public virtual Vendor Vendor { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("BrandID")]
        public virtual Brand Brand { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("ProvinceID")]
        public virtual Province Province { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<BudgetTemplateItemMaterial> BudgetTemplateItemMaterials { get; set; }
    }
}

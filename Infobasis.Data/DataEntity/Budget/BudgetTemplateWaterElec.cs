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
    [Table("SMtbBudgetTemplateWaterElec")]
    public class BudgetTemplateWaterElec : TenantEntity
    {
        public int BudgetTemplateID { get; set; }
        /// <summary>
        /// 水路或电路
        /// </summary>
        public int WaterElecTypeID { get; set; }
        public string WaterElecTypeName { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public decimal Num { get; set; }
        public decimal MainMaterialPrice { get; set; }
        public decimal SupplementalMaterialPrice { get; set; }
        /// <summary>
        /// 人工费单价
        /// </summary>
        public decimal LabourPrice { get; set; }
        public int DisplayOrder { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("BudgetTemplateID")]
        public virtual BudgetTemplate BudgetTemplate { get; set; }
    }
}

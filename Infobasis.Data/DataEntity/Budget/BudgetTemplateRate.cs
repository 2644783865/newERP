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
    [Table("SMtbBudgetTemplateRate")]
    public class BudgetTemplateRate : TenantEntity
    {
        public int BudgetTemplateID { get; set; }
        /// <summary>
        /// 人工费单价
        /// </summary>
        public decimal LabourPrice { get; set; }
        /// <summary>
        /// 管理费比例
        /// </summary>
        public decimal ManagementRatePercent { get; set; }
        public decimal TaxRatePercent { get; set; }
        public string Desc { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("BudgetTemplateID")]
        public virtual BudgetTemplate BudgetTemplate { get; set; }
    }
}

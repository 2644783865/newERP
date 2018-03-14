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
    [Table("SMtbBudgetTemplate")]
    public class BudgetTemplate : TenantEntity
    {
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(30)]
        public string Code { get; set; }
        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// 预算类型
        /// </summary>
        public int BudgetTypeID { get; set; }
        [StringLength(100)]
        public string BudgetTypeName { get; set; }

        public int? ProvinceID { get; set; }
        [StringLength(30)]
        public string ProvinceName { get; set; }
        public string Remark { get; set; }

        public BudgetTemplateStatus BudgetTemplateStatus { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("ProvinceID")]
        public virtual Province Province { get; set; }
    }
}

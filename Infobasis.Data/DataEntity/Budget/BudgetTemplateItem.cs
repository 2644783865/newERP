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
    [Table("SMtbBudgetTemplateItem")]
    public class BudgetTemplateItem : TenantEntity
    {
        public int BudgetTemplateID { get; set; }
        /// <summary>
        /// 部位类型
        /// </summary>
        public int PartTypeID { get; set; }
        [StringLength(100)]
        public string PartTypeName { get; set; }
        public int DisplayOrder { get; set; }

        public string Remark { get; set; }

        [NotMapped]
        public int MaterialCount { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("BudgetTemplateID")]
        public virtual BudgetTemplate BudgetTemplate { get; set; }
        [JsonIgnoreAttribute]
        [ForeignKey("PartTypeID")]
        public virtual EntityListValue EntityListValue { get; set; }
        [JsonIgnoreAttribute]
        public virtual ICollection<BudgetTemplateItemMaterial> BudgetTemplateItemMaterials { get; set; }
    }
}

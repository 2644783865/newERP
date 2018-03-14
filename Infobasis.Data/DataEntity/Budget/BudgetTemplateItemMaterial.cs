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
    [Table("SMtbBudgetTemplateItemMaterial")]
    public class BudgetTemplateItemMaterial : TenantEntity
    {
        public int BudgetTemplateItemID { get; set; }
        public int MaterialID { get; set; }
        [StringLength(100)]
        public string Qty { get; set; }
        [StringLength(100)]
        public string Price { get; set; }
        public int DisplayOrder { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("BudgetTemplateItemID")]
        public virtual BudgetTemplateItem BudgetTemplateItem { get; set; }
        [JsonIgnoreAttribute]
        [ForeignKey("MaterialID")]
        public virtual Material Material { get; set; }
    }
}

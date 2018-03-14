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
    [Table("SMtbBudgetTemplateBasePrice")]
    public class BudgetTemplateBasePrice : TenantEntity
    {
        public int BudgetTemplateID { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal BaseNum { get; set; }
        public int DisplayOrder { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("BudgetTemplateID")]
        public virtual BudgetTemplate BudgetTemplate { get; set; }
    }
}

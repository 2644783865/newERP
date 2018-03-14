using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbBudgetTemplateSpace")]
    public class BudgetTemplateSpace : TenantEntity
    {
        public int BudgetTemplateDataID { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public decimal Size { get; set; }
        public decimal Amount { get; set; }
        public int? CopyFromID { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("BudgetTemplateDataID")]
        public virtual BudgetTemplateData BudgetTemplateData { get; set; }
    }
}

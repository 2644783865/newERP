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
    [Table("SMtbBudgetTemplateInclude")]
    public class BudgetTemplateInclude : TenantEntity
    {
        public int BudgetTemplateID { get; set; }
        public string Include { get; set; }
        public string Exclude { get; set; }
        public int DisplayOrder { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("BudgetTemplateID")]
        public virtual BudgetTemplate BudgetTemplate { get; set; }
    }
}

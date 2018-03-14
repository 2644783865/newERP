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
    [Table("SYtbBudgetTemplateData")]
    public class BudgetTemplateData : TenantEntity
    {
        public int UserID { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(60)]
        public string Code { get; set; }
        public int? CopyFromID { get; set; }
        public int? PublishFromID { get; set; }
        [StringLength(200)]
        public string PublishFromName { get; set; }
        [StringLength(600)]
        public string Remark { get; set; }

        public BudgetTemplateStatus BudgetTemplateStatus { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        [JsonIgnoreAttribute]
        public virtual ICollection<BudgetTemplateSpace> BudgetTemplateSpaces { get; set; }
    }

    public enum BudgetTemplateStatus
    { 
        Enabled = 0,
        Disabled = 1
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbEntityListValue")]
    public class EntityListValue : TenantEntity
    {
        public int EntityListID { get; set; }
        [StringLength(120)]
        public string Code { get; set; }
        [StringLength(120)]
        public string Name { get; set; }
        [StringLength(60)]
        public string Value { get; set; }
        [StringLength(20)]
        public string Color { get; set; }
        public int DisplayOrder { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("EntityListID")]
        public virtual EntityList EntityList { get; set; }
    }
}

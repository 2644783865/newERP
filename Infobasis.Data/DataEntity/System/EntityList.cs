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
    [Table("SYtbEntityList")]
    public class EntityList : TenantEntity
    {
        [StringLength(60)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        [DefaultValue(false)]
        public bool HasColorDefine { get; set; }
        public int DisplayOrder { get; set; }
        [StringLength(10)]
        public string CodePrefix { get; set; }
        [StringLength(10)]
        public string GroupCode { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<EntityListValue> EntityListValues { get; set; }
    }
}

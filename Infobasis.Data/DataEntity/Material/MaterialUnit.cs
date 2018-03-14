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
    [Table("SMtbMaterialUnit")]
    public class MaterialUnit : TenantEntity
    {
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(30)]
        public string Code { get; set; }
        public int DisplayOrder { get; set; }
        public string Remark { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<Material> Materials { get; set; }
    }
}

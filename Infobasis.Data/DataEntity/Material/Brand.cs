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
    [Table("SMtbBrand")]
    public class Brand : TenantEntity
    {
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(30)]
        public string Code { get; set; }
        [StringLength(200)]
        public string SpellCode { get; set; }
        [StringLength(30)]
        public string FirstSpellCode { get; set; }
        public int DisplayOrder { get; set; }
        public string Remark { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<Material> Materials { get; set; }
    }
}

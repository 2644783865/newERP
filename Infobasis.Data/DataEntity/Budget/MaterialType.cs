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
    [Table("SMtbMaterialType")]
    public class MaterialType : TenantEntity
    {
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<MaterialTypeList> MaterialTypeLists { get; set; }
    }
}

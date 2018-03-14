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
    [Table("SMtbMaterialTypeList")]
    public class MaterialTypeList : TenantEntity
    {
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }

        public int MaterialTypeID { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("MaterialTypeID")]
        public virtual MaterialType MaterialType { get; set; }
    }
}

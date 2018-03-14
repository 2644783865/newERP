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
    [Table("SYtbCity")]
    public class City : TenantEntity
    {
        [MaxLength(20)]
        public string Code { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public int ProvinceID { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnoreAttribute]
        public virtual Province Province { get; set; }
    }
}

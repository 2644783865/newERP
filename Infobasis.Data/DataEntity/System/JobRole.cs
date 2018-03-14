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
    [Table("SYtbJobRole")]
    public class JobRole : TenantEntity
    {
        [MaxLength(20)]
        public string Code { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public int ParentID { get; set; }
        public int? Level { get; set; }
        public int DisplayOrder { get; set; }
        [MaxLength(20)]
        public string Color { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(600)]
        public string Remark { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<User> Users { get; set; }
    }
}

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
    [Table("SYtbModule")]
    public class Module : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public int ParentID { get; set; }
        [MaxLength(200)]
        public string Url { get; set; }
        [MaxLength(20)]
        public string Icon { get; set; }
        [MaxLength(20)]
        public string IconFont { get; set; }
        public int DisplayOrder { get; set; }
        [DefaultValue(false)]
        public bool Expanded { get; set; }
        [DefaultValue(false)]
        public bool Highlight { get; set; }
        [MaxLength(20)]
        public string Target { get; set; }
        [MaxLength(1000)]
        public string Remark { get; set; }
        [DefaultValue(true)]
        public bool Active { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<ModulePermissionRole> ModulePermissionRoles { get; set; }
    }
}

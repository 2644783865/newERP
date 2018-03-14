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
    [Table("SYtbModulePermissionRole")]
    public class ModulePermissionRole : TenantEntity
    {
        public int ModuleID { get; set; }
        public int PermissionRoleID { get; set; }

        [JsonIgnoreAttribute]
        public virtual Module Module { get; set; }
        [JsonIgnoreAttribute]
        public virtual PermissionRole PermissionRole { get; set; }

    }
}

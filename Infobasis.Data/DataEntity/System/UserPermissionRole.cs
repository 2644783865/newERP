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
    /// <summary>
    /// 用户角色表
    /// </summary>
    [Table("SYtbUserPermissionRole")]
    public class UserPermissionRole : TenantEntity
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int PermissionRoleID { get; set; }

        [JsonIgnoreAttribute]
        public virtual User User { get; set; }
        [JsonIgnoreAttribute]
        public virtual PermissionRole PermissionRole { get; set; }
    }
}

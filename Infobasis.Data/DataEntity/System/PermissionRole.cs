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
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("SYtbPermissionRole")]
    public class PermissionRole : TenantEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
        [MaxLength(20)]
        public string Color { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        [DefaultValue(false)]
        public bool IsClientAdminRole { get; set; }
        /// <summary>
        /// 禁止删除
        /// </summary>
        [DefaultValue(false)]
        public bool ForbidDelete { get; set; }
        [MaxLength(600)]
        public string Remark { get; set; }

        [NotMapped]
        public int CountofUsers { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<UserPermissionRole> UserPermissionRoles { get; set; }
        [JsonIgnoreAttribute]
        public virtual ICollection<ModulePermissionRole> ModulePermissionRoles { get; set; }
    }
}

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
    [Table("SYtbCloudFolder")]
    public class CloudFolder : TenantEntity
    {
        [StringLength(32)]
        public string Code { get; set; }
        [StringLength(300)]
        public string Name { get; set; }
        public int? ParentID { get; set; }
        public bool IsPublic { get; set; }
        public int DisplayOrder { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<CloudFile> CloudFiles { get; set; }
        [JsonIgnoreAttribute]
        public virtual CloudFolder ParentCloudFolder { get; set; }
        [JsonIgnoreAttribute]
        public virtual ICollection<CloudFolder> ChildrenCloudFolders { get; set; }
    }
}

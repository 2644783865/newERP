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
    [Table("SYtbMeetingTask")]
    public class MeetingTask : TenantEntity
    {
        public int MeetingID { get; set; }
        public int DisplayOrder { get; set; }
        public string Task { get; set; }
        public int? ApplyUserID { get; set; }
        public string ApplyUserDisplayName { get; set; }
        public DateTime? ApplyDateTime { get; set; }
        public string Remark { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("MeetingID")]
        public virtual Meeting Meeting { get; set; }
    }
}

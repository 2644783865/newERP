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
    [Table("SYtbMeeting")]
    public class Meeting : TenantEntity
    {
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(300)]
        public string Topic { get; set; }
        public string Content { get; set; }
        [StringLength(1000)]
        public string AttendanceIDs { get; set; }
        [StringLength(1000)]
        public string AttendanceNames { get; set; }
        public int? DeptID { get; set; }
        [StringLength(30)]
        public string DeptName { get; set; }
        public string MeetingRole { get; set; }
        public string MeetingSpirit { get; set; }
        public int? MeetingTypeID { get; set; }
        [StringLength(60)]
        public string MeetingTypeName { get; set; }
        public int? HostUserID { get; set; }
        [StringLength(60)]
        public string HostUserDisplayName { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<MeetingTask> MeetingTasks { get; set; }
    }
}

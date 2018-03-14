using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbNotification")]
    public class Notification : TenantEntity
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(20)]
        public string NotificationType { get; set; }
        [MaxLength(300)]
        public string Subject { get; set; }
        public string Content { get; set; }
        public int? FromUserID { get; set; } //为空时，就是System
        [MaxLength(20)]
        public string ActionEntityCode { get; set; }
        public int? ActionEntityID { get; set; }
        public DateTime? ActionDate { get; set; }
        [MaxLength(200)]
        public string ActionAdditionalData { get; set; }
        [MaxLength(20)]
        public string NotificationGroupCode { get; set; }
        public bool? Closed { get; set; }
        [MaxLength(200)]
        public string ClosedReason { get; set; }
        public DateTime? ClosedDate { get; set; }
    }
}

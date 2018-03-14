using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbNotificationReceiver")]
    public class NotificationReceiver : TenantEntity
    {
        [Key]
        public int ID { get; set; }
        public int NotificationID { get; set; }
        public int ReceiverID { get; set; }
        [MaxLength(20)]
        public string ReceiverEntityCode { get; set; }
        public DateTime? MarkReadDate { get; set; }
        public bool? Closed { get; set; }
        public string ClosedReason { get; set; }
        public DateTime? ClosedDate { get; set; }
    }
}

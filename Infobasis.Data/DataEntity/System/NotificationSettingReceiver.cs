using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbNotificationSettingReceiver")]
    public class NotificationSettingReceiver : TenantEntity
    {
        [Key]
        public int ID { get; set; }
        public int NotificationSettingID { get; set; }
        public int ReceiverID { get; set; }
        [MaxLength(20)]
        public string ReceiverEntityCode { get; set; }
        [MaxLength(1000)]
        public string ReceiverDesc { get; set; }
    }
}

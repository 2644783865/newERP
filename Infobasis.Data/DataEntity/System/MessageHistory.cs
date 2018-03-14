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
    [Table("SYtbMessageHistory")]
    public class MessageHistory : TenantEntity
    {
        public int ID { get; set; }
        [MaxLength(30)]
        public string MobileNumber { get; set; }
        [MaxLength(30)]
        public string UserName { get; set; }
        [MaxLength(300)]
        public string ExtendValue { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        public string Body { get; set; }
        public string Param { get; set; }
        [MaxLength(100)]
        public string Code { get; set; }
        public MessageHistoryType MessageType { get; set; }
        public MessageHistorySMSType SMSType { get; set; }
        [MaxLength(20)]
        public string IP { get; set; }
        [DefaultValue(false)]
        public bool IsUsed { get; set; }
    }

    public enum MessageHistoryType
    {
        SMS = 0,
        EMAIL = 1
    } 

    public enum MessageHistorySMSType
    {
        Registration = 0,
        UserCreation = 1,
        FindPassword = 2,
        ResetPassword = 3
    }
}

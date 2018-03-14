using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    ////消息队列,需要处理的消息,将来考虑其他like redis
    [Table("SYtbMessageQueue")]
    public class MessageQueue : TenantEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [MaxLength(20)]
        public string MessageGroup { get; set; }
        [MaxLength(20)]
        public string EntityCode { get; set; }
        public int EntityID { get; set; }
        [MaxLength(100)]
        public string ProcessCode { get; set; }
        public DateTime ActionDate { get; set; }
        public DateTime? ActionStartDate { get; set; }
        public DateTime? ActionEndDate { get; set; }
        public int? ProcessStatus { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}

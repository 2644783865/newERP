using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbEmail")]
    public class Email : TenantEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [MaxLength(200)]
        public string From { get; set; }
        [MaxLength(2000)]
        public string To { get; set; }
        [MaxLength(2000)]
        public string CC { get; set; }
        [MaxLength(200)]
        public string Subject { get; set; }
        public string Body { get; set; }
        public int? TrackingID { get; set; }
        public int? AttachedID { get; set; }
        public DateTime? SendTime { get; set; }
    }
}

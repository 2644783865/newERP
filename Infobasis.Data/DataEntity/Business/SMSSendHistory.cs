using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbSMSSendHistory")]
    public class SMSSendHistory : TenantEntity
    {
        [StringLength(200)]
        public string SMSType { get; set; }
        [StringLength(20)]
        public string Tel { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Content { get; set; }
        public DateTime? SentDate { get; set; }
    }
}

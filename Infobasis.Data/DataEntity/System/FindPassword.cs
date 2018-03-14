using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbFindPassword")]
    public class FindPassword : TenantEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [MaxLength(30)]
        public string PhoneNum { get; set; }
        [MaxLength(30)]
        public string EmployeeName { get; set; }
        [MaxLength(100)]
        public string VerifyCode { get; set; }
        [MaxLength(60)]
        public string Token { get; set; }
        [MaxLength(30)]
        public string IP { get; set; }
        public DateTime? ClosedDate { get; set; }
    }
}

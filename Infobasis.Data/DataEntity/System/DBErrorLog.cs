using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbDBErrorLog")]
    public class DBErrorLog : TenantEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int? ErrorSeverity { get; set; }
        public string ErrorMessage { get; set; }
        public int? ErrorLine { get; set; }
        public int? ErrorNumber { get; set; }
        public string ErrorProcedure { get; set; }
        public DateTime? ErrorDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity.System
{
    [Table("SYtbErrorLog")]
    public class ErrorLog : TenantEntity
    {
        public string ErrorMessage { get; set; }
        [MaxLength(2)]
        public string ErrorType { get; set; }
        [MaxLength(60)]
        public string CompanyCode { get; set; }
         [MaxLength(60)]
        public string UserName { get; set; }
    }
}

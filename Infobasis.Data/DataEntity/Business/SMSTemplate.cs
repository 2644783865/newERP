using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbSMSTemplate")]
    public class SMSTemplate : TenantEntity
    {
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(300)]
        public string Content { get; set; }
        [StringLength(60)]
        public string Code { get; set; }
        [StringLength(30)]
        public string TemplateType { get; set; }
        [StringLength(100)]
        public string TemplateTypeName { get; set; }
        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }
    }
}

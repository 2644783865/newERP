using Newtonsoft.Json;
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
    [Table("SMtbVendorContact")]
    public class VendorContact : TenantEntity
    {
        public int VendorID { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string RoleName { get; set; }
        [StringLength(100)]
        public string Tel { get; set; }
        [StringLength(100)]
        public string CellPhone { get; set; }
        [StringLength(100)]
        public string Fax { get; set; }
        [StringLength(100)]
        public string QQ { get; set; }
        [StringLength(100)]
        public string WeChat { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(30)]
        public string ERPAccount { get; set; }
        [StringLength(100)]
        public string ERPPassword { get; set; }
        public bool OpenERPAccount { get; set; }
        [StringLength(100)]
        public string BankAccount { get; set; }
        [DefaultValue(false)]
        public bool IsMainContact { get; set; }
        public int EmployeeStatus { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("VendorID")]
        public virtual Vendor Vendor { get; set; }
    }
}

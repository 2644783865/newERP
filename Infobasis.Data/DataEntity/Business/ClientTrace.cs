using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbClientTrace")]
    public class ClientTrace : TenantEntity
    {
        public int ClientID { get; set; }
        public int UserID { get; set; }
        [StringLength(200)]
        public string UserDisplayName { get; set; }
        public string TraceDesc { get; set; }
        public DateTime? NextTraceDate { get; set; }
        public int? ClientTraceStatusID { get; set; }
        [StringLength(200)]
        public string ClientTraceStatusName { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("ClientID")]
        public virtual Client Client { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}

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
    [Table("SYtbSystemAdmin")]
    public class SystemAdmin : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public bool Active { get; set; }

        [JsonIgnoreAttribute]
        public virtual User User { get; set; }
    }
}

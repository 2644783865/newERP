using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    public class BaseEntity : IKeyID
    {
        [Key]
        public int ID { get; set; }
        public int? CreateByID { get; set; }
        public string CreateByName { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public int? LastUpdateByID { get; set; }
        public string LastUpdateByName { get; set; }
        public DateTime? LastUpdateDatetime { get; set; }

    }
}

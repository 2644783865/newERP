using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbAnnouncement")]
    public class Announcement : TenantEntity
    {
        [StringLength(30)]
        public string Code { get; set; }
        [StringLength(30)]
        public string AnnounceTypeID { get; set; }
        [StringLength(30)]
        public string AnnounceTypeName { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PublisherID { get; set; }
        [StringLength(20)]
        public string Publisher { get; set; }
        public int? ReadNum { get; set; }
        public bool? RecordReadNum { get; set; }
    }
}

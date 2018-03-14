using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbUserGoal")]
    public class UserGoal : TenantEntity
    {
        public int UserID { get; set; }
        public int Year { get; set; }
        public decimal? Month1 { get; set; }
        public decimal? Month2 { get; set; }
        public decimal? Month3 { get; set; }
        public decimal? Month4 { get; set; }
        public decimal? Month5 { get; set; }
        public decimal? Month6 { get; set; }
        public decimal? Month7 { get; set; }
        public decimal? Month8 { get; set; }
        public decimal? Month9 { get; set; }
        public decimal? Month10 { get; set; }
        public decimal? Month11 { get; set; }
        public decimal? Month12 { get; set; }
        public decimal? Group1 { get; set; }
        public decimal? Group2 { get; set; }
        public decimal? Group3 { get; set; }
        public decimal? Group4 { get; set; }
        public decimal? Total { get; set; }

        [StringLength(200)]
        public string Name { get; set; }
        public string Remark { get; set; }
    }
}

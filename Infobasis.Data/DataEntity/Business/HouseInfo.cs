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
    [Table("SYtbHouseInfo")]
    public class HouseInfo : TenantEntity
    {
        [StringLength(300)]
        public string Name { get; set; }
        [StringLength(300)]
        public string NameSpellCode { get; set; }
        [StringLength(600)]
        public string Location { get; set; }
        public int? ProvinceID { get; set; }
        [StringLength(60)]
        public string ProvinceName { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public int? CityID { get; set; }
        [StringLength(60)]
        public string CityName { get; set; }
        /// <summary>
        /// 房屋类型 别墅，叠墅
        /// </summary>
        public int? HouseTypeID { get; set; }
        [StringLength(200)]
        public string HouseTypeName { get; set; }
        /// <summary>
        /// 开盘时间
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 交房时间
        /// </summary>
        public DateTime? CompletionDate { get; set; }
        public int? HouseNum { get; set; }
        public int? Price { get; set; }
        [StringLength(1000)]
        public string Remark { get; set; }
        /// <summary>
        /// 重点楼盘
        /// </summary>
        public bool IsImportant { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<Client> Clients { get; set; }
    }
}

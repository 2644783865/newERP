using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("EEtbEmployeeFixPayAdjust")]
    public class EmployeeFixPayAdjust : TenantEntity
    {
        public int? UserID { get; set; }
        /// <summary>
        /// 试用期固定工资
        /// </summary>
        public decimal ProbationFixPayValue { get; set; }
        /// <summary>
        /// 固定工资
        /// </summary>
        public decimal FixPayValue { get; set; }
        /// <summary>
        /// 岗位津贴
        /// </summary>
        public decimal JobAllowanceValue { get; set; }
        /// <summary>
        /// 交通津贴
        /// </summary>
        public decimal TrafficAllowanceValue { get; set; }
        /// <summary>
        /// 餐饮津贴
        /// </summary>
        public decimal DiningAllowanceValue { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime AdjustDate { get; set; }
        /// <summary>
        /// 调整原因
        /// </summary>
        public string AdjustReason { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ApplyDate { get; set; }
        /// <summary>
        /// 是否已调整
        /// </summary>
        public bool isAdjusted { get; set; }

        public int? PreID { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("PreID")]
        public virtual EmployeeFixPayAdjust PreEmployeeFixPayAdjust { get; set; }
        [JsonIgnoreAttribute]
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}

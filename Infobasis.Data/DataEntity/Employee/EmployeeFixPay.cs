using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("EEtbEmployeeFixPay")]
    public class EmployeeFixPay : TenantEntity
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


        [JsonIgnoreAttribute]
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("试用期固定工资: " + this.ProbationFixPayValue.ToString() + ", ");
            sb.Append("固定工资: " + this.FixPayValue + ", ");
            sb.Append("岗位津贴: " + this.JobAllowanceValue + ", ");
            sb.Append("交通津贴: " + this.TrafficAllowanceValue + ", ");
            sb.Append("餐饮津贴: " + this.DiningAllowanceValue + ", ");
            return sb.ToString();
        }
    }
}

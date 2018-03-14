using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;

namespace Infobasis.Data.DataEntity
{
    [Table("EEtbEmployeeContract")]
    public class EmployeeContract : TenantEntity
    {
        public int? UserID { get; set; }
        /// <summary>
        /// 合同编码
        /// </summary>
        [MaxLength(100)]
        public string ContractCode { get; set; }
        /// <summary>
        /// 合同期类型
        /// </summary>
        public int? ContractTermType { get; set; }
        /// <summary>
        /// 合同期限(年)
        /// </summary>
        [MaxLength(200)]
        public string ContractTerm { get; set; }
        /// <summary>
        /// 合同开始日期
        /// </summary>
        public DateTime? ContractStartDate { get; set; }
        /// <summary>
        /// 合同结束日期
        /// </summary>
        public DateTime? ContractEndDate { get; set; }
        /// <summary>
        /// 法人实体
        /// </summary>
        [MaxLength(200)]
        public string LegalEntity { get; set; }
        /// <summary>
        /// 用工形式
        /// </summary>
        public int? EmploymentType { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        [MaxLength(200)]
        public string JobTitle { get; set; }
        /// <summary>
        /// 合同工资
        /// </summary>
        [MaxLength(200)]
        public string ContractSalary { get; set; }
        /// <summary>
        /// 试用期(月)
        /// </summary>
        [MaxLength(200)]
        public string ProbationPeriod { get; set; }
        /// <summary>
        /// 终止日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 第几次续约
        /// </summary>
        public int? ContractTimes { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        public string Remark { get; set; }

        // Navigation properties 
        [JsonIgnoreAttribute]
        public virtual User User { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("合同号: " + this.ContractCode + ", ");
            sb.Append("职位: " + this.JobTitle + ", ");
            sb.Append("开始时间: " + (this.ContractStartDate.HasValue ? this.ContractStartDate.Value.ToString("yyyy-MM-dd") : "") + ", ");
            sb.Append("结束时间: " + (this.ContractEndDate.HasValue ? this.ContractEndDate.Value.ToString("yyyy-MM-dd") : "") + ", ");
            return sb.ToString();
        }
    }
}

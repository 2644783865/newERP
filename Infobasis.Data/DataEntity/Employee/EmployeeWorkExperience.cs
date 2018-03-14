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
    [Table("EEtbEmployeeWorkExperience")]
    public class EmployeeWorkExperience : TenantEntity
    {
        public int? UserID { get; set; }
        /// <summary>
        /// 工作单位
        /// </summary>
        [StringLength(100)]
        public string CompanyName { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        [StringLength(200)]
        public string JobTitle { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 工作职责
        /// </summary>
        public string JobDuty { get; set; }
        /// <summary>
        /// 工资
        /// </summary>
        [StringLength(20)]
        public string Salary { get; set; }
        /// <summary>
        /// 证明人
        /// </summary>
        [StringLength(1000)]
        public string References { get; set; }
        /// <summary>
        /// 证明人联系电话
        /// </summary>
        [StringLength(100)]
        public string ReferencesPhone { get; set; }
        /// <summary>
        /// 离职原因
        /// </summary>
        [StringLength(1000)]
        public string LeaveReason { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        // Navigation properties 
        [JsonIgnoreAttribute]
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("公司: " + this.CompanyName.ToString() + ", ");
            sb.Append("职位: " + this.JobTitle + ", ");
            sb.Append("离职原因: " + this.LeaveReason + ", ");
            sb.Append("开始时间: " + (this.StartDate.HasValue ? this.StartDate.Value.ToString("yyyy-MM-dd") : "") + ", ");
            sb.Append("结束时间: " + (this.EndDate.HasValue ? this.EndDate.Value.ToString("yyyy-MM-dd") : "") + ", ");
            return sb.ToString();
        }
    }
}

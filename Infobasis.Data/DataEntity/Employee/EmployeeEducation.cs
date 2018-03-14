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
    [Table("EEtbEmployeeEducation")]
    public class EmployeeEducation : TenantEntity
    {
        public int? UserID { get; set; }
        /// <summary>
        /// 学校
        /// </summary>
        [MaxLength(200)]
        public string EducationalInstitution { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        [MaxLength(200)]
        public string Major { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 学位
        /// </summary>
        [MaxLength(200)]
        public string AcademicDegree { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public int? Education { get; set; }
        [MaxLength(100)]
        public string EducationName { get; set; }
        /// <summary>
        /// 教育类型
        /// </summary>
        public int? EducationType { get; set; }
        [MaxLength(100)]
        public string EducationTypeName { get; set; }
        /// <summary>
        /// 是否为最高学历
        /// </summary>
        public bool? IsHighest { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        public string Remark { get; set; }

        // Navigation properties 
        [JsonIgnoreAttribute]
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("学校/机构: " + this.EducationalInstitution + ", ");
            sb.Append("专业: " + this.Major + ", ");
            sb.Append("学位: " + this.AcademicDegree + ", ");
            sb.Append("学历: " + this.EducationName + ", ");
            sb.Append("教育类型: " + this.EducationTypeName + ", ");
            sb.Append("是否为最高学历: " + this.IsHighest + ", ");
            sb.Append("开始时间: " + (this.StartDate.HasValue ? this.StartDate.Value.ToString("yyyy-MM-dd") : "") + ", ");
            sb.Append("结束时间: " + (this.EndDate.HasValue ? this.EndDate.Value.ToString("yyyy-MM-dd") : "") + ", ");
            return sb.ToString();
        }
    }
}

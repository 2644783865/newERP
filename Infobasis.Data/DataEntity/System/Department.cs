using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbDepartment")]
    public class Department : TenantEntity
    {
        [StringLength(60)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Code { get; set; }            //部门代码
        [StringLength(200)]
        public string FullName { get; set; }
        public Nullable<int> ParentID { get; set; } //上级部门
        public string Description { get; set; }     //部门描述
        public int? DepartmentType { get; set; }     //部门类型
        public int? HeadCount { get; set; }          //编制人数

        public int? LeaderID { get; set; } //部门负责人ID

        [StringLength(200)]
        public string LeaderName { get; set; } //部门负责人Name
        [NotMapped]
        public int? EECount { get; set; } //部门人数

        [DefaultValue(false)]
        public bool Enabled { get; set; }   //启用
        public Nullable<DateTime> EffectiveDate { get; set; } //启用日
        public Nullable<DateTime> EndDate { get; set; }       //结束日

        [DefaultValue(0)]
        public Nullable<int> DisplayOrder { get; set; } //排列顺序
        public DepartmentControlType? DepartmentControlType { get; set; }
        [StringLength(200)]
        public string DepartmentControlTypeName { get; set; }

        public int? ProvinceID { get; set; }

        [StringLength(30)]
        [NotMapped]
        public string ProvinceName { get; set; }

        /// <summary>
        /// 外部ID, 用于外部如钉钉数据同步
        /// </summary>
        [MaxLength(20)]
        public string ExternalID { get; set; }
        /// <summary>
        /// 外部同步过来的数据
        /// </summary>
        public bool? IsExternalSyncData { get; set; }

        [JsonIgnoreAttribute]
        //[ForeignKey("DepartmentID")]
        public virtual User User { get; set; }
        [JsonIgnoreAttribute]
        //[ForeignKey("DepartmentID")]
        public virtual ICollection<User> Users { get; set; }
        [JsonIgnoreAttribute]
        public virtual Department ParentDepartment { get; set; }
        [JsonIgnoreAttribute]
        public virtual ICollection<Department> ChildrenDepartments { get; set; }
    }

    public enum DepartmentControlType
    { 
        None = 0,

        [Description("业务部门")]
        Sale = 1,

        [Description("设计部门")]
        Design = 2,

        [Description("财务部门")]
        Finance = 3,

        [Description("工程部门")]
        Project = 4,

        [Description("客服部门")]
        Service = 5,

        [Description("行政部门")]
        Administrative = 6,

        [Description("市场部门")]
        Marketing = 7,

        [Description("总办")]
        Exoffice = 8
    }
}
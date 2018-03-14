using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbUser")]
    public class User : TenantEntity
    {        
        [MaxLength(60)]
        public string Name { get; set; }
        [MaxLength(60)]
        public string ChineseName { get; set; }
        [MaxLength(60)]
        public string EnglishName { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        [MaxLength(200)]
        public string Email { get; set; }
        [MaxLength(200)]
        public string Password { get; set; }
        /// <summary>
        /// password的Salt，新建，修改password时都要一个新的salt
        /// </summary>
        [MaxLength(60)]
        public string Salt { get; set; }
        /// <summary>
        /// 登录就刷新token
        /// </summary>
        [MaxLength(200)]
        public string Token { get; set; }
        public DateTime? TokenCreationDate { get; set; }
        public DateTime? LastLogonDate { get; set; }
        [DefaultValue(0)]
        public int LogonCount { get; set; }
        [DefaultValue(0)]
        public int FailedLogonCount { get; set; }
        public DateTime? LastFailedLogonDate { get; set; }
        public DateTime? LegalAcceptDate { get; set; }
        [DefaultValue(false)]
        public bool MustChangePassword { get; set; }
        public DateTime? AccountLockedUntil { get; set; }
        public bool Enabled { get; set; }
        [DefaultValue(50)]
        public int DefaultPageSize { get; set; }
        [DefaultValue(false)]
        public bool IsClientAdmin { get; set; }
        public UserType UserType { get; set; }
        [MaxLength(600)]
        public string UserPortraitPath { get; set; }
        [MaxLength(600)]
        public string Remark { get; set; }

        ///-----------员工信息------------------
        /// <summary>
        /// 员工编号
        /// </summary>
        [MaxLength(60)]
        public string EmployeeCode { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int? DepartmentID { get; set; }
        /// <summary>
        /// 部门,仅用做显示
        /// </summary>
        [NotMapped]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 职位ID
        /// </summary>
        public int? JobID { get; set; }
        /// <summary>
        /// 员工职级
        /// </summary>
        public int? EmployeeJobGradeID { get; set; }
        [NotMapped]
        public string EmployeeJobGradeName { get; set; }
        /// <summary>
        /// 职位,仅用做显示
        /// </summary>
        [NotMapped]
        public string JobName { get; set; }

        [MaxLength(60)]
        public string EmployeeSpellCode { get; set; }
        [StringLength(30)]
        public string FirstSpellCode { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? OnBoardDate { get; set; }
        /// <summary>
        /// 服务开始时间
        /// </summary>
        public DateTime? ServeStartDate { get; set; }
        /// <summary>
        /// 参加工作时间
        /// </summary>
        public DateTime? AttendWorkingDate { get; set; }
        /// <summary>
        /// 离职时间
        /// </summary>
        public DateTime? TerminateDate { get; set; }

        //Basic Information
        /// <summary>
        /// 雇佣状态
        /// </summary>
        public int? HireStatus { get; set; }
        [MaxLength(30)]
        [NotMapped]
        public string HireStatusName { get; set; }
        /// <summary>
        /// 用工形式
        /// </summary>
        public int? EmploymentType { get; set; }
        [MaxLength(30)]
        [NotMapped]
        public string EmploymentTypeName { get; set; }
        /// <summary>
        /// 汇报上级
        /// </summary>
        public int? ReportsTo { get; set; }
        /// <summary>
        /// 汇报上级名称,不添加到数据库
        /// </summary>
        [NotMapped]
        public string ReportsToName { get; set; }
        /// <summary>
        /// 拟转正日期
        /// </summary>
        public DateTime? ProbationEndDate { get; set; }
        /// <summary>
        /// 转正日期
        /// </summary>
        public DateTime? BecomeRegularDate { get; set; }
        /// <summary>
        /// 离职类型
        /// </summary>
        public int? TerminateType { get; set; }
        /// <summary>
        /// 离职原因
        /// </summary>
        [MaxLength(200)]
        public string TerminateReason { get; set; }

        //Basic Personal Information
        /// <summary>
        /// 人事备注
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        [MaxLength(60)]
        public string MaritalStatus { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        [MaxLength(60)]
        public string Education { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        [MaxLength(60)]
        public string IDType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        [MaxLength(60)]
        public string IDNumber { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        [MaxLength(30)]
        public string Ethnic { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        [MaxLength(30)]
        public string Nationality { get; set; }
        /// <summary>
        /// 即时通讯账号
        /// </summary>
        [MaxLength(60)]
        public string IMAccount { get; set; }
        /// <summary>
        /// 公司邮箱
        /// </summary>
        [MaxLength(100)]
        public string CompanyEmail { get; set; }
        /// <summary>
        /// 公司电话
        /// </summary>
        [MaxLength(60)]
        public string WorkPhone { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [MaxLength(30)]
        public string MobileNumber { get; set; }
        /// <summary>
        /// 通讯地址
        /// </summary>
        [MaxLength(300)]
        public string MailAddress { get; set; }
        /// <summary>
        /// 紧急联系人
        /// </summary>
        [MaxLength(100)]
        public string EmergencyContact { get; set; }
        /// <summary>
        /// 紧急联系人电话
        /// </summary>
        [MaxLength(60)]
        public string EmergencyContactPhone { get; set; }
        /// <summary>
        /// 紧急联系人地址
        /// </summary>
        [MaxLength(300)]
        public string EmergencyContactAddress { get; set; }
        /// <summary>
        /// 工作城市
        /// </summary>
        public int? CityID { get; set; }

        /// <summary>
        /// 工作城市,仅用做显示
        /// </summary>
        [NotMapped]
        public string CityName { get; set; }
        /// <summary>
        /// 户口类别
        /// </summary>
        public HuKouType? HuKouType { get; set; }

        /// <summary>
        /// 独生子女费
        /// </summary>
        public bool? IsOnlyChildAllowance { get; set; }

        ///// <summary>
        ///// 户口所在地城市ID
        ///// </summary>
        //public int? CityIDHuKou { get; set; }
        ///// <summary>
        ///// 户口所在地城市,仅用做显示
        ///// </summary>
        //[NotMapped]
        public string CityNameHuKou { get; set; }
        /// <summary>
        /// 政治面貌
        /// </summary>
        public int? PoliticsStatus { get; set; }
        /// <summary>
        /// 档案所在地
        /// </summary>
        [MaxLength(300)]
        public string ArchiveLocation { get; set; }
        /// <summary>
        /// 档案编号
        /// </summary>
        [MaxLength(30)]
        public string ArchiveNumber { get; set; }
        /// <summary>
        /// 是否有居住证
        /// </summary>
        public bool? IsHasResidenceCard { get; set; }
        /// <summary>
        /// 居住证编号
        /// </summary>
        [MaxLength(30)]
        public string ResidenceCardNumber { get; set; }
        /// <summary>
        /// 是否加入工会
        /// </summary>
        public bool? IsLaborUnionMember { get; set; }
        [MaxLength(30)]
        public string NativePlace { get; set; }

        /// <summary>
        /// 外部ID, 用于外部如钉钉数据同步
        /// </summary>
        [MaxLength(20)]
        public string ExternalID { get; set; }
        /// <summary>
        /// 外部同步过来的数据
        /// </summary>
        public bool? IsExternalSyncData { get; set; }
        [MaxLength(20)]
        public string WeChat { get; set; }

        [JsonIgnoreAttribute]
        public virtual Department Department { get; set; }
        //[JsonIgnoreAttribute]
        //public virtual City City { get; set; }
        [JsonIgnoreAttribute]
        [ForeignKey("ReportsTo")]
        public virtual User ReportsToUser { get; set; }
        [JsonIgnoreAttribute]
        public virtual ICollection<User> ManagedUsers { get; set; }


        [JsonIgnoreAttribute]
        public virtual ICollection<EmployeeBank> EmployeeBanks { get; set; }
        [JsonIgnoreAttribute]
        public virtual ICollection<EmployeeContract> EmployeeContracts { get; set; }
        [JsonIgnoreAttribute]
        public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }
        [JsonIgnoreAttribute]
        public virtual ICollection<UserPermissionRole> UserPermissionRoles { get; set; }
        [JsonIgnoreAttribute]
        [ForeignKey("UserID")]
        public virtual ICollection<SystemAdmin> SystemAdmins { get; set; }
        [JsonIgnoreAttribute]
        [ForeignKey("JobID")]
        public virtual JobRole JobRole { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("姓名: " + this.ChineseName + ", ");
            sb.Append("工号: " + this.EmployeeCode + ", ");
            sb.Append("电话: " + this.MobileNumber + ", ");
            sb.Append("性别: " + this.Gender + ", ");
            sb.Append("职位: " + this.JobName + ", ");
            int employeeTypeID = this.EmploymentType.HasValue ? this.EmploymentType.Value : 1;
            string employeeTypeName = "";
            if (employeeTypeID == 1)
                employeeTypeName = "全职";
            else if (employeeTypeID == 2)
                employeeTypeName = "兼职";
            else if (employeeTypeID == 3)
                employeeTypeName = "实习生";
            else if (employeeTypeID == 4)
                employeeTypeName = "临时工";

            sb.Append("用工形式: " + employeeTypeName + ", ");
            sb.Append("婚否: " + this.MaritalStatus + ", ");
            int reportID = this.ReportsTo.HasValue ? this.ReportsTo.Value : 0;

            sb.Append("汇报上级: " + (this.ReportsToUser != null ? this.ReportsToUser.ChineseName : reportID.ToString()) + ", ");
            sb.Append("证件号码: " + this.IDNumber + ", ");
            sb.Append("在职状态: " + (!this.HireStatus.HasValue || this.HireStatus.Value == 0 ? "在职" : "离职") + ", ");
            return sb.ToString();
        }
    }

    public enum UserType
    {
        Employee = 0,
        Vendor = 1
    }

    public enum HuKouType
    {
        Urban = 0,
        Rural = 1
    }
}
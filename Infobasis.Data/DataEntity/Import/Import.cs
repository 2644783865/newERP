using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity.Import
{
    [Table("SYtbImport")]
    public class Import : TenantEntity
    {
        /// <summary>
        /// 导入的编号, 方便导入后再次查找到这条记录
        /// </summary>
        [MaxLength(60)]
        public string ImportGuid { get; set; }
        /// <summary>
        /// 导入类型
        /// </summary>
        public ImportEntityType ImportEntityType { get; set; }
        /// <summary>
        /// 导入时间
        /// </summary>
        public DateTime ImportDate { get; set; }
        /// <summary>
        /// 导入行数
        /// </summary>
        public int TotalRows { get; set; }
        /// <summary>
        /// 错误行数
        /// </summary>
        public int ErrorRows { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        [MaxLength(200)]
        public string FileName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileLength { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        [MaxLength(300)]
        public string FileContentType { get; set; }
        /// <summary>
        /// 是否有严重错误
        /// </summary>
        public bool HasFatalError { get; set; }
        /// <summary>
        /// 严重错误信息
        /// </summary>
        public string FatalErrorMsg { get; set; }
        /// <summary>
        /// 空行数
        /// </summary>
        public int BlankRows { get; set; }
        /// <summary>
        /// 是否有一般错误
        /// </summary>
        public bool HasVerifyError { get; set; }
        /// <summary>
        /// 一般错误信息
        /// </summary>
        public string VerifyErrorMsg { get; set; }
        /// <summary>
        /// 一些配置信息
        /// </summary>
        [MaxLength(200)]
        public string SetupConfig { get; set; }

        /// <summary>
        /// 最后导入结果
        /// </summary>
        public string ImportResultMsg { get; set; }
        public int? ImportBy { get; set; }
    }

    public enum ImportEntityType
    { 
        Employee = 1,
        Department = 2,
        Job = 3,
        [Description("员工休假信息")]
        EmployeeLeave = 4,
        [Description("员工加班信息")]
        EmployeeOvertime = 5,
        [Description("社保公积金设置")]
        EmployeeSocialBenefitProfile = 6,
        [Description("社保公积金结果")]
        EmployeeBenefitContribution = 7,
        [Description("员工考勤卡号")]
        EmployeeTimeCard = 10,
        [Description("员工打卡记录")]
        EmployeeTimeRecord = 11,
        [Description("固定薪酬")]
        EmployeeFixpay = 12,
        [Description("每月计发录入")]
        EmployeeFloatpay = 13,
        [Description("员工补签信息")]
        EmployeeTimeFillup = 15,
        [Description("员工出差信息")]
        EmployeeBusinessTrip = 16,
        [Description("员工银行账号信息")]
        EmployeeBankingInfo = 17,
        [Description("员工报税信息")]
        EmployeeTaxInfo = 18,
        [Description("员工劳动合同")]
        EmployeeContractInfo = 19,
        [Description("员工协议")]
        EmployeeAgreementInfo = 20,
        [Description("员工教育经历")]
        EmployeeEducationInfo = 21,
        [Description("员工语言能力")]
        EmployeeLanguageInfo = 22,
        [Description("员工职业技术资格")]
        EmployeeQualificationInfo = 23,
        [Description("员工工作经历")]
        EmployeeWorkExperienceInfo = 24,
        [Description("员工社会关系")]
        EmployeeSocialRelationInfo = 25
    }
}

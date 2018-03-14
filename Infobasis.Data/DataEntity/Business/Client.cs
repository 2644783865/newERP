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
    [Table("SYtbClient")]
    public class Client : TenantEntity
    {
        [StringLength(30)]
        public string ProjectNo { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        public string SpellCode { get; set; }
        [StringLength(30)]
        public string FirstSpellCode { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        [StringLength(20)]
        public string Tel { get; set; }
        [StringLength(20)]
        public string QQ { get; set; }
        [StringLength(20)]
        public string WeChat { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        /// <summary>
        /// 业务部门
        /// </summary>
        public int? SalesDeptID { get; set; }
        [StringLength(200)]
        public string SalesDeptName { get; set; }
        /// <summary>
        /// 业务人员
        /// </summary>
        public int? SalesUserID { get; set; }
        [StringLength(200)]
        public string SalesUserDisplayName { get; set; }
        public DateTime? AssignToSalesDatetime { get; set; }

        /// <summary>
        /// 设计部门
        /// </summary>
        public int? DesignDeptID { get; set; }
        [StringLength(200)]
        public string DesignDeptName { get; set; }
        /// <summary>
        /// 设计师
        /// </summary>
        public int? DesignUserID { get; set; }
        [StringLength(200)]
        public string DesignUserDisplayName { get; set; }
        public DateTime? AssignToDesignerDatetime { get; set; }
        [StringLength(300)]
        public string AssignToDesignerRemark { get; set; }
        /// <summary>
        /// 工程部门
        /// </summary>
        public int? EngineeringDeptID { get; set; }
        [StringLength(200)]
        public string EngineeringDeptName { get; set; }
        /// <summary>
        /// 财务部门
        /// </summary>
        public int? FinanceDeptID { get; set; }
        [StringLength(200)]
        public string FinanceDeptName { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public double? BuiltupArea { get; set; }
        /// <summary>
        /// 预算
        /// </summary>
        public double? Budget { get; set; }
        /// <summary>
        /// 客户来源
        /// </summary>
        public int? ClientFromID { get; set; }
        [StringLength(200)]
        public string ClientFromName { get; set; }
        [Required, StringLength(300)]
        public string DecorationAddress { get; set; }

        public int? HouseInfoID { get; set; }
        /// <summary>
        /// 楼盘地址
        /// </summary>
        [StringLength(300)]
        public string HousesName { get; set; }
        public DateTime? DeliverHouseDate { get; set; }
        public DateTime? MeasuringHousingDate { get; set; }
        public int? PackageID { get; set; }
        [StringLength(200)]
        public string PackageName { get; set; }
        [StringLength(1000)]
        public string Remark { get; set; }

        public int? ProvinceID { get; set; }
        [StringLength(30)]
        public string ProvinceName { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public int? CityID { get; set; }
        [StringLength(200)]
        public string CityName { get; set; }

        /// <summary>
        /// 装修风格
        /// </summary>
        public int? DecorationStyleID { get; set; }
        [StringLength(200)]
        public string DecorationStyleName { get; set; }

        /// <summary>
        /// 装修类型 全装，精装
        /// </summary>
        public int? DecorationTypeID { get; set; }
        [StringLength(200)]
        public string DecorationTypeName { get; set; }
        /// <summary>
        /// 房屋结构 1室1厅
        /// </summary>
        public int? HouseStructTypeID { get; set; }
        [StringLength(200)]
        public string HouseStructTypeName { get; set; }
        /// <summary>
        /// 房屋用途 住宅，商铺，写字楼，别墅
        /// </summary>
        public int? HouseUseTypeID { get; set; }
        [StringLength(200)]
        public string HouseUseTypeName { get; set; }
        /// <summary>
        /// 房屋类型 别墅，叠墅
        /// </summary>
        public int? HouseTypeID { get; set; }
        [StringLength(200)]
        public string HouseTypeName { get; set; }
        /// <summary>
        /// 预计开工时间
        /// </summary>
        public DateTime? PlanStartDate { get; set; }
        /// <summary>
        /// 预计结束时间
        /// </summary>
        public DateTime? PlanEndDate { get; set; }
        /// <summary>
        /// 颜色取向
        /// </summary>
        public int? DecorationColorID { get; set; }
        [StringLength(200)]
        public string DecorationColorName { get; set; }
        /// <summary>
        /// 客户需求, 设计等
        /// </summary>
        [StringLength(200)]
        public string ClientNeedIDs { get; set; }
        [StringLength(200)]
        public string ClientNeedName { get; set; }

        public DesignStatus? DesignStatus { get; set; }
        /// <summary>
        /// 跟进状态
        /// </summary>
        public int? ClientTraceStatusID { get; set; }
        [StringLength(200)]
        public string ClientTraceStatusName { get; set; }
        public DateTime? DesignStatusUpdateDate { get; set; } 
        public ClientProjectStatus? ClientProjectStatus { get; set; }
        public DateTime? ClientProjectStatusUpdateDate { get; set; }
        /// <summary>
        /// 作废
        /// </summary>
        public bool? Disabled { get; set; }
        public int? DisableReasonID { get; set; }
        [StringLength(200)]
        public string DisableReasonName { get; set; }
        public DateTime? DisableDateTime { get; set; }
        [StringLength(300)]
        public string DisableReasonRemark { get; set; }
        public int? DisableByUserID { get; set; }
        [StringLength(200)]
        public string DisableByUserDisplayName { get; set; }

        public DateTime? LastTraceDate { get; set; }
        public DateTime? LastTraceBookingDate { get; set; }
        [NotMapped]
        public string LastTraceMsg { get; set; }
        [NotMapped]
        public int? TraceNum { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<ClientTrace> ClientTraces { get; set; }
        [JsonIgnoreAttribute]
        [ForeignKey("HouseInfoID")]
        public virtual HouseInfo HouseInfo { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("客户名: " + this.Name + ", ");
            sb.Append("电话: " + this.Tel + ", ");
            sb.Append("性别: " + this.Gender + ", ");
            sb.Append("QQ: " + this.QQ + ", ");
            sb.Append("装修地址: " + this.DecorationAddress + ", ");
            sb.Append("业务部: " + this.SalesDeptName + ", ");
            sb.Append("设计部: " + this.DesignDeptName + ", ");
            sb.Append("预算: " + this.Budget + ", ");
            sb.Append("楼盘: " + this.HouseInfo + ", ");
            sb.Append("装修类型: " + this.DecorationTypeName + ", ");
            sb.Append("装修风格: " + this.DecorationStyleName + ", ");
            sb.Append("颜色爱好: " + this.DecorationColorName + ", ");
            sb.Append("装修需求: " + this.ClientNeedName + ", ");
            sb.Append("状态: " + this.ClientProjectStatus + ", ");
            sb.Append("跟进状态: " + this.ClientTraceStatusName + ", ");
            sb.Append("操作时间: " + this.CreateDatetime.Value.ToString("yyyy-MM-dd hh:mm:ss") + ", ");

            return sb.ToString();
        }
    }

    public enum ClientProjectStatus
    {
        None = 0,
        Budget = 1,
        WaitToDecoration = 2,
        Decorating = 3,
        Finished = 4
    }

    public enum DesignStatus
    {
        None = 0,
        Designing = 2,
        DesignDone = 3
    }
}

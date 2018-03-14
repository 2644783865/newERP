using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("EEtbEmployeeAdjust")]
    public class EmployeeAdjust : TenantEntity
    {
        public int? UserID { get; set; }
        [StringLength(300)]
        public string AdjustItemName { get; set; }
        /// <summary>
        /// 调整前的值，可能来源很多数据源
        /// </summary>
        public int? OriginalValueID { get; set; }
        /// <summary>
        /// 预存调整前的值的文本,后台处理，前台不用传回
        /// </summary>
        public string OriginalValueName { get; set; }
        /// <summary>
        /// 调整值，可能来源很多数据源
        /// </summary>
        public int? AdjustValueID { get; set; }
        /// <summary>
        /// 调整值文本，后台处理，前台不用传回
        /// </summary>
        public string AdjustValueName { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? AdjustDate { get; set; }
        /// <summary>
        /// 调整原因，文本
        /// </summary>
        public string AdjustReason { get; set; }
        public string AllChangeData { get; set; }
        /// <summary>
        /// 是否已调整
        /// </summary>
        public bool isAdjusted { get; set; }

        public int? PreID { get; set; }

        [JsonIgnoreAttribute]
        [ForeignKey("PreID")]
        public virtual EmployeeAdjust PreEmployeeAdjust { get; set; }
        [JsonIgnoreAttribute]
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

    // 1=入职, 2=用工形式, 3=部门, 4=职位, 5=职级, 6=汇报上级,6=再入职, 99=离职
    public enum EmployeeAdjustItem
    {
        [Description("新增")]
        Create = 0,
        [Description("入职")]
        OnBoard = 1,
        [Description("用工形式")]
        EmploymentType = 2,
        [Description("部门")]
        Department = 3,
        [Description("职位")]
        Job = 4,
        [Description("职级")]
        EmployeeJobGrade = 5,
        [Description("汇报上级")]
        ReportsTo = 6,
        [Description("再入职")]
        ReOnBoard = 7,

        [Description("其他")]
        Others = 8,

        [Description("角色")]
        UserPermissionRole = 9,

        [Description("离职")]
        Terminate = 99
    }
}

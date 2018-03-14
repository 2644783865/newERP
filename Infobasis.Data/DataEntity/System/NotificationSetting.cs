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
    [Table("SYtbNotificationSetting")]
    public class NotificationSetting : TenantEntity
    {
        [Key]
        public int ID { get; set; }
        public NotificationSettingType SettingType { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Desc { get; set; }
        [MaxLength(200)]
        public string RemindingValue { get; set; }
        public NotificationRemindingType RemindingType { get; set; }
        public Boolean IsActive { get; set; }
    }

    public enum NotificationSettingType
    {
        [Description("转正")]
        BecomeRegular = 1,
        [Description("合同到期")]
        ContractExpire = 2,
        [Description("社保公积金处理")]
        ProcessSocial = 3,
        [Description("发薪")]
        PaySalary = 4,
        [Description("当月生日提醒")]
        BirthdayInCurrentMonth = 5,
        [Description("当天生日提醒")]
        BirthdayAtToday = 6      
    }

    public enum NotificationRemindingType
    { 
        [Description("每月")]
        EachMonth = 1,
        [Description("每天")]
        EachDay = 2,
        [Description("到期之前")]
        BeforeActionDay = 3,
        [Description("到期之后")]
        AfterActionDay = 4,
        [Description("当天")]
        ActionDay = 5
    }
}

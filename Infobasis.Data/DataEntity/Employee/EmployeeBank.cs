using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Infobasis.Data.DataEntity;
using System.Text;

namespace Infobasis.Data.DataEntity
{
    /// <summary>
    /// 员工银行账户信息
    /// </summary>
    [Table("EEtbEmployeeBank")]
    public class EmployeeBank : TenantEntity
    {
        public int UserID { get; set; }
        ///// <summary>
        ///// 薪酬年度
        ///// </summary>
        //public int Year { get; set; }
        ///// <summary>
        ///// 薪酬月份
        ///// </summary>
        //public int Month { get; set; }
        /// <summary>
        /// 开户人
        /// </summary>
        [MaxLength(200)]
        public string AccountHolder { get; set; }
        /// <summary>
        /// 银行名
        /// </summary>
        [MaxLength(300)]
        public string BankName { get; set; }
        /// <summary>
        /// 支行编码
        /// </summary>
        [MaxLength(300)]
        public string BankBranchCode { get; set; }
        /// <summary>
        /// 支行名
        /// </summary>
        [MaxLength(300)]
        public string BankBranchName { get; set; }
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MaxLength(100)]
        public string BankAccount { get; set; }
        public bool Default { get; set; }
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
            sb.Append("银行名: " + this.BankName + ", ");
            sb.Append("银行帐号: " + this.BankAccount + ", ");
            sb.Append("开户人: " + this.AccountHolder + ", ");
            sb.Append("备注: " + this.Remark + ", ");
            return sb.ToString();
        }
    }
}
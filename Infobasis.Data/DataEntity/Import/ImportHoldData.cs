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
    [Table("SYtbImportHoldData")]
    public class ImportHoldData : TenantEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(60)]
        public string ImportGuid { get; set; }
        /// <summary>
        /// 导入的行号
        /// </summary>
        public int RowNumber { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        [DefaultValue(false)]
        public bool IsValid { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public string Column6 { get; set; }
        public string Column7 { get; set; }
        public string Column8 { get; set; }
        public string Column9 { get; set; }
        public string Column10 { get; set; }
        public string Column11 { get; set; }
        public string Column12 { get; set; }
        public string Column13 { get; set; }
        public string Column14 { get; set; }
        public string Column15 { get; set; }
        public string Column16 { get; set; }
        public string Column17 { get; set; }
        public string Column18 { get; set; }
        public string Column19 { get; set; }
        public string Column20 { get; set; }
        public string Column21 { get; set; }
        public string Column22 { get; set; }
        public string Column23 { get; set; }
        public string Column24 { get; set; }
        public string Column25 { get; set; }
        public string Column26 { get; set; }
        public string Column27 { get; set; }
        public string Column28 { get; set; }
        public string Column29 { get; set; }
        public string Column30 { get; set; }
        public string Column31 { get; set; }
        public string Column32 { get; set; }
        public string Column33 { get; set; }
        public string Column34 { get; set; }
        public string Column35 { get; set; }
        public string Column36 { get; set; }
        public string Column37 { get; set; }
        public string Column38 { get; set; }
        public string Column39 { get; set; }
        public string Column40 { get; set; }
        public string Column41 { get; set; }
        public string Column42 { get; set; }
        public string Column43 { get; set; }
        public string Column44 { get; set; }
        public string Column45 { get; set; }
        public string Column46 { get; set; }
        public string Column47 { get; set; }
        public string Column48 { get; set; }
        public string Column49 { get; set; }
        public string Column50 { get; set; }
    }
}

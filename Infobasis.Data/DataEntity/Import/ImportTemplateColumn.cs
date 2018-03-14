using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity.Import
{
    [Table("SYtbImportTemplateColumn")]
    public class ImportTemplateColumn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        [MaxLength(100)]
        public string ColumnName { get; set; }
        /// <summary>
        /// 导入表列名
        /// </summary>
        [MaxLength(100)]
        public string DatabaseColumnName { get; set; }
        /// <summary>
        /// 最终导入数据列名
        /// </summary>
        [MaxLength(100)]
        public string ActualColumnName { get; set; }
        /// <summary>
        /// 导入类型
        /// </summary>
        public ImportEntityType ImportEntityType { get; set; }
        /// <summary>
        /// 数据类型 T - Text, N - Numeric, D - Date, B - Bool, I - Int
        /// </summary>
        [MaxLength(10)]
        public string DataType { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public int ColumnLength { get; set; }
        /// <summary>
        /// 来源其它表关联
        /// </summary>
        public bool FromLookupTable { get; set; }
        /// <summary>
        /// 其它表数据表名.列名
        /// </summary>
        [MaxLength(100)]
        public string LookupTableColumn { get; set; }
        /// <summary>
        /// 来源于枚举类型
        /// </summary>
        public bool FromLoopupArray { get; set; }
        /// <summary>
        /// 枚举类型数据 如1,2,3
        /// </summary>
        [MaxLength(1000)]
        public string LoopupArrayData { get; set; }
        /// <summary>
        /// 必须有值
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// 唯一
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        /// 如果是unique,则比较这列
        /// </summary>
        [MaxLength(100)]
        public string UniqueCompareColumn { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int ColumnOrder { get; set; }
    }
}

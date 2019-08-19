using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataModel
{
    public class AccountItemModel
    {
        /// <summary>
        /// 对应的excel
        /// </summary>
        public int ExcelRecordId { get; set; }

        /// <summary>
        /// 科目编号
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// 父级编号
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 科目层级
        /// </summary>
        public int? AccountLevel { get; set; }

        /// <summary>
        /// 类型：1：收入，2：支出
        /// </summary>
        public int AccountTypeId { get; set; }

        /// <summary>
        /// 相加类型：1：+  2：-
        /// </summary>
        public int AccountSumTypeId { get; set; }

        /// <summary>
        /// 是否叶子
        /// </summary>
        public int IsLeaf { get; set; }
    }
}

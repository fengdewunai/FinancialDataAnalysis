using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataModel
{
    /// <summary>
    /// 项目
    /// </summary>
    public class FinancialDataItemModel
    {
        /// <summary>
        /// 对应的excel
        /// </summary>
        public int ExcelRecordId { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 所属性质，只有项目层级有
        /// </summary>
        public int XingZhiId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 项目层级
        /// </summary>
        public int ItemLevel { get; set; }

        /// <summary>
        /// 是否叶子
        /// </summary>
        public int IsLeaf { get; set; }

        /// <summary>
        /// 类型：1：虚拟集团 2：事业部，3：片区，4：性质，5：项目
        /// <see cref="Model.Enum.FinancialDataItemTypeEnum"/>
        /// </summary>
        public int ItemTypeId { get; set; }
    }
}

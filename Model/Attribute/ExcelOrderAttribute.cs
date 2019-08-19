using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Attribute
{
    /// <summary>
    /// 用于Excel在实体中获取的字段排序
    /// </summary>
    public class ExcelOrderAttribute : System.Attribute
    {
        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 历史数据默认值
        /// </summary>
        public int HistoryVal { get; set; }

        /// <summary>
        /// 是否替换历史数据为空，true，则把值等于HistoryVal的设置为空
        /// </summary>
        public bool IsReplaceHisotry { get; set; }

        /// <summary>
        /// 列宽，默认-1表示不使用该参数
        /// </summary>
        public double ColWidth { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExcelOrderAttribute(int order, string unit = "", bool isReplaceHisotry = false, int historyVal = -1, double colWidth = -1)
        {
            Order = order;
            Unit = unit;
            IsReplaceHisotry = isReplaceHisotry;
            HistoryVal = historyVal;
            ColWidth = colWidth;
        }
    }
}

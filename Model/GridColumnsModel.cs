using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// grid表头数据
    /// </summary>
    public class GridColumnsModel
    {
        /// <summary>
        /// grid的Column定义
        /// </summary>
        public List<GridColumnsDetail> GridColumns { get; set; }

        /// <summary>
        /// Store的Fields定义
        /// </summary>
        public List<string> StoreFields { get; set; }
    }

    public class GridColumnsDetail
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 数据索引
        /// </summary>
        public string dataIndex { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string xtype { get; set; }

        /// <summary>
        /// 日期格式化
        /// </summary>
        public string dateFormat { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool hidden { get; set; }

        /// <summary>
        /// 日期格式化
        /// </summary>
        public List<GridColumnsModel> columns { get; set; }
    }
}

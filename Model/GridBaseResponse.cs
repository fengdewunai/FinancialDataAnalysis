using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// grid公用返回对象
    /// </summary>
    public class GridBaseResponsek<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T rows { get; set; }
    }
}

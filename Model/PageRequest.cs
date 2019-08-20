using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageRequest
    {
        /// <summary>
        /// 每页数据条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 页码索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 跳过页数
        /// </summary>
        public int PageSkip
        {
            get
            {
                return (PageIndex - 1) * PageSize;
            }
        }

        /// <summary>
        /// 最后一个id
        /// 用于分页
        /// </summary>
        public string LastId { get; set; }
    }
}

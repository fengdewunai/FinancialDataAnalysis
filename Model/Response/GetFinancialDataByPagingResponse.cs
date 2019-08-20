using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Response
{
    /// <summary>
    /// 获取详细数据（分页）输出参数
    /// </summary>
    public class GetFinancialDataByPagingResponse
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public long FinancialDataId { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 事业部
        /// </summary>
        public string ShiYeBu { get; set; }

        /// <summary>
        /// 片区
        /// </summary>
        public string PianQu { get; set; }

        /// <summary>
        /// 性质
        /// </summary>
        public string XingZhi { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string XiangMu { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string DetailData { get; set; }
    }
}

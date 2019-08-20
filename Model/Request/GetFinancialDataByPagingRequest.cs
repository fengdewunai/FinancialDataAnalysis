using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 获取详细数据（分页）
    /// </summary>
    public class GetFinancialDataByPagingRequest : PageRequest
    {
        /// <summary>
        /// ExcelRecordId
        /// </summary>
        public int ExcelRecordId { get; set; }

        /// <summary>
        /// 科目编号
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// 期间类型
        /// </summary>
        public int QiJianTypeId { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public int XiangMuTypeId { get; set; }

        /// <summary>
        /// 项目类型对应的Id，如果是集团会有多个
        /// </summary>
        public string XiangMuItemId { get; set; }
    }
}

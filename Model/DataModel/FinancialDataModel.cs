using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataModel
{
    /// <summary>
    /// excel数据
    /// </summary>
    public class FinancialDataModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long FinancialDataId { get; set; }

        /// <summary>
        /// 所属excel
        /// </summary>
        public int ExcelRecordId { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public int XiangMuIdId { get; set; }

        /// <summary>
        /// 性质
        /// </summary>
        public int XingZhiId { get; set; }

        /// <summary>
        /// 片区
        /// </summary>
        public int PianQuId { get; set; }

        /// <summary>
        /// 事业部
        /// </summary>
        public int ShiYeBuId { get; set; }

        /// <summary>
        /// 科目Id
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// 会计期间：1：本年，2：本月
        /// </summary>
        public int QiJianTypeId { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public double Data { get; set; }
    }
}

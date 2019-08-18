using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Response
{
    /// <summary>
    /// excel列表输出参数
    /// </summary>
    public class GetExcelListResponse
    {
        /// <summary>
        /// id
        /// </summary>
        public int ExcelRecordId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ExcelName { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string ExcelUrl { get; set; }
    }
}
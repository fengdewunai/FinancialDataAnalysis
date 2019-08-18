/*----------------------------------------------------
 * 作者:高峰
 * 创建时间:2019-08-16
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 高峰        2019-08-16      创建
 ----------------------------------------------------*/

using System;

namespace Model.DataModel
{
    /// <summary>
    /// excel导入记录
    /// </summary>
    public class ExcelRecordModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ExcelRecordId { get; set; }

        /// <summary>
        /// excel名称
        /// </summary>
        public string ExcelName { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// excel地址
        /// </summary>
        public string ExcelUrl { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int StatusFlag { get; set; }
    }
}

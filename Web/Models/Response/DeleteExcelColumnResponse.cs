using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Response
{
    /// <summary>
    /// 删除excel某列输出参数
    /// </summary>
    public class DeleteExcelColumnResponse : SuccessModel
    {
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FilePath { get; set; }
    }
}
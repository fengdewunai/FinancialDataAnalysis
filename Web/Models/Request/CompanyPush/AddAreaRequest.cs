using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Request.CompanyPush
{
    /// <summary>
    /// 新增地区输入参数
    /// </summary>
    public class AddAreaRequest
    {
        /// <summary>
		/// 地区名称
		/// </summary>
		public string AreaName { get; set; }
    }
}
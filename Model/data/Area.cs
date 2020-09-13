/*----------------------------------------------------
 * 作者:高峰
 * 创建时间:2020-09-12
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 高峰        2020-09-12      创建
 ----------------------------------------------------*/

using System;

namespace Model.data
{
    /// <summary>
    /// 地区信息
    /// </summary>
    public class Area 
    {
		/// <summary>
		/// 地区Id
		/// </summary>
		public int AreaId { get; set; }

		/// <summary>
		/// 地区名称
		/// </summary>
		public string AreaName { get; set; }

		/// <summary>
		/// 用户Id
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// 创建日期
		/// </summary>
		public DateTime CreateDateTime { get; set; }

		/// <summary>
		/// 更新日期
		/// </summary>
		public DateTime UpdateDateTime { get; set; }
    }
}
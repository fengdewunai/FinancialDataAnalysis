using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.data
{
    /// <summary>
    /// 公司联系记录
    /// </summary>
    public class CompanyConnectRecord
    {
        /// <summary>
		/// 记录Id
		/// </summary>
		public int CompanyConnectRecordId { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 电销联系日期
        /// </summary>
        public DateTime PhoneConnectDate { get; set; }

        /// <summary>
        /// 公司现状
        /// </summary>
        public int CompanyState { get; set; }

        /// <summary>
        /// 是否有效地址
        /// </summary>
        public int IsValidateAddress { get; set; }

        /// <summary>
        /// 确认地址
        /// </summary>
        public string ConfirmAddress { get; set; }

        /// <summary>
        /// 上门拜访人员
        /// </summary>
        public string GoHomePerson { get; set; }

        /// <summary>
        /// 拜访时间
        /// </summary>
        public DateTime GoHomeTime { get; set; }

        /// <summary>
        /// 电话联系情况
        /// </summary>
        public string PhoneConnectState { get; set; }

        /// <summary>
        /// 合作意向，1：无意向，2：有意向，3：待确认
        /// </summary>
        public int CooperationIntention { get; set; }

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

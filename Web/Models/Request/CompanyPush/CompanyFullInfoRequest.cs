using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Request
{
    public class CompanyFullInfoRequest
    {
        /// <summary>
        /// id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 地区Id
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 法人
        /// </summary>
        public string ArtificialPerson { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>
        public DateTime SetUpTime { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string CompanyPhone { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        public string Address { get; set; }

        /// <summary>
		/// 记录Id
		/// </summary>
		public int CompanyConnectRecordId { get; set; }

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
        /// 新增方式
        /// <see cref="AddCompanyTypeEnum"/>
        /// </summary>
        public int AddCompanyTypeId { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.data
{
    /// <summary>
    /// 公司信息
    /// </summary>
    public class Company
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
        /// 创建日期
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime UpdateDateTime { get; set; }
    }
}

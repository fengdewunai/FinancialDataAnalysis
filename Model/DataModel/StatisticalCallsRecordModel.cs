using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataModel
{
    /// <summary>
    /// action统计信息
    /// </summary>
    public class StatisticalCallsRecordModel
    {
        /// <summary>
        /// 统计id
        /// </summary>
        public int StatisticalCallsRecordId { get; set; }

        /// <summary>
        /// action名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// sessionId
        /// </summary>
        public string SessionId { get; set; }
    }
}

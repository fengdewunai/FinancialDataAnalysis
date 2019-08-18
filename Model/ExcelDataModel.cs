using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Excel中的数据
    /// </summary>
    public class ExcelDataModel
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<XiangMuForExcelDataModel> Datas { get; set; }
    }

    /// <summary>
    /// 项目信息
    /// </summary>
    public class XiangMuForExcelDataModel
    {
        /// <summary>
        /// 虚拟集团
        /// </summary>
        public int XuNiJiTuanId { get; set; }

        /// <summary>
        /// 虚拟集团名称
        /// </summary>
        public string XuNiJiTuanName { get; set; }

        /// <summary>
        /// 事业部
        /// </summary>
        public int ShiYeBuId { get; set; }

        /// <summary>
        /// 事业部名称
        /// </summary>
        public string ShiYeBuName { get; set; }

        /// <summary>
        /// 片区
        /// </summary>
        public int PianQuId { get; set; }

        /// <summary>
        /// 片区名称
        /// </summary>
        public string PianQuName { get; set; }

        /// <summary>
        /// 性质
        /// </summary>
        public int XingZhiId { get; set; }

        /// <summary>
        /// 性质名称
        /// </summary>
        public string XingZhiName { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public int XiangMuId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string XiangMuName { get; set; }

        /// <summary>
        /// 会计期间：1：本年，2：本月
        /// </summary>
        public int QiJianTypeId { get; set; }

        /// <summary>
        /// 会计期间名称
        /// </summary>
        public string QiJianTypeName { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<DataDetailForExcelDataModel> Datas { get; set; }
    }

    /// <summary>
    /// 科目信息
    /// </summary>
    public class DataDetailForExcelDataModel
    {
        /// <summary>
        /// 科目编号
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public double Data { get; set; }
    }
}

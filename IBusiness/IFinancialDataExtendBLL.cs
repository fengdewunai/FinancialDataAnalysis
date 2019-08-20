using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.DataModel;

namespace IBusiness
{
    /// <summary>
    /// 数据扩展方法
    /// </summary>
    public interface IFinancialDataExtendBLL
    {
        /// <summary>
        /// 获取表头数据
        /// </summary>
        /// <param name="excelId"></param>
        /// <param name="financialDataItemIds"></param>
        /// <param name="xiangMuTreeTypeId"></param>
        /// <param name="onlyStatisticChildren"></param>
        /// <returns></returns>
        GridColumnsModel GetGridColumns(int excelId, string financialDataItemIds, int onlyStatisticChildren,
            int xiangMuTreeTypeId);

        /// <summary>
        /// 获取grid的数据
        /// </summary>
        /// <param name="excelId"></param>
        /// <param name="accountItemIds">科目</param>
        /// <param name="financialDataItemIds">项目</param>
        /// <param name="qiJianTypeId">期间类型</param>
        /// <param name="onlyStatisticChildren">是否只统计下级项目</param>
        /// <returns></returns>
        DynamicGridDataModel GetGridData(int excelId, string accountItemIds, string financialDataItemIds,
            int qiJianTypeId, int onlyStatisticChildren, int xiangMuTreeTypeId);
    }
}

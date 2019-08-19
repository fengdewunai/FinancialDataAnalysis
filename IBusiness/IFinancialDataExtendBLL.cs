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
        /// <returns></returns>
        GridColumnsModel GetGridColumns(int excelId, string financialDataItemIds);

        /// <summary>
        /// 获取grid的数据
        /// </summary>
        /// <param name="excelId"></param>
        /// <param name="accountItemIds"></param>
        /// <param name="financialDataItemIds"></param>
        /// <param name="qiJianTypeId"></param>
        /// <returns></returns>
        DynamicGridDataModel GetGridData(int excelId, string accountItemIds, string financialDataItemIds, int qiJianTypeId);
    }
}

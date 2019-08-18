using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DataModel;

namespace IBusiness
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public interface IFinancialDataBLL
    {
        /// <summary>
        /// 根据excelRecordId获取所有科目
        /// </summary>
        /// <returns></returns>
        List<AccountItemModel> GetAccountByExcelRecordId(int excelRecordId);

        /// <summary>
        /// BatchInsertAccount
        /// </summary>
        /// <param name="dataModels"></param>
        void BatchInsertAccount(List<AccountItemModel> dataModels);

        /// <summary>
        /// 获取所有导入的excel
        /// </summary>
        /// <returns></returns>
        List<ExcelRecordModel> GetAllExcelRecord();

        /// <summary>
        /// SaveExcelRecord
        /// </summary>
        /// <returns></returns>
        int SaveExcelRecord(ExcelRecordModel model);

        /// <summary>
        /// GetFinancialDataItemByExcelRecordId
        /// </summary>
        /// <param name="excelRecordId"></param>
        /// <returns></returns>
        List<FinancialDataItemModel> GetFinancialDataItemByExcelRecordId(int excelRecordId);

        /// <summary>
        /// BatchInsertFinancialDataItem
        /// </summary>
        /// <param name="dataModels"></param>
        void BatchInsertFinancialDataItem(List<FinancialDataItemModel> dataModels);

        /// <summary>
        /// GetFinancialDataByFilter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<FinancialDataModel> GetFinancialDataByFilter(FinancialDataModel filter);

        /// <summary>
        /// BatchInsertFinancialData
        /// </summary>
        /// <param name="dataModels"></param>
        void BatchInsertFinancialData(List<FinancialDataModel> dataModels);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.DataModel;
using Model.Response;

namespace IDataAccess
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public interface IFinancialDataDAL
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
        /// 根据excelRecordId删除所有科目
        /// </summary>
        /// <param name="excelRecordId"></param>
        void DeleteAccountByExcelRecordId(int excelRecordId);

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
        /// DeleteExcelRecord
        /// </summary>
        /// <returns></returns>
        void DeleteExcelRecord(int excelRecordId);

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
        /// DeleteFinancialDataItemByExcelRecordId
        /// </summary>
        /// <param name="excelRecordId"></param>
        /// <returns></returns>
        void DeleteFinancialDataItemByExcelRecordId(int excelRecordId);

        /// <summary>
        /// GetFinancialDataByFilter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<FinancialDataModel> GetFinancialDataByFilter(FinancialDataModel filter);

        /// <summary>
        /// GetFinancialDataByPaging
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<GetFinancialDataByPagingResponse> GetFinancialDataByPaging(GetFinancialDataByPagingRequest request, out int totalCount);

        /// <summary>
        /// BatchInsertFinancialData
        /// </summary>
        /// <param name="dataModels"></param>
        void BatchInsertFinancialData(List<FinancialDataModel> dataModels);

        /// <summary>
        /// DeleteFinancialData
        /// </summary>
        /// <param name="excelRecordId"></param>
        void DeleteFinancialData(int excelRecordId);
    }
}

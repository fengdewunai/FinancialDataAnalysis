using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using IDataAccess;
using Model;
using Model.DataModel;
using Model.Response;

namespace DataAcess
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public class FinancialDataDAL : DataAccessBase, IFinancialDataDAL
    {
        /// <summary>
        /// 根据excelRecordId获取所有科目
        /// </summary>
        /// <returns></returns>
        public List<AccountItemModel> GetAccountByExcelRecordId(int excelRecordId)
        {
            return CurrentConnectStringContext.StoredProcedure("AccountItem_GetByExcelRecordId")
                .Parameter("v_ExcelRecordId", excelRecordId)
                .QueryMany<AccountItemModel>();
        }

        /// <summary>
        /// BatchInsertAccount
        /// </summary>
        /// <param name="dataModels"></param>
        public void BatchInsertAccount(List<AccountItemModel> dataModels)
        {
            var dataTable = ListToDataTable(dataModels);
            dataTable.TableName = "AccountItem";
            BulkInsert(dataTable);
        }

        /// <summary>
        /// 根据excelRecordId删除所有科目
        /// </summary>
        /// <param name="excelRecordId"></param>
        public void DeleteAccountByExcelRecordId(int excelRecordId)
        {
            CurrentConnectStringContext.StoredProcedure("AccountItem_DeleteByExcelRecordId")
                .Parameter("v_ExcelRecordId", excelRecordId)
                .Execute();
        }

        /// <summary>
        /// 获取所有导入的excel
        /// </summary>
        /// <returns></returns>
        public List<ExcelRecordModel> GetAllExcelRecord()
        {
            return CurrentConnectStringContext.StoredProcedure("ExcelRecord_Get")
                .QueryMany<ExcelRecordModel>();
        }

        /// <summary>
        /// SaveExcelRecord
        /// </summary>
        /// <returns></returns>
        public int SaveExcelRecord(ExcelRecordModel model)
        {
            return CurrentConnectStringContext.StoredProcedure("Excelrecord_Save")
                .Parameter("v_ExcelRecordId", model.ExcelRecordId)
                .Parameter("v_ExcelName", model.ExcelName)
                .Parameter("v_CreateDateTime", model.CreateDateTime)
                .Parameter("v_ExcelUrl", model.ExcelUrl)
                .Parameter("v_StatusFlag", model.StatusFlag)
                .QuerySingle<int>();
        }

        /// <summary>
        /// DeleteExcelRecord
        /// </summary>
        /// <returns></returns>
        public void DeleteExcelRecord(int excelRecordId)
        {
            CurrentConnectStringContext.StoredProcedure("ExcelRecord_DeleteById")
                .Parameter("v_ExcelRecordId", excelRecordId)
                .Execute();
        }

        /// <summary>
        /// GetFinancialDataItemByExcelRecordId
        /// </summary>
        /// <param name="excelRecordId"></param>
        /// <returns></returns>
        public List<FinancialDataItemModel> GetFinancialDataItemByExcelRecordId(int excelRecordId)
        {
            return CurrentConnectStringContext.StoredProcedure("FinancialDataItem_GetByExcelRecordId")
                .Parameter("v_ExcelRecordId", excelRecordId)
                .QueryMany<FinancialDataItemModel>();
        }

        /// <summary>
        /// BatchInsertAccount
        /// </summary>
        /// <param name="dataModels"></param>
        public void BatchInsertFinancialDataItem(List<FinancialDataItemModel> dataModels)
        {
            var dataTable = ListToDataTable(dataModels);
            dataTable.TableName = "FinancialDataItem";
            BulkInsert(dataTable);
        }

        /// <summary>
        /// DeleteFinancialDataItemByExcelRecordId
        /// </summary>
        /// <param name="excelRecordId"></param>
        /// <returns></returns>
        public void DeleteFinancialDataItemByExcelRecordId(int excelRecordId)
        {
            CurrentConnectStringContext.StoredProcedure("FinancialDataItem_DeleteByExcelRecordId")
                .Parameter("v_ExcelRecordId", excelRecordId)
                .Execute();
        }

        /// <summary>
        /// GetFinancialDataByFilter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<FinancialDataModel> GetFinancialDataByFilter(FinancialDataModel filter)
        {
            return CurrentConnectStringContext.StoredProcedure("FinancialData_GetByFilter")
                .Parameter("v_ExcelRecordId", filter.ExcelRecordId)
                .Parameter("v_XiangMuIdId", filter.XiangMuIdId)
                .Parameter("v_XingZhiId", filter.XingZhiId)
                .Parameter("v_PianQuId", filter.PianQuId)
                .Parameter("v_ShiYeBuId", filter.ShiYeBuId)
                .Parameter("v_AccountCode", filter.AccountCode)
                .Parameter("v_QiJianTypeId", filter.QiJianTypeId)
                .QueryMany<FinancialDataModel>();
        }

        /// <summary>
        /// GetFinancialDataByPaging
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<GetFinancialDataByPagingResponse> GetFinancialDataByPaging(GetFinancialDataByPagingRequest request, out int totalCount)
        {
            var store = CurrentConnectStringContext.StoredProcedure("FinancialData_GetDetailByPaging")
                .Parameter("v_ExcelRecordId", request.ExcelRecordId)
                .Parameter("v_AccountCode", request.AccountCode)
                .Parameter("v_QiJianTypeId", request.QiJianTypeId)
                .Parameter("v_XiangMuTypeId", request.XiangMuTypeId)
                .Parameter("v_XiangMuItemId", request.XiangMuItemId)
                .Parameter("v_PageIndex", request.Page)
                .Parameter("v_PageSize", request.Limit)
                .ParameterOut("v_TotalCount", DataTypes.Int32);
            var result = store.QueryMany<GetFinancialDataByPagingResponse>();
            totalCount = store.ParameterValue<int>("v_TotalCount");
            return result;
        }

        /// <summary>
        /// BatchInsertFinancialData
        /// </summary>
        /// <param name="dataModels"></param>
        public void BatchInsertFinancialData(List<FinancialDataModel> dataModels)
        {
            var dataTable = ListToDataTable(dataModels);
            dataTable.TableName = "FinancialData";
            BulkInsert(dataTable);
        }

        /// <summary>
        /// DeleteFinancialData
        /// </summary>
        /// <param name="excelRecordId"></param>
        public void DeleteFinancialData(int excelRecordId)
        {
            CurrentConnectStringContext.StoredProcedure("FinancialData_DeleteByExcelRecordId")
                .Parameter("v_ExcelRecordId", excelRecordId)
                .Execute();
        }
    }
}

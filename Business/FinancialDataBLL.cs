using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBusiness;
using IDataAccess;
using Model.DataModel;

namespace Business
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public class FinancialDataBLL : IFinancialDataBLL
    {
        /// <summary>
        /// 获取数据数据层
        /// </summary>
        private IFinancialDataDAL _financialDataDal;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="financialDataDal"></param>
        public FinancialDataBLL(IFinancialDataDAL financialDataDal)
        {
            _financialDataDal = financialDataDal;
        }

        /// <summary>
        /// 根据excelRecordId获取所有科目
        /// </summary>
        /// <returns></returns>
        public List<AccountItemModel> GetAccountByExcelRecordId(int excelRecordId)
        {
            return _financialDataDal.GetAccountByExcelRecordId(excelRecordId);
        }

        /// <summary>
        /// BatchInsertAccount
        /// </summary>
        /// <param name="dataModels"></param>
        public void BatchInsertAccount(List<AccountItemModel> dataModels)
        {
            _financialDataDal.BatchInsertAccount(dataModels);
        }

        /// <summary>
        /// 获取所有导入的excel
        /// </summary>
        /// <returns></returns>
        public List<ExcelRecordModel> GetAllExcelRecord()
        {
            return _financialDataDal.GetAllExcelRecord();
        }

        /// <summary>
        /// SaveExcelRecord
        /// </summary>
        /// <returns></returns>
        public int SaveExcelRecord(ExcelRecordModel model)
        {
            return _financialDataDal.SaveExcelRecord(model);
        }

        /// <summary>
        /// GetFinancialDataItemByExcelRecordId
        /// </summary>
        /// <param name="excelRecordId"></param>
        /// <returns></returns>
        public List<FinancialDataItemModel> GetFinancialDataItemByExcelRecordId(int excelRecordId)
        {
            return _financialDataDal.GetFinancialDataItemByExcelRecordId(excelRecordId);
        }

        /// <summary>
        /// BatchInsertFinancialDataItem
        /// </summary>
        /// <param name="dataModels"></param>
        public void BatchInsertFinancialDataItem(List<FinancialDataItemModel> dataModels)
        {
            _financialDataDal.BatchInsertFinancialDataItem(dataModels);
        }

        /// <summary>
        /// GetFinancialDataByFilter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<FinancialDataModel> GetFinancialDataByFilter(FinancialDataModel filter)
        {
            return _financialDataDal.GetFinancialDataByFilter(filter);
        }

        /// <summary>
        /// BatchInsertFinancialData
        /// </summary>
        /// <param name="dataModels"></param>
        public void BatchInsertFinancialData(List<FinancialDataModel> dataModels)
        {
            _financialDataDal.BatchInsertFinancialData(dataModels);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBusiness;
using IDataAccess;
using Model;
using Model.DataModel;
using Model.Enum;
using Model.Response;

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
        /// 根据excelRecordId删除所有科目
        /// </summary>
        /// <param name="excelRecordId"></param>
        public void DeleteAccountByExcelRecordId(int excelRecordId)
        {
            _financialDataDal.DeleteAccountByExcelRecordId(excelRecordId);
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
        /// DeleteExcelRecord
        /// </summary>
        /// <returns></returns>
        public void DeleteExcelRecord(int excelRecordId)
        {
            _financialDataDal.DeleteExcelRecord(excelRecordId);
        }

        /// <summary>
        /// GetFinancialDataItemByExcelRecordId
        /// </summary>
        /// <param name="excelRecordId"></param>
        /// <returns></returns>
        public List<FinancialDataItemModel> GetFinancialDataItemByExcelRecordId(int excelRecordId, int xiangMuTreeTypeId = 0)
        {
            var result = _financialDataDal.GetFinancialDataItemByExcelRecordId(excelRecordId);
            if (xiangMuTreeTypeId == 0)
            {
                return result;
            }
            if (xiangMuTreeTypeId == 1)
            {
                result = result.Where(x => x.ItemTypeId != (int)FinancialDataItemTypeEnum.XingZhi).ToList();
            }
            else
            {
                result = result.Where(x => x.ItemTypeId != (int)FinancialDataItemTypeEnum.PianQu).ToList();
            }
            return result;
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
        /// DeleteFinancialDataItemByExcelRecordId
        /// </summary>
        /// <param name="excelRecordId"></param>
        /// <returns></returns>
        public void DeleteFinancialDataItemByExcelRecordId(int excelRecordId)
        {
            _financialDataDal.DeleteFinancialDataItemByExcelRecordId(excelRecordId);
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
        /// GetFinancialDataByPaging
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<GetFinancialDataByPagingResponse> GetFinancialDataByPaging(GetFinancialDataByPagingRequest request,
            out int totalCount)
        {
            return _financialDataDal.GetFinancialDataByPaging(request, out totalCount);
        }

        /// <summary>
        /// BatchInsertFinancialData
        /// </summary>
        /// <param name="dataModels"></param>
        public void BatchInsertFinancialData(List<FinancialDataModel> dataModels)
        {
            _financialDataDal.BatchInsertFinancialData(dataModels);
        }

        /// <summary>
        /// DeleteFinancialData
        /// </summary>
        /// <param name="excelRecordId"></param>
        public void DeleteFinancialData(int excelRecordId)
        {
            _financialDataDal.DeleteFinancialData(excelRecordId);
        }
    }
}

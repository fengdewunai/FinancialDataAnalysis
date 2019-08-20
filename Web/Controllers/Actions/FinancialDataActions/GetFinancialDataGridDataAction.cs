using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using IBusiness;
using Model.DataModel;
using Model.Enum;
using Newtonsoft.Json.Linq;

namespace Web.Controllers.Actions.FinancialDataActions
{
    /// <summary>
    /// 获取grid数据
    /// </summary>
    public class GetFinancialDataGridDataAction
    {
        private IFinancialDataExtendBLL _financialDataExtendBLL;

        public GetFinancialDataGridDataAction(IFinancialDataExtendBLL financialDataExtendBll)
        {
            _financialDataExtendBLL = financialDataExtendBll;
        }

        /// <summary>
        /// 业务逻辑
        /// </summary>
        /// <param name="excelId"></param>
        /// <param name="accountItemIds"></param>
        /// <param name="financialDataItemIds"></param>
        /// <param name="qiJianTypeId"></param>
        /// <returns></returns>
        public DynamicGridDataModel Process(int excelId, string accountItemIds, string financialDataItemIds, int qiJianTypeId, int onlyStatisticChildren, int xiangMuTreeTypeId)
        {
            return _financialDataExtendBLL.GetGridData(excelId, accountItemIds, financialDataItemIds, qiJianTypeId, onlyStatisticChildren, xiangMuTreeTypeId);
        }

        
    }
}
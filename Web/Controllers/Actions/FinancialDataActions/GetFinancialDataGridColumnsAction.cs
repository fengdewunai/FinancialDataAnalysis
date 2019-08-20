using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBusiness;
using Model;

namespace Web.Controllers.Actions.FinancialDataActions
{
    /// <summary>
    /// 获取数据表头数据
    /// </summary>
    public class GetFinancialDataGridColumnsAction
    {
        private IFinancialDataExtendBLL _financialDataExtendBLL;

        public GetFinancialDataGridColumnsAction(IFinancialDataExtendBLL financialDataExtendBll)
        {
            _financialDataExtendBLL = financialDataExtendBll;
        }

        public GridColumnsModel Process(int excelId, string financialDataItemIds, int onlyStatisticChildren)
        {
            return _financialDataExtendBLL.GetGridColumns(excelId, financialDataItemIds, onlyStatisticChildren);
        }
    }
}
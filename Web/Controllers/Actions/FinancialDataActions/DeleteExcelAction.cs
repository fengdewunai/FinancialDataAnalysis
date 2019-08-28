using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBusiness;
using Model;

namespace Web.Controllers.Actions.FinancialDataActions
{
    /// <summary>
    /// 删除excel
    /// </summary>
    public class DeleteExcelAction
    {
        private IFinancialDataBLL _financialDataBll;

        public DeleteExcelAction(IFinancialDataBLL financialDataBll)
        {
            _financialDataBll = financialDataBll;
        }

        public void Process(string excelIds)
        {
            var excelIdList = excelIds.Split(',').Select(x=>Convert.ToInt32(x)).ToList();
            foreach (var excelId in excelIdList)
            {
                _financialDataBll.DeleteExcelRecord(excelId);
                _financialDataBll.DeleteAccountByExcelRecordId(excelId);
                _financialDataBll.DeleteFinancialDataItemByExcelRecordId(excelId);
                _financialDataBll.DeleteFinancialData(excelId);
            }
        }
    }
}
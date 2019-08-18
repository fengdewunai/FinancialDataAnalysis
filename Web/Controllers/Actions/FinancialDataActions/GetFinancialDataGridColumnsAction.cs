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
        private IFinancialDataBLL _financialDataBll;

        public GetFinancialDataGridColumnsAction(IFinancialDataBLL financialDataBll)
        {
            _financialDataBll = financialDataBll;
        }

        public GridColumnsModel Process(int excelId, string financialDataItemIds)
        {
            var result = new GridColumnsModel(){GridColumns = new List<GridColumnsDetail>(), StoreFields = new List<string>()};
            var financialDataItems = _financialDataBll.GetFinancialDataItemByExcelRecordId(excelId);
            var financialDataItemIdList = financialDataItemIds.Split(',').OrderBy(x => x).OrderBy(x => x).ToList();
            result.GridColumns.Add(new GridColumnsDetail() {text = "项目名称", dataIndex = "AccountName", width = 120});
            result.StoreFields.Add("AccountName");
            foreach (var financialDataItemId in financialDataItemIdList)
            {
                var financialDataItem = financialDataItems.FirstOrDefault(x => x.ItemId.ToString() == financialDataItemId);
                result.GridColumns.Add(new GridColumnsDetail()
                {
                    text = financialDataItem.ItemName.ToString(),
                    dataIndex = financialDataItem.ItemId.ToString(),
                    width = 120
                });
                result.StoreFields.Add(financialDataItem.ItemId.ToString());
            }

            return result;
        }
    }
}
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
        private IFinancialDataBLL _financialDataBll;

        public GetFinancialDataGridDataAction(IFinancialDataBLL financialDataBll)
        {
            _financialDataBll = financialDataBll;
        }

        public DynamicGridDataModel Process(int excelId, string accountItemIds, string financialDataItemIds)
        {
            var gridData = new DynamicGridDataModel(){rows = new List<Dictionary<string, string>>()};
            var datas = _financialDataBll.GetFinancialDataByFilter(new FinancialDataModel()
            {
                ExcelRecordId = excelId
            });
            datas = datas.Where(x => x.QiJianTypeId == 1).ToList();
            var accountItems = _financialDataBll.GetAccountByExcelRecordId(excelId);
            var financialDataItems = _financialDataBll.GetFinancialDataItemByExcelRecordId(excelId);
            var accountItemIdList = accountItemIds.Split(',').OrderBy(x => x).ToList();
            var financialDataItemIdList = financialDataItemIds.Split(',').OrderBy(x => x).ToList();
            gridData.total = accountItemIdList.Count;
            foreach (var accountItemId in accountItemIdList)
            {
                var dataDic = new Dictionary<string, string>();
                var accountItem = accountItems.First(x => x.AccountCode == accountItemId);
                var dataInAccountItem = datas.Where(x => x.AccountCode == accountItemId);
                dataDic.Add("AccountName", accountItem.AccountName);
                foreach (var financialDataItemId in financialDataItemIdList)
                {
                    var financialDataItem = financialDataItems.FirstOrDefault(x=>x.ItemId.ToString() == financialDataItemId);
                    var financialData = 0.0;
                    switch (financialDataItem.ItemTypeId)
                    {
                        case (int)FinancialDataItemTypeEnum.XuNiJiTuan:
                            var shiYeBuItem = financialDataItems.Where(x=>x.ParentId == financialDataItem.ItemId).ToList();
                            financialData = dataInAccountItem.Where(x=> shiYeBuItem.Exists(y=>y.ItemId == x.ShiYeBuId)).Sum(x=>x.Data);
                            break;
                        case (int)FinancialDataItemTypeEnum.ShiYeBu:
                            financialData = dataInAccountItem.Where(x => x.ShiYeBuId == financialDataItem.ItemId).Sum(x => x.Data);
                            break;
                        case (int)FinancialDataItemTypeEnum.PianQu:
                            financialData = dataInAccountItem.Where(x => x.PianQuId == financialDataItem.ItemId).Sum(x => x.Data);
                            break;
                        case (int)FinancialDataItemTypeEnum.XingZhi:
                            financialData = dataInAccountItem.Where(x => x.XingZhiId == financialDataItem.ItemId).Sum(x => x.Data);
                            break;
                        case (int)FinancialDataItemTypeEnum.XiangMu:
                            financialData = dataInAccountItem.Where(x => x.XiangMuIdId == financialDataItem.ItemId).Sum(x => x.Data);
                            break;
                    }
                    dataDic.Add(financialDataItem.ItemId.ToString(), financialData.ToString());
                }
                gridData.rows.Add(dataDic);
            }
            return gridData;
        }
    }
}
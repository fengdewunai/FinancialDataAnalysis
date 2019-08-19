using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBusiness;
using Model;
using Model.DataModel;
using Model.Enum;

namespace Business
{
    /// <summary>
    /// 数据扩展方法
    /// </summary>
    public class FinancialDataExtendBLL : IFinancialDataExtendBLL
    {
        /// <summary>
        /// 获取数据数据层
        /// </summary>
        private IFinancialDataBLL _financialDataBll;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="financialDataBll"></param>
        public FinancialDataExtendBLL(IFinancialDataBLL financialDataBll)
        {
            _financialDataBll = financialDataBll;
        }

        /// <summary>
        /// 获取表头数据
        /// </summary>
        /// <param name="excelId"></param>
        /// <param name="financialDataItemIds"></param>
        /// <returns></returns>
        public GridColumnsModel GetGridColumns(int excelId, string financialDataItemIds)
        {
            var result = new GridColumnsModel() { GridColumns = new List<GridColumnsDetail>(), StoreFields = new List<string>() };
            var financialDataItems = _financialDataBll.GetFinancialDataItemByExcelRecordId(excelId);
            var financialDataItemIdList = financialDataItemIds.Split(',').OrderBy(x => x).OrderBy(x => x).ToList();
            result.GridColumns.Add(new GridColumnsDetail() { text = "项目名称", dataIndex = "AccountName", width = 120 });
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

        /// <summary>
        /// 获取grid的数据
        /// </summary>
        /// <param name="excelId"></param>
        /// <param name="accountItemIds"></param>
        /// <param name="financialDataItemIds"></param>
        /// <param name="qiJianTypeId"></param>
        /// <returns></returns>
        public DynamicGridDataModel GetGridData(int excelId, string accountItemIds, string financialDataItemIds, int qiJianTypeId)
        {
            var gridData = new DynamicGridDataModel() { rows = new List<Dictionary<string, string>>() };
            var shouRuDatas = new List<Dictionary<string, string>>();
            var zhiChuDatas = new List<Dictionary<string, string>>();
            var datas = _financialDataBll.GetFinancialDataByFilter(new FinancialDataModel()
            {
                ExcelRecordId = excelId
            });
            datas = datas.Where(x => x.QiJianTypeId == qiJianTypeId).ToList();
            var accountItems = _financialDataBll.GetAccountByExcelRecordId(excelId);
            var financialDataItems = _financialDataBll.GetFinancialDataItemByExcelRecordId(excelId);
            var accountItemIdList = accountItemIds.Split(',').OrderBy(x => x).ToList();
            var financialDataItemIdList = financialDataItemIds.Split(',').OrderBy(x => x).ToList();
            gridData.total = accountItemIdList.Count;
            foreach (var accountItemId in accountItemIdList)
            {
                var dataDic = new Dictionary<string, string>();
                var accountItem = accountItems.First(x => x.AccountCode == accountItemId);
                var dataInAccountItem = datas.Where(x => x.AccountCode == accountItemId).ToList();
                dataDic.Add("AccountName", accountItem.AccountName);
                foreach (var financialDataItemId in financialDataItemIdList)
                {
                    var financialDataItem = financialDataItems.FirstOrDefault(x => x.ItemId.ToString() == financialDataItemId);
                    var financialData = GetFinancialDataForAccountItem(financialDataItem, financialDataItems, dataInAccountItem);
                    dataDic.Add(financialDataItem.ItemId.ToString(), financialData.ToString());
                }
                if (accountItem.AccountTypeId == 1)
                {
                    shouRuDatas.Add(dataDic);
                }
                else
                {
                    zhiChuDatas.Add(dataDic);
                }
            }
            GetSumResultWithAccountItemType(gridData.rows, shouRuDatas, zhiChuDatas, accountItems);
            return gridData;
        }

        /// <summary>
        /// 汇总传入的科目下的数据
        /// </summary>
        /// <param name="financialDataItem"></param>
        /// <param name="financialDataItems"></param>
        /// <param name="dataInAccountItem"></param>
        /// <returns></returns>
        private double GetFinancialDataForAccountItem(FinancialDataItemModel financialDataItem, List<FinancialDataItemModel> financialDataItems, List<FinancialDataModel> dataInAccountItem)
        {
            var financialData = 0.0;
            switch (financialDataItem.ItemTypeId)
            {
                case (int)FinancialDataItemTypeEnum.XuNiJiTuan:
                    var shiYeBuItem = financialDataItems.Where(x => x.ParentId == financialDataItem.ItemId).ToList();
                    financialData = dataInAccountItem.Where(x => shiYeBuItem.Exists(y => y.ItemId == x.ShiYeBuId)).Sum(x => x.Data);
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
            return financialData;
        }

        /// <summary>
        /// 统计收入和支出小计
        /// </summary>
        /// <param name="result"></param>
        /// <param name="shouRuDatas"></param>
        /// <param name="zhiChuDatas"></param>
        /// <param name="accountItems"></param>
        private void GetSumResultWithAccountItemType(List<Dictionary<string, string>> result, List<Dictionary<string, string>> shouRuDatas, List<Dictionary<string, string>> zhiChuDatas, List<AccountItemModel> accountItems)
        {
            var shouRuSumDic = new Dictionary<string, string>();
            if (shouRuDatas.Count > 0)
            {
                result.AddRange(shouRuDatas);
                shouRuSumDic.Add("AccountName", "小计");
                foreach (var shouRuDataDic in shouRuDatas)
                {
                    BuildSumResult(accountItems, shouRuDataDic, shouRuSumDic);
                }
                result.Add(shouRuSumDic);
            }
            var zhiChuSumDic = new Dictionary<string, string>();
            if (zhiChuDatas.Count > 0)
            {
                result.AddRange(zhiChuDatas);
                zhiChuSumDic.Add("AccountName", "小计");
                foreach (var zhiChuDataDic in zhiChuDatas)
                {
                    BuildSumResult(accountItems, zhiChuDataDic, zhiChuSumDic);
                }
                result.Add(zhiChuSumDic);
            }
            var resultSumDic = new Dictionary<string, string>();
            resultSumDic.Add("AccountName", "收支盈亏");
            if (shouRuSumDic.Count > 0)
            {
                foreach (var shouRuData in shouRuSumDic)
                {
                    if (shouRuData.Key == "AccountName")
                    {
                        continue;
                    }
                    resultSumDic.Add(shouRuData.Key, shouRuData.Value);
                }
            }
            if (zhiChuSumDic.Count > 0)
            {
                foreach (var zhiChuData in zhiChuSumDic)
                {
                    if (zhiChuData.Key == "AccountName")
                    {
                        continue;
                    }

                    if (resultSumDic.ContainsKey(zhiChuData.Key))
                    {
                        resultSumDic[zhiChuData.Key] = (Convert.ToDouble(resultSumDic[zhiChuData.Key]) - Convert.ToDouble(zhiChuData.Value)).ToString();
                    }
                    else
                    {
                        resultSumDic.Add(zhiChuData.Key, zhiChuData.Value);
                    }
                }
            }
            result.Add(resultSumDic);
        }

        /// <summary>
        ///  对数据进行汇总
        /// </summary>
        /// <param name="accountItems"></param>
        /// <param name="originDatas"></param>
        /// <param name="resultDic"></param>
        private void BuildSumResult(List<AccountItemModel> accountItems, Dictionary<string, string> originDatas, Dictionary<string, string> resultDic)
        {
            var accountItem = accountItems.FirstOrDefault(x => x.AccountCode == originDatas["AccountName"]);
            foreach (var dicData in originDatas)
            {
                if (dicData.Key == "AccountName")
                {
                    continue;
                }
                if (resultDic.ContainsKey(dicData.Key))
                {
                    resultDic.Add(dicData.Key, dicData.Value);
                }
                else
                {
                    var sumValue = accountItem.AccountSumTypeId == 1 ? Convert.ToDouble(resultDic[dicData.Key]) + Convert.ToDouble(dicData.Value) : Convert.ToDouble(resultDic[dicData.Key]) - Convert.ToDouble(dicData.Value);
                    resultDic[dicData.Key] = sumValue.ToString();
                }
            }
        }
    }
}

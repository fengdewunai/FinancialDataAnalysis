using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Aspose.Cells;
using IBusiness;
using Model;
using Model.DataModel;
using Model.Enum;
using RuanYun.Excel.Reader;
using Web.Models;

namespace Web.Controllers.Actions.FinancialDataActions
{
    /// <summary>
    /// 上传Excel
    /// </summary>
    public class UpladExcelAction
    {
        private IFinancialDataBLL _financialDataBll;

        public UpladExcelAction(IFinancialDataBLL financialDataBll)
        {
            _financialDataBll = financialDataBll;
        }

        public SuccessModel Process(string excelName)
        {
            var result = new SuccessModel();
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                return result;
            }
            var file = HttpContext.Current.Request.Files[0];
            var excel = new Aspose.Cells.Workbook(file.InputStream);
            var sheet = excel.Worksheets[0];
            var excelId = SaveExcel(excel, excelName);
            ProcessAccountItem(sheet, excelId);
            ProcessDatas(sheet, excelId);
            return new SuccessModel(){success = true};
        }

        /// <summary>
        /// 处理项目数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="excelId"></param>
        private void ProcessDatas(Worksheet sheet, int excelId)
        {
            var excelDatas = new ExcelDataModel(){Datas = new List<XiangMuForExcelDataModel>()};
            for (int i = 3; i < sheet.Cells.MaxDataColumn + 200; i++)
            {
                if (string.IsNullOrEmpty(GetCellValue(sheet, 6, i)))
                {
                    break;
                }
                var xiangMuForExcelDataModel = new XiangMuForExcelDataModel()
                {
                    ShiYeBuName = GetCellValue(sheet, 2, i),
                    PianQuName = GetCellValue(sheet, 1, i),
                    XingZhiName = string.IsNullOrEmpty(GetCellValue(sheet, 3, i)) ? GetCellValue(sheet, 4, i) : GetCellValue(sheet, 3, i),
                    XiangMuName = GetCellValue(sheet, 6, i),
                    QiJianTypeName = GetCellValue(sheet, 5, i),
                    Datas = new List<DataDetailForExcelDataModel>()
                };
                for (int j = 8; j < sheet.Cells.MaxDataRow + 200; j++)
                {
                    if(string.IsNullOrEmpty(GetCellValue(sheet, j, 0)))
                    {
                        break;
                    }
                    var dataDetailForExcelDataModel = new DataDetailForExcelDataModel()
                    {
                        AccountCode = GetCellValue(sheet, j, 0),
                        Data = string.IsNullOrEmpty(GetCellValue(sheet, j, i)) ? 0.0 : Convert.ToDouble(GetCellValue(sheet, j, i))
                    };
                    xiangMuForExcelDataModel.Datas.Add(dataDetailForExcelDataModel);
                }
                excelDatas.Datas.Add(xiangMuForExcelDataModel);
            }
            ProcessXiangMu(excelDatas.Datas, excelId);
            var financialDataModels = new List<FinancialDataModel>();
            foreach (var excelXiangMuData in excelDatas.Datas)
            {
                foreach (var detailData in excelXiangMuData.Datas)
                {
                    var financialDataModel = new FinancialDataModel()
                    {
                        ExcelRecordId = excelId,
                        XiangMuIdId = excelXiangMuData.XiangMuId,
                        XingZhiId = excelXiangMuData.XingZhiId,
                        PianQuId = excelXiangMuData.PianQuId,
                        ShiYeBuId = excelXiangMuData.ShiYeBuId,
                        AccountCode = detailData.AccountCode,
                        QiJianTypeId = excelXiangMuData.QiJianTypeName.Contains("年") ? 1 : 2,
                        Data = detailData.Data
                    };
                    financialDataModels.Add(financialDataModel);
                }
            }
            _financialDataBll.BatchInsertFinancialData(financialDataModels);
        }

        /// <summary>
        /// 处理项目分类
        /// </summary>
        /// <param name="items"></param>
        public void ProcessXiangMu(List<XiangMuForExcelDataModel> items, int excelId)
        {
            var id = 1;
            var financialDataItems = new List<FinancialDataItemModel>();
            var xuNiJiTuan = new FinancialDataItemModel()
            {
                ItemId = id++,
                ItemName = "集团职能",
                ItemLevel = 1,
                ItemTypeId = (int) FinancialDataItemTypeEnum.XuNiJiTuan
            };
            financialDataItems.Add(xuNiJiTuan);

            var emptyPianQuItems = items.Where(x => string.IsNullOrEmpty(x.PianQuName)).ToList();
            var notEmptyPianQuItems = items.Where(x => !string.IsNullOrEmpty(x.PianQuName)).ToList();
            foreach (var item in emptyPianQuItems)
            {
                if (string.IsNullOrEmpty(item.ShiYeBuName))
                {
                    item.ShiYeBuName = item.XiangMuName;
                }
            }

            // 事业部
            var emptyShiYeBuGroup = emptyPianQuItems.GroupBy(x => x.ShiYeBuName);
            FillShiYeBuInfo(emptyShiYeBuGroup, financialDataItems, ref id, xuNiJiTuan.ItemId, 2);
            var notEmptyShiYeBuGroup = notEmptyPianQuItems.GroupBy(x => x.ShiYeBuName);
            FillShiYeBuInfo(notEmptyShiYeBuGroup, financialDataItems, ref id, 0, 1);

            // 片区
            var pianQuGroup = notEmptyPianQuItems.GroupBy(x => x.PianQuName);
            foreach (var group in pianQuGroup)
            {
                var datasInPianQu = group.ToList();
                var pianQuModel = new FinancialDataItemModel()
                {
                    ItemId = id++,
                    ItemName = datasInPianQu[0].PianQuName,
                    ParentId = datasInPianQu[0].ShiYeBuId,
                    ItemLevel = 2,
                    ItemTypeId = (int)FinancialDataItemTypeEnum.PianQu
                };
                financialDataItems.Add(pianQuModel);
                foreach (var item in datasInPianQu)
                {
                    item.PianQuId = pianQuModel.ItemId;
                }
            }

            //性质
            var emptyXingZhiGroup = emptyPianQuItems.GroupBy(x => x.XingZhiName);
            foreach (var group in emptyXingZhiGroup)
            {
                var datasInXingZhi = group.ToList();
                var xingZhiModel = new FinancialDataItemModel()
                {
                    ItemId = id++,
                    ItemName = datasInXingZhi[0].XingZhiName,
                    ParentId = datasInXingZhi[0].ShiYeBuId,
                    ItemLevel = 3,
                    ItemTypeId = (int)FinancialDataItemTypeEnum.XingZhi
                };
                financialDataItems.Add(xingZhiModel);
                foreach (var item in datasInXingZhi)
                {
                    item.XingZhiId = xingZhiModel.ItemId;
                }
            }
            var notEmptyXingZhiGroup = notEmptyPianQuItems.GroupBy(x => x.XingZhiName);
            foreach (var group in notEmptyXingZhiGroup)
            {
                var datasInXingZhi = group.ToList();
                var xingZhiModel = new FinancialDataItemModel()
                {
                    ItemId = id++,
                    ItemName = datasInXingZhi[0].XingZhiName,
                    ParentId = datasInXingZhi[0].ShiYeBuId,
                    ItemLevel = 2,
                    ItemTypeId = (int)FinancialDataItemTypeEnum.XingZhi
                };
                financialDataItems.Add(xingZhiModel);
                foreach (var item in datasInXingZhi)
                {
                    item.XingZhiId = xingZhiModel.ItemId;
                }
            }

            //项目
            var emptyXiangMuGroup = emptyPianQuItems.GroupBy(x => x.XiangMuName);
            foreach (var group in emptyXiangMuGroup)
            {
                var datasInXiangMu = group.ToList();
                var xiangMuModel = new FinancialDataItemModel()
                {
                    ItemId = id++,
                    ItemName = datasInXiangMu[0].XiangMuName,
                    ParentId = datasInXiangMu[0].ShiYeBuId,
                    XingZhiId = datasInXiangMu[0].XingZhiId,
                    ItemLevel = 3,
                    ItemTypeId = (int)FinancialDataItemTypeEnum.XiangMu,
                    IsLeaf = 1
                };
                financialDataItems.Add(xiangMuModel);
                foreach (var item in datasInXiangMu)
                {
                    item.XiangMuId = xiangMuModel.ItemId;
                }
            }
            var notEmptyXiangMuGroup = notEmptyPianQuItems.GroupBy(x => x.XiangMuName);
            foreach (var group in notEmptyXiangMuGroup)
            {
                var datasInXiangMu = group.ToList();
                var xiangMuModel = new FinancialDataItemModel()
                {
                    ItemId = id++,
                    ItemName = datasInXiangMu[0].XiangMuName,
                    ParentId = datasInXiangMu[0].PianQuId,
                    XingZhiId = datasInXiangMu[0].XingZhiId,
                    ItemLevel = 3,
                    ItemTypeId = (int)FinancialDataItemTypeEnum.XiangMu,
                    IsLeaf = 1
                };
                financialDataItems.Add(xiangMuModel);
                foreach (var item in datasInXiangMu)
                {
                    item.XiangMuId = xiangMuModel.ItemId;
                }
            }

            foreach (var item in financialDataItems)
            {
                item.ExcelRecordId = excelId;
            }
            _financialDataBll.BatchInsertFinancialDataItem(financialDataItems);
        }

        /// <summary>
        /// 填充事业部信息
        /// </summary>
        /// <param name="xiangMuGroup"></param>
        /// <param name="financialDataItems"></param>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        /// <param name="itemLevel"></param>
        public void FillShiYeBuInfo(IEnumerable<IGrouping<string, XiangMuForExcelDataModel>> xiangMuGroup, List<FinancialDataItemModel> financialDataItems,
           ref int id, int parentId, int itemLevel)
        {
            foreach (var group in xiangMuGroup)
            {
                var datasInShiYeBu = group.ToList();
                var shiYeBuModel = new FinancialDataItemModel()
                {
                    ItemId = id++,
                    ItemName = datasInShiYeBu[0].ShiYeBuName,
                    ParentId = parentId,
                    ItemLevel = itemLevel,
                    ItemTypeId = (int)FinancialDataItemTypeEnum.ShiYeBu
                };
                financialDataItems.Add(shiYeBuModel);
                foreach (var item in datasInShiYeBu)
                {
                    item.ShiYeBuId = shiYeBuModel.ItemId;
                }
            }
        }

        /// <summary>
        /// 处理科目
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="excelId"></param>
        public void ProcessAccountItem(Worksheet sheet, int excelId)
        {
            var items = new List<AccountItemModel>();
            for (int i = 8; i < sheet.Cells.MaxDataRow + 200; i++)
            {
                if (string.IsNullOrEmpty(GetCellValue(sheet, i, 1)))
                {
                    break;
                }
                items.Add(new AccountItemModel()
                {
                    ExcelRecordId = excelId,
                    AccountCode = GetCellValue(sheet,i,0),
                    AccountLevel = Convert.ToInt32(GetCellValue(sheet, i, 1)),
                    AccountName = GetCellValue(sheet, i, 2),
                });
            }
            var firstLevelItems = items.Where(x => x.AccountLevel == 1).ToList();
            foreach (var firstLevelItem in firstLevelItems)
            {
                firstLevelItem.ParentCode = "";
                ProcessAccountItemParent(firstLevelItem, items);
            }
            _financialDataBll.BatchInsertAccount(items);
        }

        /// <summary>
        /// 为科目设置上下级关系
        /// </summary>
        /// <param name="accountItem"></param>
        /// <param name="allItems"></param>
        public void ProcessAccountItemParent(AccountItemModel accountItem, List<AccountItemModel> allItems)
        {
            var children = allItems.Where(x=>x.AccountLevel == accountItem.AccountLevel + 1 && x.AccountCode.Contains(accountItem.AccountCode)).ToList();
            if (children.Count > 0)
            {
                foreach (var item in children)
                {
                    item.ParentCode = accountItem.AccountCode;
                    ProcessAccountItemParent(item, allItems);
                }
            }
            else
            {
                accountItem.IsLeaf = 1;
            }
            accountItem.AccountTypeIdl = Convert.ToInt32(accountItem.AccountCode.Substring(0, 4)) < 6401 ? 1 : 2;
            accountItem.AccountSumTypeId = accountItem.AccountCode == "6813" ? 2 : 1;
        }

        /// <summary>
        /// 保存上传记录
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="excelName"></param>
        /// <returns></returns>
        public int SaveExcel(Workbook excel, string excelName)
        {
            var directory = HttpContext.Current.Request.PhysicalApplicationPath;
            var relativePath = string.Concat("\\Files\\FinancialExcel\\", excelName, "_", Guid.NewGuid(), ".xlsx");
            var filePath = string.Concat(directory, relativePath);
            excel.Save(filePath,SaveFormat.Xlsx);
            var excelRecordModel = new ExcelRecordModel()
            {
                ExcelName = excelName,
                ExcelUrl = relativePath,
                CreateDateTime = DateTime.Now,
                StatusFlag = 1
            };
            var excelId = _financialDataBll.SaveExcelRecord(excelRecordModel);
            return excelId;
        }

        /// <summary>
        /// 获取单元格数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private string GetCellValue(Worksheet sheet, int rowIndex, int columnIndex)
        {
            var value = sheet.Cells[rowIndex, columnIndex].Value;
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString();
        }
    }
}
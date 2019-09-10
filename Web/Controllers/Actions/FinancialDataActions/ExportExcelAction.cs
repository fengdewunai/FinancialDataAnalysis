using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Aspose.Cells;
using Common;
using IBusiness;
using Model;
using RuanYun.Excel;

namespace Web.Controllers.Actions.FinancialDataActions
{
    public class ExportExcelAction
    {
        private IFinancialDataBLL _financialDataBll;

        private IFinancialDataExtendBLL _financialDataExtendBLL;

        public ExportExcelAction(IFinancialDataBLL financialDataBll, IFinancialDataExtendBLL financialDataExtendBll)
        {
            _financialDataBll = financialDataBll;
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
        public Stream Process(int excelId, string accountItemIds, string financialDataItemIds, int qiJianTypeId, int onlyStatisticChildren, int xiangMuTreeTypeId, int statisticAccountChildren)
        {
            var result = new List<List<string>>();
            var columnData = _financialDataExtendBLL.GetGridColumns(excelId, financialDataItemIds, onlyStatisticChildren, xiangMuTreeTypeId);
            var gridData = _financialDataExtendBLL.GetGridData(excelId, accountItemIds, financialDataItemIds, qiJianTypeId, onlyStatisticChildren, xiangMuTreeTypeId, statisticAccountChildren);
            var columnList = new List<string>();
            foreach (var gridColumn in columnData.GridColumns)
            {
                columnList.Add(gridColumn.text);
            }
            result.Add(columnList);
            foreach (var dataDic in gridData.rows)
            {
                var dataList = new List<string>();
                foreach (var gridColumn in columnData.GridColumns)
                {
                    dataList.Add(dataDic[gridColumn.dataIndex]);
                }
                result.Add(dataList);
            }
            var excelBuilder = ExcelFactory.CreateBuilder();
            var sheet = excelBuilder.InsertSheet("统计数据");
            sheet.InsertSheetContent(ExcelExportRequest.GetInstance(result));
            var stream = new MemoryStream();
            excelBuilder.Save(stream,SaveFormat.Xlsx);
            stream.Position = 0;
            return stream;
        }

        
    }
}
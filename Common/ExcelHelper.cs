using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using Model;
using Model.Attribute;
using RuanYun.Excel.Builder;

namespace Common
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// 填充内容
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="request">待插入数据</param>
        public static void InsertSheetContent(this SheetBuilder sheet, ExcelExportRequest request)
        {
            var titleStyle = GetDefaultStyle(true);
            var rowStyle = GetDefaultStyle();
            sheet.SetSheetMerges(request.Merges);
            int rowIndex = request.RowIndex;
            var colWidths = new List<double>();
            foreach (var row in request.RowsData)
            {
                List<double> textRowNums = new List<double>();
                for (int col = 0; col < row.Count; col++)
                {
                    sheet.Worksheet.Cells[rowIndex, col].SetStyle(rowIndex < request.TitleRowCount ? titleStyle : rowStyle);
                    sheet.WriteText(rowIndex, col, row[col].Replace("^^", "\n"));
                    int length = Encoding.UTF8.GetBytes(row[col]).Length;
                    double width = length * 2 < 8 ? 8 : length * 2;
                    if (rowIndex == (request.TitleRowCount - 1))
                    {
                        double colSetWidth = request.ColumnsWidth != null && request.ColumnsWidth.Count > col ? request.ColumnsWidth[col] : 0;
                        double colWidth = colSetWidth > 0 ? colSetWidth : width;
                        sheet.SetColumnWidth(col, colWidth);
                        colWidths.Add(colWidth);
                    }
                    double textRowNum = 2;
                    if (rowIndex > (request.TitleRowCount - 1) && colWidths.Count > 0)
                    {
                        textRowNum = Math.Ceiling(width / colWidths[col]);
                    }
                    textRowNums.Add(textRowNum);
                }
                var rowHeight = rowStyle.Font.Size * (textRowNums.Max() <= 2 ? 2 : textRowNums.Max());
                sheet.SetRowHeight(rowIndex, rowHeight > 400 ? 400 : rowHeight);
                rowIndex++;
            }
            if (request.IsAutoRowFit) sheet.Worksheet.AutoFitRows();
        }

        /// <summary>
        /// 设置表格的单元格合并
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="merges"></param>
        public static void SetSheetMerges(this SheetBuilder sheet, List<ExcelHelperMergeModel> merges)
        {
            if (merges != null)
            {
                foreach (var merge in merges)
                {
                    sheet.MergeCells(merge.FirstRow, merge.FirstColumn, merge.TotalRows, merge.TotalColumns);
                }
            }
        }

        /// <summary>
        /// 将列名和数据合并到一个数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="titles"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public static ExcelExportRequest ConvertToRowsData<T>(List<List<string>> titles, List<T> models)
        {
            var rows = new List<List<string>>();
            var colWidths = new List<double>();
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (models == null || models.Count <= 0 || titles == null || titles.Any(d => d.Count <= 0 || d.Count != properties.Length)) return null;
            rows.AddRange(titles);
            foreach (var model in models)
            {
                var row = new List<string>();
                var sortDic = new SortedDictionary<int, string>();
                foreach (PropertyInfo t in properties)
                {
                    var attributes = t.GetCustomAttributes(typeof(ExcelOrderAttribute), false);
                    if (attributes.Length > 0)
                    {
                        var match = (attributes[0] as ExcelOrderAttribute);
                        if (match == null) continue;
                        var val = t.GetValue(model, null);
                        string text;
                        if (val == null || (match.IsReplaceHisotry && val.ToString().Equals(match.HistoryVal.ToString()))) text = "";
                        else text = val + match.Unit;
                        sortDic.Add(match.Order, text);
                        colWidths.Add(match.ColWidth);
                    }
                }
                foreach (var item in sortDic)
                {
                    row.Add(item.Value);
                }
                rows.Add(row);
            }
            return ExcelExportRequest.GetInstance(rows, 0, titles.Count, null, colWidths);
        }

        /// <summary>
        /// 获取样式
        /// </summary>
        /// <returns></returns>
        private static Style GetDefaultStyle(bool isTitle = false)
        {
            var style = new Style()
            {
                Pattern = BackgroundType.Solid,
                HorizontalAlignment = TextAlignmentType.Center,
                VerticalAlignment = TextAlignmentType.Center
            };
            style.Font.Size = 13;
            style.IsTextWrapped = true;
            if (isTitle)
            {
                style.Font.IsBold = true;
                style.ForegroundColor = Color.FromArgb(239, 239, 239);
            }
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.TopBorder].Color = Color.FromArgb(226, 226, 226);
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].Color = Color.FromArgb(226, 226, 226);
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].Color = Color.FromArgb(226, 226, 226);
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].Color = Color.FromArgb(226, 226, 226);
            return style;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// excel导出帮助类输入参数
    /// </summary>
    public class ExcelExportRequest
    {
        /// <summary>
        /// 待插入数据
        /// </summary>
        public List<List<string>> RowsData { get; set; }

        /// <summary>
        /// sheet插入RowsData数据的起始行数下标，例如：sheet之前已插入说明
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 标题行数
        /// </summary>
        public int TitleRowCount { get; set; }

        /// <summary>
        /// 待合并单元格数组
        /// </summary>
        public List<ExcelHelperMergeModel> Merges { get; set; }

        /// <summary>
        /// 列宽
        /// </summary>
        public List<double> ColumnsWidth { get; set; }

        /// <summary>
        /// 是否设置自适应行高
        /// </summary>
        public bool IsAutoRowFit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowsData"></param>
        /// <param name="rowIndex"></param>
        /// <param name="titalRowCount"></param>
        /// <param name="merges"></param>
        /// <param name="columnsWidth"></param>
        /// <param name="isAutoRowFit"></param>
        /// <returns></returns>
        public static ExcelExportRequest GetInstance(List<List<string>> rowsData, int rowIndex = 0, int titalRowCount = 1, List<ExcelHelperMergeModel> merges = null, List<double> columnsWidth = null, bool isAutoRowFit = false)
        {
            return new ExcelExportRequest()
            {
                RowsData = rowsData,
                RowIndex = rowIndex,
                TitleRowCount = titalRowCount,
                Merges = merges,
                ColumnsWidth = columnsWidth,
                IsAutoRowFit = isAutoRowFit
            };
        }
    }

    /// <summary>
    /// Excel的合并实体类
    /// </summary>
    public class ExcelHelperMergeModel
    {
        /// <summary>
        /// 行下标
        /// </summary>
        public int FirstRow { get; set; }

        /// <summary>
        /// 列下标
        /// </summary>
        public int FirstColumn { get; set; }

        /// <summary>
        /// 合并的行数
        /// </summary>
        public int TotalRows { get; set; }

        /// <summary>
        /// 合并的列数
        /// </summary>
        public int TotalColumns { get; set; }

        public static ExcelHelperMergeModel GetInstance(int firstRow, int firstColumn, int totalRows, int totalColumns)
        {
            return new ExcelHelperMergeModel()
            {
                FirstColumn = firstColumn,
                FirstRow = firstRow,
                TotalColumns = totalColumns,
                TotalRows = totalRows,
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Aspose.Cells;
using Common;
using Web.Models;
using Web.Models.Response;

namespace Web.Controllers.Actions.FinancialDataActions
{
    public class DeleteExcelColumnAction
    {
        public DeleteExcelColumnResponse Process(string keyWordName, int rowIndex)
        {
            var result = new DeleteExcelColumnResponse();
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                return result;
            }
            var file = HttpContext.Current.Request.Files[0];
            var excel = new Aspose.Cells.Workbook(file.InputStream);
            var sheet = excel.Worksheets[0];
            for (int i = 0; i < sheet.Cells.MaxDataColumn + 100; i++)
            {
                if (ExcelHelper.GetCellValue(sheet, rowIndex, i) == keyWordName)
                {
                    sheet.Cells.DeleteColumn(i, false);
                    i--;
                }
            }
            var directory = HttpContext.Current.Request.PhysicalApplicationPath;
            var relativePath = string.Concat("\\Files\\FinancialExcel\\", Guid.NewGuid().ToString("n"), ".xlsx");
            var filePath = string.Concat(directory, relativePath);
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            excel.Save(filePath, SaveFormat.Xlsx);
            result.success = true;
            result.FilePath = relativePath;
            return result;
        }
    }
}
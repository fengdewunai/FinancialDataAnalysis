using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBusiness;
using Web.Models.Response;

namespace Web.Controllers.Actions.FinancialDataActions
{
    /// <summary>
    /// 获取excel列表
    /// </summary>
    public class GetExcelListAction
    {
        private IFinancialDataBLL _financialDataBll;

        public GetExcelListAction(IFinancialDataBLL financialDataBll)
        {
            _financialDataBll = financialDataBll;
        }

        public List<GetExcelListResponse> Process()
        {
            var excelList = _financialDataBll.GetAllExcelRecord().OrderByDescending(x=>x.CreateDateTime);
            var result = new List<GetExcelListResponse>();
            foreach (var excel in excelList)
            {
                result.Add(new GetExcelListResponse()
                {
                    ExcelRecordId = excel.ExcelRecordId,
                    ExcelName = excel.ExcelName,
                    CreateDateTime = excel.CreateDateTime.ToString("yyyy-MM-dd HH:mm"),
                    ExcelUrl = string.IsNullOrEmpty(excel.ExcelUrl) ? "" : excel.ExcelUrl
                });
            }

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBusiness;
using Model;
using Model.DataModel;
using Model.Enum;
using Model.Response;

namespace Web.Controllers.Actions.FinancialDataActions
{
    /// <summary>
    /// 反查数据
    /// </summary>
    public class PeggingFinancialDataAction
    {
        private IFinancialDataBLL _financialDataBll;

        public PeggingFinancialDataAction(IFinancialDataBLL financialDataBll)
        {
            _financialDataBll = financialDataBll;
        }

        /// <summary>
        /// 业务逻辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<GetFinancialDataByPagingResponse> Process(GetFinancialDataByPagingRequest request)
        {
            var bllRequest = new GetFinancialDataByPagingRequest()
            {
                AccountCode = request.AccountCode,
                ExcelRecordId = request.ExcelRecordId,
                QiJianTypeId = request.QiJianTypeId,
                XiangMuItemId = request.XiangMuItemId,
                PageSize = request.PageSize,
                PageIndex = (request.PageIndex - 1) * request.PageSize
            };
            var financialDataItems = _financialDataBll.GetFinancialDataItemByExcelRecordId(request.ExcelRecordId);
            var dataItem = financialDataItems.FirstOrDefault(x => x.ItemId.ToString() == request.XiangMuItemId);
            request.XiangMuTypeId = dataItem.ItemTypeId;
            if (dataItem.ItemTypeId == (int) FinancialDataItemTypeEnum.XuNiJiTuan)
            {
                var children = financialDataItems.Where(x => x.ParentId == dataItem.ItemId);
                request.XiangMuItemId = string.Join(",", children.Select(x => x.ItemId).ToList());
            }

            var totalCount = 0;
            var result = _financialDataBll.GetFinancialDataByPaging(bllRequest, out totalCount);
            return result;
        }
    }
}
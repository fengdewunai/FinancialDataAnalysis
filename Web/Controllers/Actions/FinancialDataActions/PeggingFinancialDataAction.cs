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
        public GridBaseResponsek<List<GetFinancialDataByPagingResponse>> Process(GetFinancialDataByPagingRequest request)
        {
            var result = new GridBaseResponsek<List<GetFinancialDataByPagingResponse>>();
            var bllRequest = new GetFinancialDataByPagingRequest()
            {
                AccountCode = request.AccountCode,
                ExcelRecordId = request.ExcelRecordId,
                QiJianTypeId = request.QiJianTypeId,
                XiangMuItemId = request.XiangMuItemId,
                Limit = request.Limit,
                Page = (request.Page - 1) * request.Limit
            };
            var financialDataItems = _financialDataBll.GetFinancialDataItemByExcelRecordId(request.ExcelRecordId);
            var dataItem = financialDataItems.FirstOrDefault(x => x.ItemId.ToString() == request.XiangMuItemId);
            bllRequest.XiangMuTypeId = dataItem.ItemTypeId;
            if (dataItem.ItemTypeId == (int) FinancialDataItemTypeEnum.XuNiJiTuan)
            {
                var children = financialDataItems.Where(x => x.ParentId == dataItem.ItemId);
                bllRequest.XiangMuItemId = string.Join(",", children.Select(x => x.ItemId).ToList());
            }

            var totalCount = 0;
            var datas = _financialDataBll.GetFinancialDataByPaging(bllRequest, out totalCount);
            foreach (var data in datas)
            {
                data.DetailData = Math.Round(data.DetailData, 2);
            }
            result.total = totalCount;
            result.rows = datas;
            return result;
        }
    }
}
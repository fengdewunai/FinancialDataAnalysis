using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBusiness;
using Model;
using Model.DataModel;

namespace Web.Controllers.Actions.FinancialDataActions
{
    /// <summary>
    /// 获取项目树
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public class GetFinancialDataItemTreeDataAction
    {
        private IFinancialDataBLL _financialDataBll;

        public GetFinancialDataItemTreeDataAction(IFinancialDataBLL financialDataBll)
        {
            _financialDataBll = financialDataBll;
        }

        public List<TreeDataModel> Process(string id, int excelId)
        {
            var result = new List<TreeDataModel>();
            var items = new List<FinancialDataItemModel>();
            var datas = _financialDataBll.GetFinancialDataItemByExcelRecordId(excelId);
            if (id == "0")
            {
                items = datas.Where(x => x.ItemLevel == 1).ToList();
            }
            else
            {
                items = datas.Where(x => x.ParentId.ToString() == id).ToList();
            }
            foreach (var item in items)
            {
                result.Add(new TreeDataModel()
                {
                    id = item.ItemId.ToString(),
                    text = item.ItemName,
                    leaf = item.IsLeaf == 1,
                    @checked = false
                });
            }
            return result;
        }
    }
}
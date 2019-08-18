using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBusiness;
using Model;
using Model.DataModel;

namespace Web.Controllers.Actions.FinancialDataActions
{
    /// <summary>
    /// 获取科目树
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public class GetAccountItemTreeDataAction
    {
        private IFinancialDataBLL _financialDataBll;

        public GetAccountItemTreeDataAction(IFinancialDataBLL financialDataBll)
        {
            _financialDataBll = financialDataBll;
        }

        public List<TreeDataModel> Process(string id, int excelId)
        {
            var result = new List<TreeDataModel>();
            var items = new List<AccountItemModel>();
            var datas = _financialDataBll.GetAccountByExcelRecordId(excelId);
            if (id == "0")
            {
                items = datas.Where(x => x.AccountLevel == 1).ToList();
            }
            else
            {
                items = datas.Where(x => x.ParentCode == id).ToList();
            }
            foreach (var item in items)
            {
                result.Add(new TreeDataModel()
                {
                    id = item.AccountCode,
                    text = item.AccountName,
                    leaf = item.IsLeaf == 1,
                    @checked = false
                });
            }
            return result;
        }
    }
}
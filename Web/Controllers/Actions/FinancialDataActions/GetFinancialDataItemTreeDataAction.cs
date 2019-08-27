using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBusiness;
using Model;
using Model.DataModel;
using Model.Enum;

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

        public List<TreeDataModel> Process(string id, int excelId, int treeTypeId)
        {
            var result = new List<TreeDataModel>();
            var items = new List<FinancialDataItemModel>();
            var datas = _financialDataBll.GetFinancialDataItemByExcelRecordId(excelId);
            if (id == "0")
            {
                items = datas.Where(x => x.ItemLevel == 1).ToList();
            }
            else if(treeTypeId == 1) //展示片区
            {
                items = datas.Where(x => x.ItemTypeId != (int)FinancialDataItemTypeEnum.XingZhi && x.ParentId.ToString() == id).ToList();
            }
            else if(treeTypeId == 2) //展示性质
            {
                var currentItem = datas.FirstOrDefault(x => x.ItemId.ToString() == id);
                if (currentItem.ItemTypeId != (int)FinancialDataItemTypeEnum.XingZhi)
                {
                    items = datas.Where(x => (x.ItemTypeId == (int)FinancialDataItemTypeEnum.XingZhi || x.ItemTypeId == (int)FinancialDataItemTypeEnum.ShiYeBu) && x.ParentId.ToString() == id).ToList();
                }
                else
                {
                    items = datas.Where(x => x.ItemTypeId == (int)FinancialDataItemTypeEnum.XiangMu && x.XingZhiId.ToString() == id).ToList();
                }
                
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
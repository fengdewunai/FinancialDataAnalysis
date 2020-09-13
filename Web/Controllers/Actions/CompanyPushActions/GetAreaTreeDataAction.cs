using IBusiness;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Controllers.Actions.CompanyPushActions
{
    /// <summary>
    /// 获取树结构
    /// </summary>
    public class GetAreaTreeDataAction
    {
        private ICompanyBLL _companyBll;

        public GetAreaTreeDataAction(ICompanyBLL companyBll)
        {
            _companyBll = companyBll;
        }

        public List<TreeDataModel> Process(string id)
        {
            var result = new List<TreeDataModel>();
            if(string.IsNullOrEmpty(id) || id == "0")
            {
                var areas = _companyBll.GetAllArea();
                if(areas == null)
                {
                    return result;
                }
                foreach(var area in areas)
                {
                    result.Add(new TreeDataModel()
                    {
                        id = "1_" + area.AreaId,
                        text = area.AreaName,
                    });
                }
            }
            else
            {
                var idArr = id.Split('_');
                var areaId = Convert.ToInt32(idArr[1]);
                var companys = _companyBll.GetCompanyByAreaId(areaId);
                if(companys == null)
                {
                    return result;
                }
                foreach(var company in companys)
                {
                    result.Add(new TreeDataModel()
                    {
                        id = "2_" + company.CompanyId,
                        text = company.CompanyName,
                        leaf = true
                    });
                }
            }
            return result;
        }
    }
}
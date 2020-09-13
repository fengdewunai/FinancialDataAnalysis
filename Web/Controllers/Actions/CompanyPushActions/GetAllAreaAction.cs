using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Response.CompanyPush;

namespace Web.Controllers.Actions.CompanyPushActions
{
    /// <summary>
    /// 获取所以地区
    /// </summary>
    public class GetAllAreaAction
    {
        private ICompanyBLL _companyBll;

        public GetAllAreaAction(ICompanyBLL companyBll)
        {
            _companyBll = companyBll;
        }

        public List<GetAllAreaResponse> Process()
        {
            var result = new List<GetAllAreaResponse>();
            var allArea = _companyBll.GetAllArea();
            if(allArea == null)
            {
                return result;
            }
            foreach(var area in allArea)
            {
                result.Add(new GetAllAreaResponse()
                {
                    key = area.AreaId.ToString(),
                    value = area.AreaName
                });
            }
            return result;
        }
    }
}
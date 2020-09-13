using IBusiness;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Common.Helper;

namespace Web.Controllers.Actions.CompanyPushActions
{
    /// <summary>
    /// 获取公司表单
    /// </summary>
    public class GetAreaFormDataAction
    {
        private ICompanyBLL _companyBll;

        public GetAreaFormDataAction(ICompanyBLL companyBll)
        {
            _companyBll = companyBll;
        }

        public FormDataModel Process(string id)
        {
            var result = new FormDataModel() { success = true, data = new List<Dictionary<string, string>>()};
            if(string.IsNullOrEmpty(id))
            {
                return result;
            }
            var idArr = id.Split('_');
            var companyId = Convert.ToInt32(idArr[1]);
            var dic = new Dictionary<string, string>();
            var company = _companyBll.GetCompanyByKey(companyId);
            dic.Add("CompanyId", company.CompanyId.ToString());
            dic.Add("AreaId", company.AreaId.ToString());
            dic.Add("CompanyName", company.CompanyName?.ToString());
            dic.Add("ArtificialPerson", company.ArtificialPerson?.ToString());
            dic.Add("SetUpTime", company.SetUpTime.GetFormatStr());
            dic.Add("CompanyPhone", company.CompanyPhone?.ToString());
            dic.Add("Address", company.Address?.ToString());
            result.data.Add(dic);
            return result;
        }
    }
}
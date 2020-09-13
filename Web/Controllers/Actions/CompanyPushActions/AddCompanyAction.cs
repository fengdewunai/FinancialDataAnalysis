using IBusiness;
using Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Request;
using Common;

namespace Web.Controllers.Actions.CompanyPushActions
{
    /// <summary>
    /// 新增公司
    /// </summary>
    public class AddCompanyAction
    {
        private ICompanyBLL _companyBll;

        public AddCompanyAction(ICompanyBLL companyBll)
        {
            _companyBll = companyBll;
        }

        public void Process(CompanyFullInfoRequest request)
        {
            var companyInfo = request.MapTo<Company>();
            var companyId = _companyBll.SaveCompany(companyInfo);
            var companyRecordInfo = request.MapTo<CompanyConnectRecord>();
            companyRecordInfo.CompanyId = companyId;
            _companyBll.SaveCompanyConnectRecord(companyRecordInfo);
        }
    }
}
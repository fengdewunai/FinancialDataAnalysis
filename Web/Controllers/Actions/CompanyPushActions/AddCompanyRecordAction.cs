using Common;
using IBusiness;
using Model.data;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Request;

namespace Web.Controllers.Actions.CompanyPushActions
{
    /// <summary>
    /// 新增或编辑公司记录
    /// </summary>
    public class AddCompanyRecordAction
    {
        private ICompanyBLL _companyBll;

        public AddCompanyRecordAction(ICompanyBLL companyBll)
        {
            _companyBll = companyBll;
        }

        public void Process(CompanyFullInfoRequest request)
        {
            var companyRecordInfo = request.MapTo<CompanyConnectRecord>();
            if(request.AddCompanyTypeId == (int)AddCompanyTypeEnum.AddCompanyRecord)
            {
                companyRecordInfo.CompanyConnectRecordId = 0;
            }
            _companyBll.SaveCompanyConnectRecord(companyRecordInfo);
        }
    }
}
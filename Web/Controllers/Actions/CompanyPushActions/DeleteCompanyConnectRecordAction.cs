using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Controllers.Actions.CompanyPushActions
{
    public class DeleteCompanyConnectRecordAction
    {
        private ICompanyBLL _companyBll;

        public DeleteCompanyConnectRecordAction(ICompanyBLL companyBll)
        {
            _companyBll = companyBll;
        }

        public void Process(int companyConnectRecordId)
        {
            _companyBll.DeleteCompanyconnectrecordByCompanyId(companyConnectRecordId);
        }
    }
}
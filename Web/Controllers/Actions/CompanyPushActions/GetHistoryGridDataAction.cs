using IBusiness;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Common.Helper;

namespace Web.Controllers.Actions.CompanyPushActions
{
    public class GetHistoryGridDataAction
    {
        private ICompanyBLL _companyBll;

        public GetHistoryGridDataAction(ICompanyBLL companyBll)
        {
            _companyBll = companyBll;
        }

        public DynamicGridDataModel Process(string companyId)
        {
            var result = new DynamicGridDataModel() { total = 10000, rows = new List<Dictionary<string, string>>() };
            if(string.IsNullOrEmpty(companyId))
            {
                return result;
            }
            var idArr = companyId.Split('_');
            var id = Convert.ToInt32(idArr[1]);
            var allRecords = _companyBll.GetCompanyconnectrecordByCompanyId(id);
            if(allRecords == null)
            {
                return result;
            }
            foreach(var record in allRecords)
            {
                var dic = new Dictionary<string, string>();
                dic.Add("CompanyConnectRecordId", record.CompanyConnectRecordId.ToString());
                dic.Add("PhoneConnectDate", record.PhoneConnectDate.GetFormatStr());
                dic.Add("CompanyState", record.CompanyState.ToString());
                dic.Add("IsValidateAddress", record.IsValidateAddress.ToString());
                dic.Add("ConfirmAddress", record.ConfirmAddress?.ToString());
                dic.Add("GoHomePerson", record.GoHomePerson?.ToString());
                dic.Add("GoHomeTime", record.GoHomeTime.GetFormatStr());
                dic.Add("PhoneConnectState", record.PhoneConnectState?.ToString());
                dic.Add("CooperationIntention", record.CooperationIntention.ToString());
                result.rows.Add(dic);
            }
            return result;
        }
    }
}
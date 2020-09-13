using IBusiness;
using Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Request.CompanyPush;

namespace Web.Controllers.Actions.CompanyPushActions
{
    public class AddAreaAction
    {
        private ICompanyBLL _companyBll;

        public AddAreaAction(ICompanyBLL companyBll)
        {
            _companyBll = companyBll;
        }

        public void Process(AddAreaRequest request)
        {
            var area = new Area()
            {
                AreaName = request.AreaName,
                CreateDateTime = DateTime.Now,
                UpdateDateTime = DateTime.Now
            };
            _companyBll.SaveArea(area);
        }
    }
}
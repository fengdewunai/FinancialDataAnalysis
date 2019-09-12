using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using IBusiness;
using Microsoft.Practices.ServiceLocation;
using Model.DataModel;
using RuanYun.Logger;

namespace Web.Filters
{
    /// <summary>
    ///  统计调用次数
    /// </summary>
    public class StatisticalCallsAttribute : ActionFilterAttribute
    {

        private readonly IFinancialDataBLL _financialDataBll = ServiceLocator.Current.GetInstance<IFinancialDataBLL>();

        /// <summary>
        /// Action执行之前，判断用户角色状态
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionName = filterContext.ActionDescriptor.ActionName;
            var staticModel = new StatisticalCallsRecordModel()
            {
                ActionName = actionName,
                CreateDateTime = DateTime.Now
            };
            _financialDataBll.SaveStatisticalCallsRecord(staticModel);
        }
    }
}
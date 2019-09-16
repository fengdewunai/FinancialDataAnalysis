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
            var ip = GetLoginIp();
            var staticModel = new StatisticalCallsRecordModel()
            {
                ActionName = actionName,
                CreateDateTime = DateTime.Now,
                IpAddress = ip,
                SessionId = HttpContext.Current.Session.SessionID
            };
            _financialDataBll.SaveStatisticalCallsRecord(staticModel);
        }

        /// <summary> 
        /// 获取远程访问用户的Ip地址 
        /// </summary> 
        /// <returns>返回Ip地址</returns> 
        protected string GetLoginIp()
        {
            string loginip = "";
            //Request.ServerVariables[""]--获取服务变量集合  
            if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null) //判断发出请求的远程主机的ip地址是否为空 
            {
                //获取发出请求的远程主机的Ip地址 
                loginip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            //判断登记用户是否使用设置代理 
            else if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    //获取代理的服务器Ip地址 
                    loginip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    //获取客户端IP 
                    loginip = HttpContext.Current.Request.UserHostAddress;
                }
            }
            else
            {
                //获取客户端IP 
                loginip = HttpContext.Current.Request.UserHostAddress;
            }
            return loginip;
        }
    }
}
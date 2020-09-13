using Common;
using Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Models.Request;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Bootstrapper.Bootstrap();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            StrapRegister.Bootstrap(RegistAutoMapper);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(Server.MapPath("/logger.config")));
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 注册映射
        /// </summary>
        /// <param name="config"></param>
        private void RegistAutoMapper(BootstrapConfig config)
        {
            config.AutoMapperConfigurator = (cfg =>
            {
                cfg.CreateMap(typeof(CompanyFullInfoRequest), typeof(Company), false);
                cfg.CreateMap(typeof(CompanyFullInfoRequest), typeof(CompanyConnectRecord), false);
            });
                
        }
    }
}

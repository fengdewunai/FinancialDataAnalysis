using IBusiness;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFinancialDataBLL bll = ServiceLocator.Current.GetInstance<IFinancialDataBLL>();

        public ActionResult Index()
        {
            var a = bll.GetAllAccount();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
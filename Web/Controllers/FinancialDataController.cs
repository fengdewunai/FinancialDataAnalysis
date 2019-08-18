using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBusiness;
using Microsoft.Practices.ServiceLocation;
using Model;
using Model.DataModel;
using RuanYun.Logger;
using Web.Controllers.Actions.FinancialDataActions;
using Web.Models;

namespace Web.Controllers
{
    public class FinancialDataController : Controller
    {
        private readonly IFinancialDataBLL _financialDataBll = ServiceLocator.Current.GetInstance<IFinancialDataBLL>();

        // GET: FinancialData
        public ActionResult DetailDataIndex(int excelId)
        {
            ViewBag.excelId = excelId;
            return View();
        }

        // GET: FinancialData
        public ActionResult ExcelListIndex()
        {
            return View();
        }

        /// <summary>
        /// 获取excel列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetExcelList()
        {
            var action = new GetExcelListAction(_financialDataBll);
            var result = action.Process();
            return Json(result);
        }

        /// <summary>
        /// 获取科目树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAccountItemTreeData(string id, int excelId)
        {
            var action = new GetAccountItemTreeDataAction(_financialDataBll);
            var result = action.Process(id, excelId);
            return Json(result);
        }

        /// <summary>
        /// 获取项目树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFinancialDataItemTreeData(string id, int excelId)
        {
            var action = new GetFinancialDataItemTreeDataAction(_financialDataBll);
            var result = action.Process(id, excelId);
            return Json(result);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFinancialDataGridData(int excelId, string accountItemIds, string financialDataItemIds)
        {
            var action = new GetFinancialDataGridDataAction(_financialDataBll);
            var result = action.Process(excelId, accountItemIds, financialDataItemIds);
            return Json(result);
        }

        /// <summary>
        /// 获取数据表头数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFinancialDataGridColumns(int excelId, string financialDataItemIds)
        {
            var action = new GetFinancialDataGridColumnsAction(_financialDataBll);
            var result = action.Process(excelId, financialDataItemIds);
            return Json(result);
        }

        /// <summary>
        /// 上传Excel
        /// </summary>
        /// <param name="excelId"></param>
        /// <param name="excelName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpladExcel(string excelName)
        {
            try
            {
                var action = new UpladExcelAction(_financialDataBll);
                var result = action.Process(excelName);
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Write("上传Excel出错",MessageType.Error,typeof(FinancialDataController),ex);
                return Json(new SuccessModel());
            }
        }
    }
}
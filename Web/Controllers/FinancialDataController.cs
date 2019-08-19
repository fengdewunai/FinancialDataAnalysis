using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        private readonly IFinancialDataExtendBLL _financialDataExtendBll = ServiceLocator.Current.GetInstance<IFinancialDataExtendBLL>();

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
        /// <param name="excelId"></param>
        /// <param name="treeTypeId">树类型，1：展示片区，2：展示性质</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFinancialDataItemTreeData(string id, int excelId, int treeTypeId)
        {
            var action = new GetFinancialDataItemTreeDataAction(_financialDataBll);
            var result = action.Process(id, excelId, treeTypeId);
            return Json(result);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="excelId"></param>
        /// <param name="accountItemIds">科目id集合</param>
        /// <param name="financialDataItemIds">项目id集合</param>
        /// <param name="qiJianTypeId">会计期间类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFinancialDataGridData(int excelId, string accountItemIds, string financialDataItemIds, int qiJianTypeId = 1)
        {
            var action = new GetFinancialDataGridDataAction(_financialDataExtendBll);
            var result = action.Process(excelId, accountItemIds, financialDataItemIds, qiJianTypeId);
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
            var action = new GetFinancialDataGridColumnsAction(_financialDataExtendBll);
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

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="excelId"></param>
        /// <param name="accountItemIds">科目id集合</param>
        /// <param name="financialDataItemIds">项目id集合</param>
        /// <param name="qiJianTypeId">会计期间类型</param>
        /// <returns></returns>
        public FileResult ExportExcel(int excelId, string accountItemIds, string financialDataItemIds, int qiJianTypeId = 1)
        {
            var action = new ExportExcelAction(_financialDataBll, _financialDataExtendBll);
            var result = action.Process(excelId, accountItemIds, financialDataItemIds, qiJianTypeId);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("统计数据_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));
        }
    }
}
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
            try
            {
                var action = new GetExcelListAction(_financialDataBll);
                var result = action.Process();
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Write("获取科目树出错", MessageType.Error, typeof(FinancialDataController), ex);
                return Json(new SuccessModel());
            }
            
        }

        /// <summary>
        /// 获取科目树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAccountItemTreeData(string id, int excelId)
        {
            try
            {
                var action = new GetAccountItemTreeDataAction(_financialDataBll);
                var result = action.Process(id, excelId);
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Write("获取科目树出错", MessageType.Error, typeof(FinancialDataController), ex);
                return Json(new SuccessModel());
            }
            
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
            try
            {
                var action = new GetFinancialDataItemTreeDataAction(_financialDataBll);
                var result = action.Process(id, excelId, treeTypeId);
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Write("获取项目树出错", MessageType.Error, typeof(FinancialDataController), ex);
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
        [HttpPost]
        public ActionResult GetFinancialDataGridData(int excelId, string accountItemIds, string financialDataItemIds, int xiangMuTreeTypeId, int qiJianTypeId = 1, int onlyStatisticChildren = 0)
        {
            try
            {
                var action = new GetFinancialDataGridDataAction(_financialDataExtendBll);
                var result = action.Process(excelId, accountItemIds, financialDataItemIds, qiJianTypeId, onlyStatisticChildren, xiangMuTreeTypeId);
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Write("获取grid数据出错", MessageType.Error, typeof(FinancialDataController), ex);
                return Json(new SuccessModel());
            }
            
        }

        /// <summary>
        /// 获取数据表头数据
        /// </summary>
        /// <param name="excelId"></param>
        /// <param name="financialDataItemIds"></param>
        /// <param name="xiangMuTreeTypeId">1:片区，2：性质</param>
        /// <param name="onlyStatisticChildren"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFinancialDataGridColumns(int excelId, string financialDataItemIds, int xiangMuTreeTypeId, int onlyStatisticChildren = 0)
        {
            try
            {
                var action = new GetFinancialDataGridColumnsAction(_financialDataExtendBll);
                var result = action.Process(excelId, financialDataItemIds, xiangMuTreeTypeId, onlyStatisticChildren);
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Write("获取数据表头数据出错", MessageType.Error, typeof(FinancialDataController), ex);
                return Json(new SuccessModel());
            }
            
        }

        /// <summary>
        /// 上传Excel
        /// </summary>
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
        /// 删除Excel
        /// </summary>
        /// <param name="excelIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteExcel(string excelIds)
        {
            try
            {
                var action = new DeleteExcelAction(_financialDataBll);
                action.Process(excelIds);
                return Json(new SuccessModel(){success = true});
            }
            catch (Exception ex)
            {
                Log.Write("删除Excel出错", MessageType.Error, typeof(FinancialDataController), ex);
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
        public FileResult ExportExcel(int excelId, string accountItemIds, string financialDataItemIds, int xiangMuTreeTypeId, int qiJianTypeId = 1, int onlyStatisticChildren=0)
        {
            try
            {
                var action = new ExportExcelAction(_financialDataBll, _financialDataExtendBll);
                var result = action.Process(excelId, accountItemIds, financialDataItemIds, qiJianTypeId, onlyStatisticChildren, xiangMuTreeTypeId);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("统计数据_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));
            }
            catch (Exception ex)
            {
                Log.Write("获取数据出错", MessageType.Error, typeof(FinancialDataController), ex);
                return null;
            }
            
        }

        /// <summary>
        /// 反查数据
        /// </summary>
        /// <returns></returns>
        public ActionResult PeggingFinancialData(GetFinancialDataByPagingRequest request)
        {
            try
            {
                var action = new PeggingFinancialDataAction(_financialDataBll);
                var result = action.Process(request);
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Write("反查数据出错", MessageType.Error, typeof(FinancialDataController), ex);
                return Json(new SuccessModel());
            }
        }

        /// <summary>
        /// 删除excel某列
        /// </summary>
        /// <param name="keyWordName"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public ActionResult DeleteExcelColumn(string keyWordName, int rowIndex)
        {
            try
            {
                var action = new DeleteExcelColumnAction();
                var result = action.Process(keyWordName, rowIndex-1);
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Write("删除excel某列出错", MessageType.Error, typeof(FinancialDataController), ex);
                return Json(new SuccessModel());
            }
        }
    }
}
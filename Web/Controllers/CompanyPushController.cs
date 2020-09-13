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
using Web.Controllers.Actions.CompanyPushActions;
using Web.Models;
using Web.Models.Request;
using Web.Models.Request.CompanyPush;

namespace Web.Controllers
{
    public class CompanyPushController : Controller
    {
        private readonly ICompanyBLL _companyBll = ServiceLocator.Current.GetInstance<ICompanyBLL>();


        // GET: FinancialData
        public ActionResult CompanyPushIndex()
        {
            return View();
        }

        /// <summary>
        /// 新增地区
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddArea(AddAreaRequest request)
        {
            try
            {
                var action = new AddAreaAction(_companyBll);
                action.Process(request);
                return Json(new SuccessModel() { success = true});

            }
            catch (Exception ex)
            {
                Log.Write("新增地区出错", MessageType.Error, typeof(CompanyPushController), ex);
                return Json(new SuccessModel());
            }
        } 

        /// <summary>
        /// 获取所以地区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllArea()
        {
            try
            {
                var action = new GetAllAreaAction(_companyBll);
                var result = action.Process();
                return Json(result,JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Write("获取所以地区出错", MessageType.Error, typeof(CompanyPushController), ex);
                return Json(new SuccessModel(),JsonRequestBehavior.AllowGet);
            }
        }  

        /// <summary>
        /// 获取地区树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAreaTreeData(string id)
        {
            try
            {
                var action = new GetAreaTreeDataAction(_companyBll);
                var result = action.Process(id);
                return Json(result);

                //var result = new List<TreeDataModel>();
                //var idArr = id.Split('_');
                //if (string.IsNullOrEmpty(id) || id == "0")
                //{
                //    result = new List<TreeDataModel>()
                //    {
                //        new TreeDataModel()
                //        {
                //            id = "1_1",
                //            text = "江西省南昌市南昌高新技术产业开发区"
                //        },
                //        new TreeDataModel()
                //        {
                //            id = "1_2",
                //            text = "江西省南昌市南昌红谷滩区"
                //        }

                //    };
                //}
                //else
                //{
                //    if (idArr[1] == "1")
                //    {
                //        result.Add(new TreeDataModel()
                //        {
                //            id = "2_1",
                //            text = "南昌精算通财务咨询有限公司",
                //            leaf = true
                //        });
                //    }
                //    else
                //    {
                //        result.Add(new TreeDataModel()
                //        {
                //            id = "2_2",
                //            text = "软云科技有限公司",
                //            leaf = true
                //        });
                //    }
                  
                //}
                //return Json(result);
            }
            catch (Exception ex)
            {
                Log.Write("获取地区树出错", MessageType.Error, typeof(CompanyPushController), ex);
                return Json(new SuccessModel());
            }

        }

        /// <summary>
        /// 获取历史记录Grid
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHistoryGridData(string companyId)
        {
            try
            {
                var action = new GetHistoryGridDataAction(_companyBll);
                var result = action.Process(companyId);
                return Json(result);

            }
            catch (Exception ex)
            {
                Log.Write("获取历史记录Grid出错", MessageType.Error, typeof(CompanyPushController), ex);
                return Json(new SuccessModel());
            }
            //var gridDatas = new List<Dictionary<string, string>>();
            //var rowTitle = new Dictionary<string, string>();
            //rowTitle.Add("LianXiQingKuang", "之前电销联系情况1");
            //rowTitle.Add("QuRenDiZhi", "确认地址1");
            //rowTitle.Add("ErCiQueRen", "二次确认1");
            //rowTitle.Add("ZhuXiao", "注销1");
            //rowTitle.Add("ZhengQueDiZhi", "正确地址1");
            //rowTitle.Add("ShangMenRenYuan", "上门人员1");
            //gridDatas.Add(rowTitle);
            //var rowOne = new Dictionary<string, string>();
            //rowOne.Add("LianXiQingKuang", "之前电销联系情况2");
            //rowOne.Add("QuRenDiZhi", "确认地址2");
            //rowOne.Add("ErCiQueRen", "二次确认2");
            //rowOne.Add("ZhuXiao", "注销2");
            //rowOne.Add("ZhengQueDiZhi", "正确地址2");
            //rowOne.Add("ShangMenRenYuan", "上门人员2");
            //gridDatas.Add(rowOne);

            //var result = new DynamicGridDataModel()
            //{
            //    total = 10,
            //    rows = gridDatas
            //};
            //return Json(result);
        }

        /// <summary>
        /// 获取form内容
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAreaFormData(string id)
        {
            try
            {
                var action = new GetAreaFormDataAction(_companyBll);
                var result = action.Process(id);
                return Json(result);

            }
            catch (Exception ex)
            {
                Log.Write("获取form内容出错", MessageType.Error, typeof(CompanyPushController), ex);
                return Json(new SuccessModel());
            }
            //var datas = new List<Dictionary<string, string>>();
            //var rowTitle = new Dictionary<string, string>();
            //rowTitle.Add("QuYu", "江西省南昌市南昌高新技术产业开发区");
            //rowTitle.Add("QiYeMingCheng", "南昌精算通财务咨询有限公司");
            //rowTitle.Add("FaRen", "李建");
            //rowTitle.Add("ChengLiRiQi", "2011-4-22");
            //rowTitle.Add("ZhuSuo", "江西省南昌市南昌高新技术产业开发区高新二路18号高新创业园203室");
            //rowTitle.Add("LianXiDianHua", "13307085511");
            //datas.Add(rowTitle);

            //var result = new FormDataModel()
            //{
            //    success = true,
            //    data = datas
            //};
            //return Json(result);
        }

        /// <summary>
        /// 删除联系记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteCompanyConnectRecord(int companyConnectRecordId)
        {
            try
            {
                var action = new DeleteCompanyConnectRecordAction(_companyBll);
                action.Process(companyConnectRecordId);
                return Json(new SuccessModel() { success = true});

            }
            catch (Exception ex)
            {
                Log.Write("删除联系记录出错", MessageType.Error, typeof(CompanyPushController), ex);
                return Json(new SuccessModel());
            }
        }

        /// <summary>
        /// 新增公司
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCompany(CompanyFullInfoRequest request)
        {
            try
            {
                var action = new AddCompanyAction(_companyBll);
                action.Process(request);
                return Json(new SuccessModel() { success = true});

            }
            catch (Exception ex)
            {
                Log.Write("新增公司出错", MessageType.Error, typeof(CompanyPushController), ex);
                return Json(new SuccessModel());
            }
        } 

        /// <summary>
        /// 新增或编辑公司记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCompanyRecord(CompanyFullInfoRequest request)
        {
            try
            {
                var action = new AddCompanyRecordAction(_companyBll);
                action.Process(request);
                return Json(new SuccessModel() { success = true});

            }
            catch (Exception ex)
            {
                Log.Write("新增公司出错", MessageType.Error, typeof(CompanyPushController), ex);
                return Json(new SuccessModel());
            }
        } 

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public FileResult ExportExcel(string ids)
        {
            try
            {
                var action = new ExportExcelAction(_companyBll);
                var result = action.Process(ids);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("公司地推表_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));
            }
            catch (Exception ex)
            {
                Log.Write("导出grid数据excel出错", MessageType.Error, typeof(FinancialDataController), ex);
                return null;
            }
            
        }
    }
}
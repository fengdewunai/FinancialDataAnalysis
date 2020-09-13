using Aspose.Cells;
using Common;
using IBusiness;
using Model;
using Model.data;
using RuanYun.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Web.Common.Helper;

namespace Web.Controllers.Actions.CompanyPushActions
{
    public class ExportExcelAction
    {
        private ICompanyBLL _companyBll;

        public ExportExcelAction(ICompanyBLL companyBll)
        {
            _companyBll = companyBll;
        }

        /// <summary>
        /// 业务逻辑
        /// </summary>
        /// <param name="excelId"></param>
        /// <returns></returns>
        public Stream Process(string ids)
        {
            var excelDatas = new List<List<string>>();
            excelDatas.Add(GetExcelTitel());
            var idsArr = ids.Split(',');
            var companyList = new List<Company>();
            var areas = _companyBll.GetAllArea();
            foreach(var id in idsArr)
            {
                if(string.IsNullOrEmpty(id))
                {
                    continue;
                }
                var idArr = id.Split('_');
                var findId = Convert.ToInt32(idArr[1]);
                if(idArr[0] == "1")
                {
                    var companys = _companyBll.GetCompanyByAreaId(findId);
                    if(companys != null)
                    {
                        companyList.AddRange(companys);
                    }
                }
                else
                {
                    var company = _companyBll.GetCompanyByKey(findId);
                    if(company != null)
                    {
                        companyList.Add(company);
                    }
                }
            }
            foreach(var company in companyList)
            {
                var area = areas.FirstOrDefault(x => x.AreaId == company.AreaId);
                var companyRecords = _companyBll.GetCompanyconnectrecordByCompanyId(company.CompanyId);
                if(companyRecords != null && companyRecords.Count > 0)
                {
                    foreach(var record in companyRecords)
                    {
                        excelDatas.Add(GetOneCellData(area,company,record));
                    }
                }
                else
                {
                    excelDatas.Add(GetOneCellData(area,company,new CompanyConnectRecord()));
                }
                
            }
            var excelBuilder = ExcelFactory.CreateBuilder();
            var sheet = excelBuilder.InsertSheet("sheet1");
            sheet.InsertSheetContent(ExcelExportRequest.GetInstance(excelDatas));
            var stream = new MemoryStream();
            excelBuilder.Save(stream,SaveFormat.Xlsx);
            stream.Position = 0;
            return stream;
        }

        private List<string> GetOneCellData(Area area, Company company, CompanyConnectRecord record)
        {
            var companyState = "";
            switch(record.CompanyState)
            {
                case 1:
                    companyState = "已注销";
                    break;
                case 2:
                    companyState = "存续";
                    break;
                case 3:
                    companyState = "待确认";
                    break;
            }
            var isValidateAddress = "";
            switch(record.IsValidateAddress)
            {
                case 1:
                    isValidateAddress = "有效";
                    break;
                case 2:
                    isValidateAddress = "无效";
                    break;
            }
            var cooperationIntention = "";
            switch(record.CooperationIntention)
            {
                case 1:
                    cooperationIntention = "无意向";
                    break;
                case 2:
                    cooperationIntention = "有意向";
                    break;
                case 3:
                    cooperationIntention = "待确认";
                    break;
            }
            var result = new List<string>();
            result.Add(area == null ? "" : area.AreaName);
            result.Add(company.CompanyName);
            result.Add(company.ArtificialPerson);
            result.Add(company.SetUpTime.GetFormatStr());
            result.Add(company.CompanyPhone);
            result.Add(company.Address);
            result.Add(record.PhoneConnectDate.GetFormatStr());
            result.Add(companyState);
            result.Add(isValidateAddress);
            result.Add(record.ConfirmAddress);
            result.Add(record.GoHomePerson);
            result.Add(record.GoHomeTime.GetFormatStr());
            result.Add(record.PhoneConnectState);
            result.Add(cooperationIntention);
            return result;
        }

        private List<string> GetExcelTitel()
        {
            var result = new List<string>();
            result.Add("区域");
            result.Add("企业名称");
            result.Add("法人");
            result.Add("成立日期");
            result.Add("联系电话");
            result.Add("住所1");
            result.Add("电销联系日期");
            result.Add("公司现状");
            result.Add("是否有效地址");
            result.Add("确认地址");
            result.Add("上门拜访人员");
            result.Add("拜访时间");
            result.Add("电话联系情况");
            result.Add("合作意向情况");
            return result;
        }
    }
}
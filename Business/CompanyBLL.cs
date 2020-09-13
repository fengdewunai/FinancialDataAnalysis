using IBusiness;
using IDataAccess;
using Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class CompanyBLL: ICompanyBLL
    {
        /// <summary>
        /// 获取数据数据层
        /// </summary>
        private ICompanyDAL _companyDal;

        public CompanyBLL(ICompanyDAL companyDal)
        {
            _companyDal = companyDal;
        }

        public Company GetCompanyByKey(int companyId)
        {
            return _companyDal.GetCompanyByKey(companyId);
        }

        public List<Company> GetCompanyByAreaId(int areaId)
        {
            return _companyDal.GetCompanyByAreaId(areaId);
        }

        public int SaveCompany(Company dataModel)
        {
            dataModel.UpdateDateTime = DateTime.Now;
            return _companyDal.SaveCompany(dataModel);
        }

        public CompanyConnectRecord GetCompanyconnectrecordByKey(int companyConnectRecordId)
        {
            return _companyDal.GetCompanyconnectrecordByKey(companyConnectRecordId);
        }

        public List<CompanyConnectRecord> GetCompanyconnectrecordByCompanyId(int companyId)
        {
            return _companyDal.GetCompanyconnectrecordByCompanyId(companyId);
        }

        public void DeleteCompanyconnectrecordByCompanyId(int companyConnectRecordId)
        {
            _companyDal.DeleteCompanyconnectrecordByCompanyId(companyConnectRecordId);
        }

        public int SaveCompanyConnectRecord(CompanyConnectRecord dataModel)
        {
            dataModel.UpdateDateTime = DateTime.Now;
            return _companyDal.SaveCompanyConnectRecord(dataModel);
        }

        public Area GetAreaByKey(int areaId)
        {
            return _companyDal.GetAreaByKey(areaId);
        }

        public List<Area> GetAllArea()
        {
            return _companyDal.GetAllArea();
        }

        public int SaveArea(Area dataModel)
        {
            dataModel.UpdateDateTime = DateTime.Now;
            return _companyDal.SaveArea(dataModel);
        }
    }
}

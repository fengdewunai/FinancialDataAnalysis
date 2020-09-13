using Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface ICompanyDAL
    {
        Company GetCompanyByKey(int companyId);

        List<Company> GetCompanyByAreaId(int areaId);

        int SaveCompany(Company dataModel);

        CompanyConnectRecord GetCompanyconnectrecordByKey(int companyConnectRecordId);

        List<CompanyConnectRecord> GetCompanyconnectrecordByCompanyId(int companyId);

        void DeleteCompanyconnectrecordByCompanyId(int companyConnectRecordId);

        int SaveCompanyConnectRecord(CompanyConnectRecord dataModel);

        Area GetAreaByKey(int areaId);

        List<Area> GetAllArea();

        int SaveArea(Area dataModel);
    }
}

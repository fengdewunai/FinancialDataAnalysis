using Common;
using IDataAccess;
using Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess
{
    public class CompanyDAL : DataAccessBase, ICompanyDAL
    {
        public Company GetCompanyByKey(int companyId)
        {
            return CurrentConnectStringContext.StoredProcedure("Financialdataanalysis_SP_Company_ReadByKey")
                .Parameter("v_CompanyId", companyId)
                .QuerySingle<Company>();
        }

        public List<Company> GetCompanyByAreaId(int areaId)
        {
            return CurrentConnectStringContext.StoredProcedure("Financialdataanalysis_SP_Company_ReadByAreaId")
                .Parameter("v_AreaId", areaId)
                .QueryMany<Company>();
        }

        public int SaveCompany(Company dataModel)
        {
            return CurrentConnectStringContext.StoredProcedure("Financialdataanalysis_SP_Company_Save")
                .Parameter("v_CompanyId", dataModel.CompanyId)
                .Parameter("v_AreaId", dataModel.AreaId)
                .Parameter("v_CompanyName", dataModel.CompanyName)
                .Parameter("v_ArtificialPerson", dataModel.ArtificialPerson)
                .Parameter("v_SetUpTime", dataModel.SetUpTime)
                .Parameter("v_CompanyPhone", dataModel.CompanyPhone)
                .Parameter("v_Address", dataModel.Address)
                .Parameter("v_CreateDateTime", dataModel.CreateDateTime)
                .Parameter("v_UpdateDateTime", dataModel.UpdateDateTime)
                .QuerySingle<int>();
        }

        public CompanyConnectRecord GetCompanyconnectrecordByKey(int companyConnectRecordId)
        {
            return CurrentConnectStringContext.StoredProcedure("Financialdataanalysis_SP_Companyconnectrecord_ReadByKey")
                .Parameter("v_CompanyConnectRecordId", companyConnectRecordId)
                .QuerySingle<CompanyConnectRecord>();
        }

        public List<CompanyConnectRecord> GetCompanyconnectrecordByCompanyId(int companyId)
        {
            return CurrentConnectStringContext.StoredProcedure("Financialdataanalysis_SP_Companyconnectrecord_ReadByCompanyId")
                .Parameter("v_CompanyId", companyId)
                .QueryMany<CompanyConnectRecord>();
        }

        public void DeleteCompanyconnectrecordByCompanyId(int companyConnectRecordId)
        {
             CurrentConnectStringContext.StoredProcedure("Financialdataanalysis_SP_Companyconnectrecord_Delete")
                .Parameter("v_CompanyConnectRecordId", companyConnectRecordId)
                .Execute();
        }

        public int SaveCompanyConnectRecord(CompanyConnectRecord dataModel)
        {
            return CurrentConnectStringContext.StoredProcedure("Financialdataanalysis_SP_Companyconnectrecord_Save")
                .Parameter("v_CompanyConnectRecordId", dataModel.CompanyConnectRecordId)
                .Parameter("v_CompanyId", dataModel.CompanyId)
                .Parameter("v_PhoneConnectDate", dataModel.PhoneConnectDate)
                .Parameter("v_CompanyState", dataModel.CompanyState)
                .Parameter("v_IsValidateAddress", dataModel.IsValidateAddress)
                .Parameter("v_ConfirmAddress", dataModel.ConfirmAddress)
                .Parameter("v_GoHomePerson", dataModel.GoHomePerson)
                .Parameter("v_GoHomeTime", dataModel.GoHomeTime)
                .Parameter("v_PhoneConnectState", dataModel.PhoneConnectState)
                .Parameter("v_CooperationIntention", dataModel.CooperationIntention)
                .Parameter("v_CreateDateTime", dataModel.CreateDateTime)
                .Parameter("v_UpdateDateTime", dataModel.UpdateDateTime)
                .QuerySingle<int>();
        }

        public Area GetAreaByKey(int areaId)
        {
            return CurrentConnectStringContext.StoredProcedure("Financialdataanalysis_SP_Area_ReadByKey")
                .Parameter("v_AreaId", areaId)
                .QuerySingle<Area>();
        }

        public List<Area> GetAllArea()
        {
            return CurrentConnectStringContext.StoredProcedure("Financialdataanalysis_SP_Area_ReadAll")
                .QueryMany<Area>();
        }

        public int SaveArea(Area dataModel)
        {
            return CurrentConnectStringContext.StoredProcedure("Financialdataanalysis_SP_Area_Save")
                .Parameter("v_AreaId", dataModel.AreaId)
                .Parameter("v_AreaName", dataModel.AreaName)
                .Parameter("v_UserId", dataModel.UserId)
                .Parameter("v_CreateDateTime", dataModel.CreateDateTime)
                .Parameter("v_UpdateDateTime", dataModel.UpdateDateTime)
                .QuerySingle<int>();
        }
    }
}

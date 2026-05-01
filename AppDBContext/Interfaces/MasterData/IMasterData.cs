using AppDBContext.Models;
using AppDBContext.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.MasterData
{
    public interface IMasterData
    {
        #region Mst Menu

        Task<List<MstMenu>> GetAllMenuData(string Clause);
        Task<APIResponseModel> Crud(MstMenu oMstMenu);

        #endregion

        #region Mst Form
        Task<List<MstForm>> GetAllFormData(string Clause);
        Task<APIResponseModel> Crud(MstForm oMstForm);

        #endregion

        #region Mst Area

        Task<List<MstArea>> GetAllAreaData(string Clause);
        Task<APIResponseModel> Crud(MstArea oMstArea);

        #endregion

        #region Mst City
        Task<List<MstCity>> GetAllCityData(string Clause);
        Task<APIResponseModel> Crud(MstCity oMstCity);

        #endregion

        #region Mst BusinessCustomer
        Task<List<MstBusinessCustomer>> GetAllBusinessCustomerData(string Clause);
        Task<APIResponseModel> Crud(MstBusinessCustomer oMstBusinessCustomer);

        #endregion

        #region Default Value

        Task<List<CfgDefaultValue>> GetAllDefaultValueData(string Clause);

        #endregion

        #region Mst Associates

        Task<List<MstAssociates>> GetAllAssociatesData(string Clause);
        Task<List<MstAssociatesAvailability>> GetAllAssociatesAvailabilityData(string Clause);
        Task<List<MstAssociatesPackages>> GetAllAssociatesPackagesData(string Clause);
        Task<APIResponseModel> Crud(MstAssociates oMstAssociates);

        #endregion

        #region Trns Packages

        Task<List<TrnsPackages>> GetAllPackagesData(string Clause);
        Task<List<TrnsPackagesDetail>> GetAllPackagesDetailData(string Clause);
        Task<APIResponseModel> Crud(TrnsPackages oTrnsPackages);

        #endregion

        #region Mst Facility

        Task<List<MstFacility>> GetAllFacilityData(string Clause);
        Task<APIResponseModel> Crud(MstFacility oMstFacility);

        #endregion

        #region Mst Report
        Task<List<MstReport>> GetAllReport(string Clause);
        Task<APIResponseModel> Crud(MstReport oMstReportSetup);
        Task<APIResponseModel> Crud(List<MstReport> oMstReportSetup);
        Task<APIResponseModel> DeleteReport(int ID, string UserCode, string ReportCode);

        #endregion   
    }
}
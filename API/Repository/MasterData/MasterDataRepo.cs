using API.DapperDAL;
using AppDBContext.General;
using AppDBContext.Interfaces.Dapper;
using AppDBContext.Interfaces.MasterData;
using AppDBContext.Models;
using AppDBContext.VMModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Repository.MasterData
{
    public class MasterDataRepo : IMasterData
    {
        private IDapper _dapper;

        public MasterDataRepo(IDapper dapper)
        {
            _dapper = dapper;
        }

        #region Mst Menu
        public async Task<List<MstMenu>> GetAllMenuData(string Clause)
        {
            List<MstMenu> oList = new List<MstMenu>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstMenu>(Clause);
                    oList = _dapper.SelectQueryList<MstMenu>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(MstMenu oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();
                        string xmlData = _dapper.ConvertToXml(oModel);
                        string Query = oQuery.FormatMergeQuery<MstMenu>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstMenu>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        response.Id = 1;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Update successfully";
                        }
                        else
                        {
                            response.Message = "Saved successfully";
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Failed to Update successfully";
                        }
                        else
                        {
                            response.Message = "Failed to Saved successfully";
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion

        #region Mst Form
        public async Task<List<MstForm>> GetAllFormData(string Clause)
        {
            List<MstForm> oList = new List<MstForm>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstForm>(Clause);
                    oList = _dapper.SelectQueryList<MstForm>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(MstForm oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();
                        string xmlData = _dapper.ConvertToXml(oModel);
                        string Query = oQuery.FormatMergeQuery<MstForm>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstForm>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        response.Id = 1;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Update successfully";
                        }
                        else
                        {
                            response.Message = "Saved successfully";
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Failed to Update successfully";
                        }
                        else
                        {
                            response.Message = "Failed to Saved successfully";
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion

        #region Mst Area
        public async Task<List<MstArea>> GetAllAreaData(string Clause)
        {
            List<MstArea> oList = new List<MstArea>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstArea>(Clause);
                    oList = _dapper.SelectQueryList<MstArea>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(MstArea oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();
                        string xmlData = _dapper.ConvertToXml(oModel);
                        string Query = oQuery.FormatMergeQuery<MstArea>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstArea>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        response.Id = 1;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Update successfully";
                        }
                        else
                        {
                            response.Message = "Saved successfully";
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Failed to Update successfully";
                        }
                        else
                        {
                            response.Message = "Failed to Saved successfully";
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion

        #region Mst City
        public async Task<List<MstCity>> GetAllCityData(string Clause)
        {
            List<MstCity> oList = new List<MstCity>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstCity>(Clause);
                    oList = _dapper.SelectQueryList<MstCity>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(MstCity oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();
                        string xmlData = _dapper.ConvertToXml(oModel);
                        string Query = oQuery.FormatMergeQuery<MstCity>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstCity>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        response.Id = 1;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Update successfully";
                        }
                        else
                        {
                            response.Message = "Saved successfully";
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Failed to Update successfully";
                        }
                        else
                        {
                            response.Message = "Failed to Saved successfully";
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion

        #region Cfg Default Value
        public async Task<List<CfgDefaultValue>> GetAllDefaultValueData(string Clause)
        {
            List<CfgDefaultValue> oList = new List<CfgDefaultValue>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<CfgDefaultValue>(Clause);
                    oList = _dapper.SelectQueryList<CfgDefaultValue>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }

        #endregion

        #region Mst Associates
        public async Task<List<MstAssociates>> GetAllAssociatesData(string Clause)
        {
            List<MstAssociates> oList = new List<MstAssociates>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstAssociates>(Clause);
                    string QueryDetail1 = oQuery.FormatSelectQuery<MstAssociatesPackages>(Clause);
                    string QueryDetail2 = oQuery.FormatSelectQuery<MstAssociatesAvailability>(Clause);

                    var groupHeader = _dapper.SelectQueryList<MstAssociates>(Query)
                                .GroupJoin(
                                    _dapper.SelectQueryList<MstAssociatesPackages>(QueryDetail1),
                                    header => header.DocEntry,
                                    line1 => line1.DocEntry,
                                    (header, detailGroup) =>
                                    {
                                        if (detailGroup != null && detailGroup.Any())
                                            header.AssociatesPackages = detailGroup.ToList();
                                        return header;
                                    }
                                )
                                .GroupJoin(
                                    _dapper.SelectQueryList<MstAssociatesAvailability>(QueryDetail2),
                                    header => header.DocEntry,
                                    line2 => line2.DocEntry,
                                    (header, detailGroup) =>
                                    {
                                        if (detailGroup != null && detailGroup.Any())
                                            header.AssociatesAvailability = detailGroup.ToList();
                                        return header;
                                    }
                                ).ToList();
                    oList = groupHeader.ToList();

                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<List<MstAssociatesAvailability>> GetAllAssociatesAvailabilityData(string Clause)
        {
            List<MstAssociatesAvailability> oList = new List<MstAssociatesAvailability>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstAssociatesAvailability>(Clause);
                    oList = _dapper.SelectQueryList<MstAssociatesAvailability>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<List<MstAssociatesPackages>> GetAllAssociatesPackagesData(string Clause)
        {
            List<MstAssociatesPackages> oList = new List<MstAssociatesPackages>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstAssociatesPackages>(Clause);
                    oList = _dapper.SelectQueryList<MstAssociatesPackages>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(MstAssociates oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();
                        string xmlData = _dapper.ConvertToXml(oModel);
                        string xmlDataDetail1 = _dapper.ConvertToXml(oModel.AssociatesPackages.ToList());
                        string xmlDataDetail2 = _dapper.ConvertToXml(oModel.AssociatesAvailability.ToList());
                        oModel.AssociatesPackages.ForEach(x => x.AssociateKey = oModel.UniqueKey);
                        oModel.AssociatesAvailability.ForEach(x => x.AssociateKey = oModel.UniqueKey);
                        string Query = oQuery.FormatMergeQuery<MstAssociates>(false, false, true, false);
                        Query += oQuery.FormatMergeQuery<MstAssociatesPackages>(true, true, true, false);
                        Query += oQuery.FormatMergeQuery<MstAssociatesAvailability>(true, true, true, false);
                        response.Id = _dapper.CRUDQuery<MstAssociates, MstAssociatesPackages, MstAssociatesAvailability>(Query, xmlData, xmlDataDetail1, xmlDataDetail2);
                    }
                    if (response.Id > 0)
                    {
                        response.Id = 1;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Update successfully";
                        }
                        else
                        {
                            response.Message = "Saved successfully";
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Failed to Update successfully";
                        }
                        else
                        {
                            response.Message = "Failed to Saved successfully";
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion

        #region Trns Packages
        public async Task<List<TrnsPackages>> GetAllPackagesData(string Clause)
        {
            List<TrnsPackages> oList = new List<TrnsPackages>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<TrnsPackages>(Clause);
                    string QueryDetail1 = oQuery.FormatSelectQuery<TrnsPackagesDetail>(Clause);

                    var groupHeader = _dapper.SelectQueryList<TrnsPackages>(Query)
                                .GroupJoin(
                                    _dapper.SelectQueryList<TrnsPackagesDetail>(QueryDetail1),
                                    header => header.DocEntry,
                                    line1 => line1.DocEntry,
                                    (header, detailGroup) =>
                                    {
                                        if (detailGroup != null && detailGroup.Any())
                                            header.PackagesDetail = detailGroup.ToList();
                                        return header;
                                    }
                                ).ToList();
                    oList = groupHeader.ToList();

                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<List<TrnsPackagesDetail>> GetAllPackagesDetailData(string Clause)
        {
            List<TrnsPackagesDetail> oList = new List<TrnsPackagesDetail>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<TrnsPackagesDetail>(Clause);
                    oList = _dapper.SelectQueryList<TrnsPackagesDetail>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(TrnsPackages oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();
                        string xmlData = _dapper.ConvertToXml(oModel);
                        string xmlDataDetail1 = _dapper.ConvertToXml(oModel.PackagesDetail.ToList());
                        oModel.PackagesDetail.ForEach(x => x.PackageKey = oModel.UniqueKey);
                        string Query = oQuery.FormatMergeQuery<TrnsPackages>(false, false, true, false);
                        Query += oQuery.FormatMergeQuery<TrnsPackagesDetail>(true, true, true, false);
                        response.Id = _dapper.CRUDQuery<TrnsPackages, TrnsPackagesDetail>(Query, xmlData, xmlDataDetail1);
                    }
                    if (response.Id > 0)
                    {
                        response.Id = 1;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Update successfully";
                        }
                        else
                        {
                            response.Message = "Saved successfully";
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Failed to Update successfully";
                        }
                        else
                        {
                            response.Message = "Failed to Saved successfully";
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion

        #region Mst Facility
        public async Task<List<MstFacility>> GetAllFacilityData(string Clause)
        {
            List<MstFacility> oList = new List<MstFacility>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstFacility>(Clause);
                    oList = _dapper.SelectQueryList<MstFacility>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(MstFacility oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();
                        string xmlData = _dapper.ConvertToXml(oModel);
                        string Query = oQuery.FormatMergeQuery<MstFacility>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstFacility>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        response.Id = 1;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Update successfully";
                        }
                        else
                        {
                            response.Message = "Saved successfully";
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        if (oModel.Id > 0)
                        {
                            response.Message = "Failed to Update successfully";
                        }
                        else
                        {
                            response.Message = "Failed to Saved successfully";
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion

        #region Mst Report
        public async Task<List<MstReport>> GetAllReport(string Clause)
        {
            List<MstReport> oList = new List<MstReport>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstReport>(Clause);
                    oList = _dapper.SelectQueryList<MstReport>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(MstReport oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(async () =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();
                        string xmlData = _dapper.ConvertToXml(oModel);
                        string Query = oQuery.FormatMergeQuery<MstReport>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstReport>(Query, xmlData);
                        if (oModel.IsLayout != true && oModel.IsDelete == false)
                        {
                            MstMenu oModelMenu = new MstMenu();
                            string Clause = $@"and ""ReportCode"" = '{oModel.ReportCode}'";
                            var Menu = await GetAllMenuData(Clause);
                            if (Menu?.Count() > 0)
                            {
                                oModelMenu.Id = Menu.FirstOrDefault().Id;
                                oModelMenu.UpdatedBy = oModel.UpdatedBy;
                            }
                            oModelMenu.SortNum = 1;
                            oModelMenu.MenuParent = 1021;
                            oModelMenu.MenuParentName = "Report";
                            oModelMenu.MenuName = oModel.ReportName;
                            oModelMenu.ReportCode = oModel.ReportCode;
                            oModelMenu.MenuLink = oModel.FilePath;
                            oModelMenu.IsReport = true;
                            oModelMenu.IsActive = oModel.IsActive;
                            oModelMenu.AddedBy = oModel.AddedBy;
                            oModelMenu.Uno = 0;
                            await Crud(oModelMenu);
                        }
                    }
                    if (response.Id > 0)
                    {
                        response.Id = 1;
                        response.Message = "Saved successfully";
                    }
                    else
                    {
                        response.Id = 0;
                        response.Message = "Failed to Save successfully";
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }
        public async Task<APIResponseModel> Crud(List<MstReport> oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();
                        string xmlData = _dapper.ConvertToXml(oModel);
                        string Query = oQuery.FormatMergeQuery<MstReport>(true, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstReport>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        response.Id = 1;
                        response.Message = "Saved successfully";
                    }
                    else
                    {
                        response.Id = 0;
                        response.Message = "Failed to Save successfully";
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }
        public async Task<APIResponseModel> DeleteReport(int ID, string UserCode, string ReportCode)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = $@"Delete from ""MstReport"" Where ""ID"" = {ID} AND ""ReportCode"" = '{ReportCode}';
                                      Delete from ""MstMenu"" Where ""ReportCode"" = '{ReportCode}'";
                    response.Id = _dapper.CRUDQuery(Query);
                    response.Id = 1;
                    response.Message = "Saved successfully";
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion    

    }
}
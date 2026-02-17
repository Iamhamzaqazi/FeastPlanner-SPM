using API.DapperDAL;
using AppDBContext.General;
using AppDBContext.Interfaces.Business;
using AppDBContext.Interfaces.Dapper;
using AppDBContext.Models;
using AppDBContext.VMModels;

namespace API.Repository.Business
{

    public class BusinessDataRepo : IBusinessData
    {
        private IDapper _dapper;

        public BusinessDataRepo(IDapper dapper)
        {
            _dapper = dapper;
        }

        #region Mst Business
        public async Task<List<MstBusiness>> GetAllBusinessData(string Clause)
        {
            List<MstBusiness> oList = new List<MstBusiness>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstBusiness>(Clause);
                    oList = _dapper.SelectQueryList<MstBusiness>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(MstBusiness oModel)
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
                        string Query = oQuery.FormatMergeQuery<MstBusiness>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstBusiness>(Query, xmlData);
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

        #region Mst BusinessLog
        public async Task<List<MstBusinessLog>> GetAllBusinessLogData(string Clause)
        {
            List<MstBusinessLog> oList = new List<MstBusinessLog>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstBusinessLog>(Clause);
                    oList = _dapper.SelectQueryList<MstBusinessLog>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(MstBusinessLog oModel)
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
                        string Query = oQuery.FormatMergeQuery<MstBusinessLog>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstBusinessLog>(Query, xmlData);
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

    }
}
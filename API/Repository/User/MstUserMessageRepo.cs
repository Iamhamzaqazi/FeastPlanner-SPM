using API.Authentication;
using API.DapperDAL;
using AppDBContext.General;
using AppDBContext.Interfaces.Authentication;
using AppDBContext.Interfaces.Dapper;
using AppDBContext.Interfaces.User;
using AppDBContext.Models;
using AppDBContext.VMModels;

namespace API.Repository.User
{
    public class MstUserMessageRepo : IMstUserMessage
    {
        private IDapper _dapper;

        public MstUserMessageRepo(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<MstUserMessage>> GetUserMessage(string Clause)
        {
            List<MstUserMessage> oList = new List<MstUserMessage>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstUserMessage>(Clause);
                    oList = _dapper.SelectQueryList<MstUserMessage>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<List<MstUserMessage>> GetUserMessageDetail(string Clause)
        {
            List<MstUserMessage> oList = new List<MstUserMessage>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstUserMessage>(Clause);
                    oList = _dapper.SelectQueryList<MstUserMessage>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(MstUserMessage oModel)
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
                        string Query = oQuery.FormatMergeQuery<MstUserMessage>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstUserMessage>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
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
                        if (oModel.Id > 0)
                        {
                            response.Message = "Failed to Update";
                        }
                        else
                        {
                            response.Message = "Failed to Saved";
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

    }
}
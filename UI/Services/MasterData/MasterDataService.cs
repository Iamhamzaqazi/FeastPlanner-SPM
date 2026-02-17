using AppDBContext.General;
using AppDBContext.Interfaces.MasterData;
using AppDBContext.Models;
using AppDBContext.VMModels;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using RestSharp;

namespace UI.Services.MasterData
{
    public class MasterDataService : IMasterData
    {
        private readonly RestClient _restClient;
        private readonly ILocalStorageService _localStorage;
        private string Token = "";
        public MasterDataService(ILocalStorageService localStorage)
        {
            _restClient = new RestClient(UIConfig.APIBaseURL);
            _localStorage = localStorage;
        }
        public async Task<string> GetToken()
        {
            Token = await _localStorage.GetItemAsync<string>("UserAuthenticatedToken");
            return Token;
        }

        #region Mst Menu
        public async Task<List<MstMenu>> GetAllMenuData(string Clause)
        {
            try
            {
                List<MstMenu> oList = new List<MstMenu>();

                var request = new RestRequest("MasterData/getAllMenuClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<MstMenu>>(request);

                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                return null;
            }
        }
        public async Task<APIResponseModel> Crud(MstMenu oMstMenu)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("MasterData/crudMenu", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oMstMenu);
                var res = await _restClient.ExecuteAsync(request);
                var contentObj = JsonConvert.DeserializeObject<APIResponseModel>(res.Content);
                response.Id = contentObj.Id;
                response.Message = contentObj.Message;
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsUI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion        

        #region Mst Form
        public async Task<List<MstForm>> GetAllFormData(string Clause)
        {
            try
            {
                List<MstForm> oList = new List<MstForm>();

                var request = new RestRequest("MasterData/getAllFormClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<MstForm>>(request);

                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                return null;
            }
        }
        public async Task<APIResponseModel> Crud(MstForm oMstForm)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("MasterData/crudForm", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oMstForm);
                var res = await _restClient.ExecuteAsync(request);
                var contentObj = JsonConvert.DeserializeObject<APIResponseModel>(res.Content);
                response.Id = contentObj.Id;
                response.Message = contentObj.Message;
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsUI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion

        #region Mst Area
        public async Task<List<MstArea>> GetAllAreaData(string Clause)
        {
            try
            {
                List<MstArea> oList = new List<MstArea>();

                var request = new RestRequest("MasterData/getAllAreaClause", Method.Get) { RequestFormat = DataFormat.Json };
                //request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<MstArea>>(request);

                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                return null;
            }
        }
        public async Task<APIResponseModel> Crud(MstArea oMstArea)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("MasterData/crudArea", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oMstArea);
                var res = await _restClient.ExecuteAsync(request);
                var contentObj = JsonConvert.DeserializeObject<APIResponseModel>(res.Content);
                response.Id = contentObj.Id;
                response.Message = contentObj.Message;
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsUI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion

        #region Mst City
        public async Task<List<MstCity>> GetAllCityData(string Clause)
        {
            try
            {
                List<MstCity> oList = new List<MstCity>();

                var request = new RestRequest("MasterData/getAllCityClause", Method.Get) { RequestFormat = DataFormat.Json };
                //request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<MstCity>>(request);

                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                return null;
            }
        }
        public async Task<APIResponseModel> Crud(MstCity oMstCity)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("MasterData/crudCity", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oMstCity);
                var res = await _restClient.ExecuteAsync(request);
                var contentObj = JsonConvert.DeserializeObject<APIResponseModel>(res.Content);
                response.Id = contentObj.Id;
                response.Message = contentObj.Message;
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsUI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion

        #region Default Value
        public async Task<List<CfgDefaultValue>> GetAllDefaultValueData(string Clause)
        {
            try
            {
                List<CfgDefaultValue> oList = new List<CfgDefaultValue>();

                var request = new RestRequest("MasterData/getAllDefaultValueClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<CfgDefaultValue>>(request);

                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                return null;
            }
        }

        #endregion     

        #region Mst Facility
        public async Task<List<MstFacility>> GetAllFacilityData(string Clause)
        {
            try
            {
                List<MstFacility> oList = new List<MstFacility>();

                var request = new RestRequest("MasterData/getAllFacilityClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<MstFacility>>(request);

                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                return null;
            }
        }
        public async Task<APIResponseModel> Crud(MstFacility oMstFacility)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("MasterData/crudFacility", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oMstFacility);
                var res = await _restClient.ExecuteAsync(request);
                var contentObj = JsonConvert.DeserializeObject<APIResponseModel>(res.Content);
                response.Id = contentObj.Id;
                response.Message = contentObj.Message;
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsUI.GenerateLogs(ex);
            }
            return response;
        }

        #endregion

        #region Mst Report
        public async Task<List<MstReport>> GetAllReport(string Clause)
        {
            try
            {
                List<MstReport> oList = new List<MstReport>();

                var request = new RestRequest("MasterData/getAllReportSetupClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<MstReport>>(request);

                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                return null;
            }
        }
        public async Task<APIResponseModel> Crud(MstReport oMstReport)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("MasterData/crudReportSetup", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oMstReport);
                var res = await _restClient.ExecuteAsync(request);
                var contentObj = JsonConvert.DeserializeObject<APIResponseModel>(res.Content);
                response.Id = contentObj.Id;
                response.Message = contentObj.Message;
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsUI.GenerateLogs(ex);
            }
            return response;
        }
        public async Task<APIResponseModel> Crud(List<MstReport> oMstReport)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("MasterData/crudListReportSetup", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oMstReport);
                var res = await _restClient.ExecuteAsync(request);
                var contentObj = JsonConvert.DeserializeObject<APIResponseModel>(res.Content);
                response.Id = contentObj.Id;
                response.Message = contentObj.Message;
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsUI.GenerateLogs(ex);
            }
            return response;
        }
        public async Task<APIResponseModel> DeleteReport(int ID, string UserCode, string ReportCode)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest($"MasterData/DeleteReport?ID={ID}&UserCode={UserCode}&ReportCode={ReportCode}", Method.Get);
                request.AddHeader("Authorization", await GetToken());
                var res = await _restClient.ExecuteAsync(request);
                if (res.IsSuccessful)
                {
                    response.Id = 1;
                    response.Message = "Update successfully";
                    return response;
                }
                else
                {
                    response.Id = 0;
                    response.Message = "Failed to Update successfully";
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                response.Id = 0;
                response.Message = "Failed to Update successfully";
                return response;
            }
        }

        #endregion 
    }
}
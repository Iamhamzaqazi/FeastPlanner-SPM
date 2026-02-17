using AppDBContext.General;
using AppDBContext.Interfaces.User;
using AppDBContext.Models;
using AppDBContext.VMModels;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using RestSharp;

namespace UI.Services.User
{
    public class CfgUserService : ICfgUser
    {
        private readonly RestClient _restClient;
        private readonly ILocalStorageService _localStorage;
        private string Token = "";

        public CfgUserService(ILocalStorageService localStorage)
        {
            _restClient = new RestClient(UIConfig.APIBaseURL);
            _localStorage = localStorage;
        }
        public async Task<string> GetToken()
        {
            Token = await _localStorage.GetItemAsync<string>("UserAuthenticatedToken");
            return Token;
        }
        public async Task<List<CfgContactVerification>> GetAllContactVerificationDataByClause(string Clause)
        {
            try
            {
                List<CfgContactVerification> oList = new List<CfgContactVerification>();

                var request = new RestRequest("UserData/getAllContactVerificationDataByUser", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddQueryParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<CfgContactVerification>>(request);

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
        public async Task<APIResponseModel> Crud(CfgContactVerification oCfgContactVerification)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("UserData/crudCfgContactVerification", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oCfgContactVerification);
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

        public async Task<List<CfgEmailVerification>> GetAllEmailVerificationDataByClause(string Clause)
        {
            try
            {
                List<CfgEmailVerification> oList = new List<CfgEmailVerification>();

                var request = new RestRequest("UserData/getAllEmailVerificationDataByUser", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddQueryParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<CfgEmailVerification>>(request);

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
        public async Task<APIResponseModel> Crud(CfgEmailVerification oCfgEmailVerification)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("UserData/crudCfgEmailVerification", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oCfgEmailVerification);
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

        public async Task<List<CfgTwoFa>> GetAllTwoFADataByClause(string Clause)
        {
            try
            {
                List<CfgTwoFa> oList = new List<CfgTwoFa>();

                var request = new RestRequest("UserData/getAllTwoFADataByClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddQueryParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<CfgTwoFa>>(request);

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
        public async Task<APIResponseModel> Crud(CfgTwoFa oCfgTwoFA)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("UserData/crudCfgTwoFA", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oCfgTwoFA);
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

        public async Task<List<VMUserEmailNotificationPreference>> GetAllVMPreferencesDataByClause(string Clause)
        {
            try
            {
                List<VMUserEmailNotificationPreference> oList = new List<VMUserEmailNotificationPreference>();

                var request = new RestRequest("UserData/getAllVMPreferencesDataByClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddQueryParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<VMUserEmailNotificationPreference>>(request);

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
        public async Task<List<CfgEmailNotificationPreference>> GetAllPreferencesDataByClause(string Clause)
        {
            try
            {
                List<CfgEmailNotificationPreference> oList = new List<CfgEmailNotificationPreference>();

                var request = new RestRequest("UserData/getAllPreferencesDataByClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddQueryParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<CfgEmailNotificationPreference>>(request);

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
        public async Task<APIResponseModel> Crud(CfgEmailNotificationPreference oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("UserData/crudEmailNotificationPreference", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oModel);
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
        public async Task<APIResponseModel> Crud(List<CfgEmailNotificationPreference> oList)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("UserData/crudListEmailNotificationPreference", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oList);
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

        public async Task<CfgThemeMode> GetThemeSettingDataByClause(string Clause)
        {
            try
            {
                CfgThemeMode oCfgThemeMode = new CfgThemeMode();

                var request = new RestRequest("UserData/getCfgThemeModeDataByClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddQueryParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<CfgThemeMode>(request);

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
        public async Task<APIResponseModel> Crud(CfgThemeMode oCfgThemeMode)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("UserData/crudCfgThemeMode", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oCfgThemeMode);
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
    }
}
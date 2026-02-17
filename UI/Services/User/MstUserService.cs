using AppDBContext.General;
using AppDBContext.Interfaces.User;
using AppDBContext.Models;
using AppDBContext.VMModels;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using RestSharp;

namespace UI.Services.User
{
    public class MstUserService : IMstUser
    {
        private readonly RestClient _restClient;
        private readonly ILocalStorageService _localStorage;
        private string Token = "";

        public MstUserService(ILocalStorageService localStorage)
        {
            _restClient = new RestClient(UIConfig.APIBaseURL);
            _localStorage = localStorage;
        }
        public async Task<string> GetToken()
        {
            Token = await _localStorage.GetItemAsync<string>("UserAuthenticatedToken");
            return Token;
        }
        public async Task<List<MstUser>> GetAllData(string Clause)
        {
            try
            {
                var request = new RestRequest("UserData/getAllUserClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<MstUser>>(request);

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
        public async Task<APIResponseModel> Crud(MstUser oMstUser)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("UserData/crudUser", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oMstUser);
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
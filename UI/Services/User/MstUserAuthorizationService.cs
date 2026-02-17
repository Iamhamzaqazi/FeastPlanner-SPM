using AppDBContext.General;
using AppDBContext.Interfaces.User;
using AppDBContext.Models;
using AppDBContext.VMModels;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using RestSharp;

namespace UI.Services.User
{
    public class MstUserAuthorizationService : IMstUserAuthorization
    {
        private readonly RestClient _restClient;
        private readonly ILocalStorageService _localStorage;
        private string Token = "";

        public MstUserAuthorizationService(ILocalStorageService localStorage)
        {
            _restClient = new RestClient(UIConfig.APIBaseURL);
            _localStorage = localStorage;
        }
        public async Task<string> GetToken()
        {
            Token = await _localStorage.GetItemAsync<string>("UserAuthenticatedToken");
            return Token;
        }
        public async Task<List<VMMstUserAuthorization>> GetAllData(string Clause, bool IsSuper)
        {
            try
            {
                List<VMMstUserAuthorization> oList = new List<VMMstUserAuthorization>();

                var request = new RestRequest("UserData/getAllUserAuthorizationClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                request.AddParameter("IsSuper", IsSuper);
                var response = await _restClient.ExecuteAsync<List<VMMstUserAuthorization>>(request);

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
        public async Task<APIResponseModel> Crud(List<MstUserAuthorization> oMstUserAuthorization)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("UserData/crudUserAuthorization", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oMstUserAuthorization);
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

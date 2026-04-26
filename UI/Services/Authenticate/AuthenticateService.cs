using AppDBContext.Interfaces.Authentication;
using AppDBContext.VMModels;
using Newtonsoft.Json;
using RestSharp;

namespace UI.Services.Authenticate
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly RestClient _restClient;
        public AuthenticateService()
        {
            _restClient = new RestClient(UIConfig.APIBaseURL);
        }
        public async Task<List<MstUser>> VerifyUser(string Clause)
        {
            try
            {
                var request = new RestRequest("Authenticate/verifyUser", Method.Get) { RequestFormat = DataFormat.Json };
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
        public async Task<APIResponseModel> SignUp(SignUpRequest oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("Authenticate/signUp", Method.Post);
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
        public async Task<MstUser> Login(MstUser oMstUser)
        {
            try
            {
                var request = new RestRequest("Authenticate/login", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddBody(oMstUser);
                var response = await _restClient.ExecuteAsync<MstUser>(request);

                if (response.IsSuccessful && !string.IsNullOrWhiteSpace(response.Data.UpdatedWs))
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
        public async Task<APIResponseModel> CheckEmail(string Email)
        {
            try
            {
                var request = new RestRequest("Authenticate/checkEmail", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddQueryParameter("Email", Email);
                var response = await _restClient.ExecuteAsync<APIResponseModel>(request);

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
        public async Task<APIResponseModel> CheckContact(string Contact)
        {
            try
            {
                var request = new RestRequest("Authenticate/checkContact", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddQueryParameter("Contact", Contact);
                var response = await _restClient.ExecuteAsync<APIResponseModel>(request);

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
        public async Task<List<UserPasswordRequest>> GetAllUserPasswordDataByClause(string Clause)
        {
            try
            {
                var request = new RestRequest("Authenticate/getAllUserPasswordRequestByClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<UserPasswordRequest>>(request);

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
        public async Task<APIResponseModel> Crud(UserPasswordRequest oUserPasswordRequest)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("Authenticate/crudUserPasswordRequest", Method.Post);
                request.AddJsonBody(oUserPasswordRequest);
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
        public async Task<APIResponseModel> ChangePassword(MstUser oMstUser)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("Authenticate/changePassword", Method.Post);
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

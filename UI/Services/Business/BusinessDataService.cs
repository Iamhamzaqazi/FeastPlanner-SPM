using AppDBContext.Interfaces.Business;
using AppDBContext.VMModels;
using Newtonsoft.Json;
using RestSharp;

namespace UI.Services.Business
{
    public class BusinessDataService : IBusinessData
    {
        private readonly RestClient _restClient;
        private readonly ILocalStorageService _localStorage;
        private string Token = "";
        public BusinessDataService(ILocalStorageService localStorage)
        {
            _restClient = new RestClient(UIConfig.APIBaseURL);
            _localStorage = localStorage;
        }
        public async Task<string> GetToken()
        {
            Token = await _localStorage.GetItemAsync<string>("UserAuthenticatedToken");
            return Token;
        }


        #region Mst Business
        public async Task<List<MstBusiness>> GetAllBusinessData(string Clause)
        {
            try
            {
                List<MstBusiness> oList = new List<MstBusiness>();

                var request = new RestRequest("BusinessData/getAllBusinessClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<MstBusiness>>(request);

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
        public async Task<APIResponseModel> Crud(MstBusiness oMstBusiness)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("BusinessData/crudBusiness", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oMstBusiness);
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

        #region Mst BusinessLog
        public async Task<List<MstBusinessLog>> GetAllBusinessLogData(string Clause)
        {
            try
            {
                List<MstBusinessLog> oList = new List<MstBusinessLog>();

                var request = new RestRequest("BusinessData/getAllBusinessLogClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<MstBusinessLog>>(request);

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
        public async Task<APIResponseModel> Crud(MstBusinessLog oMstBusinessLog)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("BusinessData/crudBusinessLog", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oMstBusinessLog);
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
    }
}
using AppDBContext.Interfaces.Booking;
using AppDBContext.VMModels;
using Newtonsoft.Json;
using RestSharp;

namespace UI.Services.Booking
{
    public class BookingDataService : IBooking
    {
        private readonly RestClient _restClient;
        private readonly ILocalStorageService _localStorage;
        private string Token = "";
        public BookingDataService(ILocalStorageService localStorage)
        {
            _restClient = new RestClient(UIConfig.APIBaseURL);
            _localStorage = localStorage;
        }
        public async Task<string> GetToken()
        {
            Token = await _localStorage.GetItemAsync<string>("UserAuthenticatedToken");
            return Token;
        }

        #region Trns BusinessBooking
        public async Task<List<TrnsBusinessBooking>> GetAllBusinessBookingData(string Clause)
        {
            try
            {
                List<TrnsBusinessBooking> oList = new List<TrnsBusinessBooking>();

                var request = new RestRequest("BookingData/getAllBusinessBookingClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<TrnsBusinessBooking>>(request);

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
        public async Task<List<TrnsBusinessBookingDetail>> GetAllBusinessBookingDetailData(string Clause)
        {
            try
            {
                List<TrnsBusinessBookingDetail> oList = new List<TrnsBusinessBookingDetail>();

                var request = new RestRequest("BookingData/getAllBusinessBookingDetailClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<TrnsBusinessBookingDetail>>(request);

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
        public async Task<List<TrnsBusinessBookingPayment>> GetAllBusinessBookingPaymentData(string Clause)
        {
            try
            {
                List<TrnsBusinessBookingPayment> oList = new List<TrnsBusinessBookingPayment>();

                var request = new RestRequest("BookingData/getAllBusinessBookingPaymentClause", Method.Get) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", await GetToken());
                request.AddParameter("Clause", Clause);
                var response = await _restClient.ExecuteAsync<List<TrnsBusinessBookingPayment>>(request);

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
        public async Task<APIResponseModel> Crud(TrnsBusinessBooking oTrnsBusinessBooking)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                var request = new RestRequest("BookingData/crudBusinessBooking", Method.Post);
                request.AddHeader("Authorization", await GetToken());
                request.AddJsonBody(oTrnsBusinessBooking);
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
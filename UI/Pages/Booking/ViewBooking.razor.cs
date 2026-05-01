using AppDBContext.Models;
using System.Threading.Tasks;

namespace UI.Pages.Booking
{
    public partial class ViewBooking
    {
        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        private bool loading = false;
        private bool IsSet = false;

        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";


        private string searchString1 = "";
        private bool FilterFuncCustomer(MstBusinessCustomer element) => FilterFuncCustomer(element, searchString1);
        private IEnumerable<MstBusinessCustomer> oListCustomer = new List<MstBusinessCustomer>();
        private IEnumerable<TrnsBusinessBooking> oListBooking = new List<TrnsBusinessBooking>();
        private IEnumerable<TrnsBusinessBookingDetail> oListBookingDetail = new List<TrnsBusinessBookingDetail>();

        #endregion

        #region Functions
        private async Task CallAPI()
        {
            try
            {
                await GetAllBusinessCustomers();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private bool FilterFuncCustomer(MstBusinessCustomer element, string searchString1)
        {
            if (string.IsNullOrWhiteSpace(searchString1))
                return true;
            if (element.CustomerName.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.CustomerEmail.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.CustomerContact.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.CustomerNIC.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.IsActive.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.IsBookingCompleted.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        public void EditRecord(string Key)
        {
            try
            {
                Navigation.NavigateTo($"AddBooking/{Key}", true);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task ViewDetail(string CustomerKey)
        {
            MstBusinessCustomer tmpPerson = oListCustomer.FirstOrDefault(x => x.UniqueKey == CustomerKey);

            string Clause = $@" AND BusinessKey = '{BusinessKey}' and CustomerKey = '{CustomerKey}'";
            oListBookingDetail = await _trnsBooking.GetAllBusinessBookingDetailData(Clause);

            tmpPerson.IsShow = !tmpPerson.IsShow;
        }
        private async Task GetAllBusinessCustomers()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}' and IsBookingCompleted = 'True'";
                oListCustomer = await _masterData.GetAllBusinessCustomerData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }

        //private async Task Search()
        //{
        //    try
        //    {
        //        loading = true;
        //        await formUser.Validate();
        //        if (successUser)
        //        {
        //            await GetAllExpense();
        //            if (oFilterList.Count() > 0)
        //            {
        //                oModel.Amount = oFilterList.Sum(x => x.Amount);
        //                Snackbar.Add("Data found against provided Date range.", Severity.Info);
        //            }
        //            else
        //            {
        //                Snackbar.Add("No data found against provided Date range.", Severity.Error);
        //            }
        //        }
        //        successUser = false;
        //        loading = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogsUI.GenerateLogs(ex);
        //    }
        //    await Task.CompletedTask;
        //}
        private async Task Refresh()
        {
            try
            {
                await Task.Delay(1);
                Navigation.NavigateTo(Navigation.Uri, true);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }

        #endregion

        #region Events

        protected async override Task OnInitializedAsync()
        {
            try
            {
                loading = true;
                //DateRange = new DateRange(DateTime.Now.Date, DateTime.Now.Date);
                var authState = await _authState.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity.IsAuthenticated)
                {
                    LoggedInUser = user.Claims.Where(x => x.Type == "Username").Select(x => x.Value).FirstOrDefault();
                    UserKey = user.Claims.Where(x => x.Type == "UserKey").Select(x => x.Value).FirstOrDefault();
                    BusinessKey = user.Claims.Where(x => x.Type == "BusinessKey").Select(x => x.Value).FirstOrDefault();
                    await CallAPI();
                }
                else
                {
                    Navigation.NavigateTo(Navigation.BaseUri + "/SignIn");
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                loading = false;
            }
        }

        #endregion
    }
}
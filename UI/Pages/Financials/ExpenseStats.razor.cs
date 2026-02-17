namespace UI.Pages.Financials
{
    public partial class ExpenseStats
    {
        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        private bool loading = false;
        private bool IsSet = false;

        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";


        private DateRange? DateRange;


        bool successUser;
        string[] errors = { };
        MudForm formUser;

        TrnsBusinessExpense oModel = new TrnsBusinessExpense();

        private IEnumerable<TrnsBusinessExpense> oList = new List<TrnsBusinessExpense>();
        private IEnumerable<TrnsBusinessExpense> oFilterList = new List<TrnsBusinessExpense>();

        private string searchString1 = "";
        private bool FilterFunc(TrnsBusinessExpense element) => FilterFunc(element, searchString1);

        #endregion

        #region Functions
        private bool FilterFunc(TrnsBusinessExpense element, string searchString1)
        {
            if (string.IsNullOrWhiteSpace(searchString1))
                return true;
            if (element.Description.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Amount.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Date.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Comments.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.AddedBy.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        private async Task GetAllExpense()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}' AND Date Between  '{Convert.ToDateTime(DateRange.Start).ToString("yyyy-MM-dd")}'
                                                                                    AND '{Convert.ToDateTime(DateRange.End).ToString("yyyy-MM-dd")}'
                                                                                    AND IsActive = 'True'";
                oFilterList = await _trnsFinancial.GetAllExpenseData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task Search()
        {
            try
            {
                loading = true;
                await formUser.Validate();
                if (successUser)
                {
                    await GetAllExpense();
                    if (oFilterList.Count() > 0)
                    {
                        oModel.Amount = oFilterList.Sum(x => x.Amount);
                        Snackbar.Add("Data found against provided Date range.", Severity.Info);
                    }
                    else
                    {
                        Snackbar.Add("No data found against provided Date range.", Severity.Error);
                    }
                }
                successUser = false;
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            await Task.CompletedTask;
        }
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
                DateRange = new DateRange(DateTime.Now.Date, DateTime.Now.Date);
                var authState = await _authState.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity.IsAuthenticated)
                {
                    LoggedInUser = user.Claims.Where(x => x.Type == "Username").Select(x => x.Value).FirstOrDefault();
                    UserKey = user.Claims.Where(x => x.Type == "UserKey").Select(x => x.Value).FirstOrDefault();
                    BusinessKey = user.Claims.Where(x => x.Type == "BusinessKey").Select(x => x.Value).FirstOrDefault();
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
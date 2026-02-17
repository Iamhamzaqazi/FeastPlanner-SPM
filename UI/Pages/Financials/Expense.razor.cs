using AppDBContext.VMModels;

namespace UI.Pages.Financials
{
    public partial class Expense
    {
        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        private bool loading = false;
        private bool IsSet = false;

        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";

        bool successUser;
        string[] errors = { };
        MudForm formUser;

        TrnsBusinessExpense oModel = new TrnsBusinessExpense();

        private IEnumerable<TrnsBusinessExpense> oList = new List<TrnsBusinessExpense>();

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
            if (element.IsActive.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        public void EditRecord(int Id)
        {
            try
            {
                var res = oList.Where(x => x.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    oModel = res;
                    //oList = oList.Where(x => x.Id != Id);
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task CallAPI()
        {
            try
            {
                await GetAllExpense();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetAllExpense()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}'";
                oList = await _trnsFinancial.GetAllExpenseData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        public void SetModelValues()
        {
            try
            {
                if (oModel.Id == 0)
                {
                    oModel.BusinessKey = BusinessKey;
                    oModel.AddedBy = LoggedInUser;                    
                    IsSet = true;
                }
                else
                {
                    oModel.BusinessKey = BusinessKey;
                    oModel.UpdatedBy = LoggedInUser;                    
                    IsSet = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }        
        private async Task<APIResponseModel> Save()
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;
                SetModelValues();
                await formUser.Validate();
                if (IsSet && successUser)
                {
                    res = await _trnsFinancial.Crud(oModel);
                    if (res.Id > 0)
                    {
                        Snackbar.Add(res.Message, Severity.Success);
                        Navigation.NavigateTo(Navigation.Uri, true);
                    }
                    else
                    {
                        Snackbar.Add(res.Message, Severity.Error);
                    }
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return res;
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
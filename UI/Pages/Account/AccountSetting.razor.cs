using AppDBContext.VMModels;
using FluentValidation;
using UI.Authentication;
using UI.Pages.Components;

namespace UI.Pages.Account
{
    public partial class AccountSetting
    {

        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        private bool loading = false;
        private bool SameData = false;
        private bool Edit = false;
        private bool EmailExist = false;
        private bool ContactExist = false;
        private bool IsSet = false;

        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";

        private int AccountCompletion = 0;
        private double AccountCompletionPercentage = 0;

        private string LoggedInUserEmail = "";
        private string LoggedInUserContact = "";
        private string LoggedInUserPassword = "";
        private string BusinessName = "";
        private string DialogFor = "";

        bool successUser;
        string[] errors = { };
        MudForm formUser;

        FluentValueValidator<string> cnValidator = new FluentValueValidator<string>(x => x
       .NotEmpty()
       .Length(1, 100)
       .Matches("^((\\+92)|(0092))-{0,1}\\d{3}-{0,1}\\d{7}$|^\\d{11}$|^\\d{4}-\\d{7}$")
       .WithMessage("Contact format incorrect"));

        MstUser oModel = new MstUser();

        CfgContactVerification oModelCfgContactVerification = new CfgContactVerification();

        CfgEmailVerification oModelCfgEmailVerification = new CfgEmailVerification();

        List<CfgEmailNotificationPreference> oListCfgEmailNotificationPreference = new List<CfgEmailNotificationPreference>();
        private IEnumerable<VMUserEmailNotificationPreference> oVMUserEmailNotificationPreferencesList = new List<VMUserEmailNotificationPreference>();
        MudTable<VMUserEmailNotificationPreference> TableRef { get; set; }

        CfgTwoFa oModelCfgTwoFa = new CfgTwoFa();



        #endregion

        #region Functions

        public class FluentValueValidator<T> : AbstractValidator<T>
        {
            public FluentValueValidator(Action<IRuleBuilderInitial<T, T>> rule)
            {
                rule(RuleFor(x => x));
            }

            private IEnumerable<string> ValidateValue(T arg)
            {
                if (arg == null)
                    return new string[0];
                var result = Validate(arg);
                if (result.IsValid)
                    return new string[0];
                return result.Errors.Select(e => e.ErrorMessage);
            }

            public Func<T, IEnumerable<string>> Validation => ValidateValue;
        }
        private async Task OpenDeactivateDialog(DialogOptions options)
        {
            try
            {
                DialogFor = "Deactivate Account";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>("Deactivate Account", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    var oUser = (MstUser)result.Data;
                    oUser.IsActive = false;
                    oUser.UpdatedBy = LoggedInUser;
                    var res = await _mstUser.Crud(oUser);
                    if (res.Id == 1)
                    {
                        Snackbar.Add("Account Deactivated", MudBlazor.Severity.Success);
                        await LocalStorage.RemoveItemAsync("UserAuthenticatedToken");
                        ((AuthStateProvider)_authState).NotifyUserLogout();
                        Navigation.NavigateTo(Navigation.BaseUri + "/SignIn");
                    }
                    else
                    {
                        Snackbar.Add(res.Message, MudBlazor.Severity.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task CallAPI()
        {
            try
            {
                await GetUser();
                await GetUserConfiguration();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetUser()
        {
            try
            {
                string Clause = $@" AND UniqueKey = '{UserKey}'";
                var obj = await _mstUser.GetAllData(Clause);

                if (obj?.Count() > 0)
                {
                    oModel = obj.FirstOrDefault();
                }

                if (!string.IsNullOrWhiteSpace(oModel.Name))
                {
                    AccountCompletion += 1;
                }
                if (!string.IsNullOrWhiteSpace(oModel.Email))
                {
                    AccountCompletion += 1;
                }
                if (!string.IsNullOrWhiteSpace(oModel.Contact))
                {
                    AccountCompletion += 1;
                }
                if (oModel.IsEmailVerify)
                {
                    AccountCompletion += 1;
                }
                if (oModel.IsContactVerify)
                {
                    AccountCompletion += 1;
                }
                if (oModel.IsOtpenable)
                {
                    AccountCompletion += 1;
                }
                AccountCompletionPercentage = (Convert.ToDouble(AccountCompletion) / Convert.ToDouble(UIConfig.TotalAccountCompletion)) * 100;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetUserConfiguration()
        {
            try
            {
                string Clause = $@" AND UserKey = '{UserKey}' and IsActive = 'True'";
                var objContact = await _cfgUser.GetAllContactVerificationDataByClause(Clause);
                if (objContact?.Count() > 0)
                {
                    oModelCfgContactVerification = objContact.FirstOrDefault();
                }

                var objEmail = await _cfgUser.GetAllEmailVerificationDataByClause(Clause);
                if (objEmail?.Count() > 0)
                {
                    oModelCfgEmailVerification = objEmail.FirstOrDefault();
                }

                var objTwoFA = await _cfgUser.GetAllTwoFADataByClause(Clause);
                if (objTwoFA?.Count() > 0)
                {
                    oModelCfgTwoFa = objTwoFA.FirstOrDefault();
                }

                oVMUserEmailNotificationPreferencesList = await _cfgUser.GetAllVMPreferencesDataByClause(UserKey);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }

        private async Task<bool> CheckEmail()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(oModel.Email))
                {
                    var res = await _authenticate.CheckEmail(oModel.Email);
                    if (res.Id == 0)
                    {
                        EmailExist = false;
                    }
                    else
                    {
                        EmailExist = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return EmailExist;
        }
        private async Task<bool> CheckContact()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(oModel.Contact))
                {
                    var res = await _authenticate.CheckContact(oModel.Contact);
                    if (res.Id == 0)
                    {
                        ContactExist = false;
                    }
                    else
                    {
                        ContactExist = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return ContactExist;
        }
        public async Task SetModelValues()
        {
            try
            {
                if (LoggedInUserEmail == oModel.Email)
                {
                    IsSet = true;
                    SameData = true;
                    oModel.Email = LoggedInUserEmail;
                }
                else
                {
                    await CheckEmail();
                    if (!EmailExist)
                    {
                        IsSet = true;
                        oModel.IsEmailVerify = false;
                    }
                    else
                    {
                        Snackbar.Add("Email already in used for account.", MudBlazor.Severity.Error);
                        IsSet = false;
                    }
                }
                if (LoggedInUserContact == oModel.Contact)
                {
                    oModel.Contact = LoggedInUserContact;
                    SameData = true;
                    IsSet = true;
                }
                else
                {
                    await CheckContact();
                    if (!ContactExist)
                    {
                        IsSet = true;
                        oModel.IsOtpenable = false;
                    }
                    else
                    {
                        Snackbar.Add("Contact already in used for account.", MudBlazor.Severity.Error);
                        IsSet = false;
                    }
                }
                oModel.UpdatedBy = LoggedInUser;

            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task OpenVerifyEmailDialog(DialogOptions options)
        {
            try
            {
                DialogFor = "Verify Email";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>(DialogFor, parameters, options);
                var result = await dialog.Result;

                if (!result.Canceled)
                {
                    oModel.IsEmailVerify = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task OpenVerifyContactDialog(DialogOptions options)
        {
            try
            {
                DialogFor = "Verify Contact";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>(DialogFor, parameters, options);
                var result = await dialog.Result;

                if (!result.Canceled)
                {
                    oModel.IsContactVerify = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task Open2FADialog(DialogOptions options)
        {
            try
            {
                DialogFor = "Two Factor Authentication";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>("", parameters, options);
                var result = await dialog.Result;

                if (!result.Canceled)
                {
                    oModel.IsOtpenable = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task Open2FADialogDisable(DialogOptions options)
        {
            try
            {
                DialogFor = "Disbale Two Factor Authentication";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>(DialogFor, parameters, options);
                var result = await dialog.Result;

                if (!result.Canceled)
                {
                    oModel.IsOtpenable = false;
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
                await SetModelValues();
                await formUser.Validate();
                if (IsSet && successUser)
                {
                    res = await _mstUser.Crud(oModel);
                    if (res.Id > 0)
                    {
                        Snackbar.Add(res.Message, MudBlazor.Severity.Success);
                        if (!SameData)
                        {
                            await LocalStorage.RemoveItemAsync("UserAuthenticatedToken");
                            ((AuthStateProvider)_authState).NotifyUserLogout();
                            Snackbar.Add("Login again!", MudBlazor.Severity.Info);
                            Navigation.NavigateTo("SignIn");
                        }
                    }
                    else
                    {
                        Snackbar.Add(res.Message, MudBlazor.Severity.Error);
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

        public async Task SetModelValuesEmailNotificationPreferences()
        {
            try
            {
                IsSet = false;
                await Task.Delay(1);
                foreach (var Preferences in oVMUserEmailNotificationPreferencesList)
                {
                    CfgEmailNotificationPreference oCfgEmailNotificationPreference = new CfgEmailNotificationPreference();
                    oCfgEmailNotificationPreference.PreferenceKey = Preferences.MstPreferenceUniqueKey;
                    oCfgEmailNotificationPreference.UserKey = UserKey;
                    oCfgEmailNotificationPreference.IsEmail = Preferences.IsEmail;
                    oCfgEmailNotificationPreference.IsSms = Preferences.IsSms;
                    oCfgEmailNotificationPreference.IsAlert = Preferences.IsAlert;
                    oCfgEmailNotificationPreference.IsActive = true;
                    oCfgEmailNotificationPreference.UserRights = Preferences.UserRights;
                    oCfgEmailNotificationPreference.AddedBy = LoggedInUser;
                    oListCfgEmailNotificationPreference.Add(oCfgEmailNotificationPreference);
                }
                IsSet = true;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task<APIResponseModel> SaveEmailNotificationPreferences()
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;
                await SetModelValuesEmailNotificationPreferences();
                if (IsSet)
                {
                    res = await _cfgUser.Crud(oListCfgEmailNotificationPreference);
                    if (res.Id > 0)
                    {
                        Snackbar.Add(res.Message, MudBlazor.Severity.Success);
                        Navigation.NavigateTo(Navigation.Uri, true);
                    }
                    else
                    {
                        Snackbar.Add(res.Message, MudBlazor.Severity.Error);
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
        private async Task EditProfile()
        {
            try
            {
                await Task.Delay(1);
                Edit = true;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task CancelEdit()
        {
            try
            {
                await Task.Delay(1);
                Edit = false;
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
                    BusinessName = user.Claims.Where(x => x.Type == "BusinessName").Select(x => x.Value).FirstOrDefault();
                    LoggedInUserEmail = user.Claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
                    LoggedInUserPassword = user.Claims.Where(x => x.Type == "Password").Select(x => x.Value).FirstOrDefault();
                    LoggedInUserContact = user.Claims.Where(x => x.Type == "Contact").Select(x => x.Value).FirstOrDefault();
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
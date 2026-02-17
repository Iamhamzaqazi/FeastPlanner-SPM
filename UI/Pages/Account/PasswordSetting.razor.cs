using AppDBContext.VMModels;
using System.Text.RegularExpressions;
using UI.Authentication;

namespace UI.Pages.Account
{
    public partial class PasswordSetting
    {

        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        private bool loading = false;
        private bool PasswordVisible = false;
        private bool VisibleMethod2FA = true;
        private bool VisibleMethodPhoneNo = false;
        private bool VisibleMethodCode = false;
        private bool OTPSend = false;
        private bool OTPExpire = false;
        private bool OTPEnable = false;

        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";

        private string LoggedInUserEmail = "";
        private string LoggedInUserPassword = "";
        private string OldPassword = "";
        private string MobileNo = "";
        private string OTPCode = "";

        private static System.Timers.Timer aTimer;
        private int counter = 100;
        private string TimerMessage = "";

        bool successUser;
        string[] errors = { };
        MudTextField<string> pwField1;
        MudForm formUser;

        private IMask ContactMasking = new RegexMask(@"^\d");

        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        MstUser oModel = new MstUser();

        private IEnumerable<MstUser> oList = new List<MstUser>();
        #endregion

        #region Functions

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }
        private string PasswordMatch(string arg)
        {
            if (pwField1.Value != arg)
                return "Passwords don't match";
            return null;
        }
        void ShowPassword()
        {
            if (PasswordVisible)
            {
                PasswordVisible = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                PasswordVisible = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        private async Task CallAPI()
        {
            try
            {
                await GetUser();
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
                string Clause = $@" AND BusinessKey = '{BusinessKey}'";
                oList = await _mstUser.GetAllData(Clause);
                if (oList?.Count() > 0)
                {
                    oModel = oList.FirstOrDefault();
                    oModel.Password = "";
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }

        public async Task Next(int Index)
        {
            await Task.Delay(1);
            switch (Index)
            {
                case 0:
                    if (VisibleMethod2FA)
                    {
                        VisibleMethodPhoneNo = true;
                        VisibleMethod2FA = false;
                        _ = InvokeAsync(StateHasChanged);
                    }
                    break;
                case 1:
                    if (VisibleMethodPhoneNo)
                    {
                        VisibleMethodPhoneNo = false;
                        VisibleMethod2FA = false;
                        VisibleMethodCode = true;
                        _ = InvokeAsync(StateHasChanged);
                    }
                    break;
            }
        }
        public async Task Back(int Index)
        {
            await Task.Delay(1);
            switch (Index)
            {
                case 0:
                    if (VisibleMethodPhoneNo)
                    {
                        VisibleMethodPhoneNo = false;
                        VisibleMethod2FA = true;
                        _ = InvokeAsync(StateHasChanged);
                    }
                    break;
                case 1:
                    if (VisibleMethodCode)
                    {
                        VisibleMethodPhoneNo = true;
                        VisibleMethod2FA = false;
                        VisibleMethodCode = false;
                        _ = InvokeAsync(StateHasChanged);
                    }
                    break;
            }
        }
        public bool SetModelValuesPassword()
        {
            bool IsSet = false;
            try
            {
                if (OldPassword == LoggedInUserPassword)
                {
                    IsSet = true;
                    oModel.UpdatedBy = LoggedInUser;
                }
                else
                {
                    Snackbar.Add("Old password doesn't match.", Severity.Error);
                    IsSet = false;
                }

            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
            return IsSet;
        }

        private async Task<APIResponseModel> SavePassword()
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;
                await formUser.Validate();
                if (SetModelValuesPassword() && successUser)
                {
                    res = await _mstUser.Crud(oModel);
                    if (res.Id > 0)
                    {
                        Snackbar.Add(res.Message, Severity.Success);
                        await LocalStorage.RemoveItemAsync("UserAuthenticatedToken");
                        ((AuthStateProvider)_authState).NotifyUserLogout();
                        Snackbar.Add("Login again!", Severity.Info);
                        Navigation.NavigateTo("SignIn");
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

        private async Task<APIResponseModel> SendOTP()
        {
            var res = new APIResponseModel();
            try
            {
                await Task.Delay(1);
                loading = true;
                if (!string.IsNullOrWhiteSpace(MobileNo))
                {
                    Snackbar.Add("OTP Send", Severity.Info);
                    OTPSend = true;
                    StartTimer();
                }
                else
                {
                    Snackbar.Add("Mobile no required", Severity.Error);
                }

                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return res;
        }

        private async Task<APIResponseModel> ConfirmOTP()
        {
            var res = new APIResponseModel();
            try
            {
                await Task.Delay(1);
                loading = true;
                //if (SetModelValuesPassword())
                //{
                //    res = await _mstUser.Update(oModel);
                //    if (res.Id == 1)
                //    {
                //        Snackbar.Add(res.Message, Severity.Success);
                //        await SessionStorage.RemoveItemAsync("UserSession");
                //        ((AuthStateProvider)_oAuth).NotifyUserLogout();
                //        Snackbar.Add("Login again!", Severity.Info);
                //        Navigation.NavigateTo("SignIn");
                //    }
                //    else
                //    {
                //        Snackbar.Add(res.Message, Severity.Error);
                //    }
                //}
                if (!string.IsNullOrWhiteSpace(OTPCode))
                {
                    if (!OTPExpire)
                    {
                        Snackbar.Add("2FA enabled successfully", Severity.Info);
                        OTPEnable = true;
                    }
                    else
                    {
                        Snackbar.Add("OTP expired", Severity.Error);
                    }
                }
                else
                {
                    Snackbar.Add("OTP code required", Severity.Error);
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
                    LoggedInUserEmail = user.Claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
                    LoggedInUserPassword = user.Claims.Where(x => x.Type == "Password").Select(x => x.Value).FirstOrDefault();
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

        public void StartTimer()
        {
            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += CountDownTimer;
            aTimer.Enabled = true;
        }
        public void CountDownTimer(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (counter > 0)
            {
                counter -= 1;
                TimerMessage = "Timer: " + counter + " seconds...";
            }
            else
            {
                aTimer.Enabled = false;
                TimerMessage = "OTP Expired, request a new one.";
                OTPExpire = true;
                OTPSend = false;
            }
            _ = InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
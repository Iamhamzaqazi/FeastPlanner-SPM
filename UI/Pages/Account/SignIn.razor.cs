using System.Text.RegularExpressions;
using UI.Authentication;

namespace UI.Pages.Account
{
    public partial class SignIn
    {
        #region Variables

        private MudTheme _theme = new();

        bool success;
        string[] errors = { };
        MudTextField<string> pwField1;
        MudForm form;

        private bool loading = false;
        private bool LoggedIn = false;
        private bool PasswordVisible = false;

        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        MstUser oModel = new MstUser();

        #endregion

        #region Function

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
        private async Task<MstUser> Login()
        {
            var res = new MstUser();
            try
            {
                loading = true;
                await form.Validate();
                if (success)
                {
                    res = await _authenticate.Login(oModel);
                    if (res != null && res.Id > 0)
                    {
                        await LocalStorage.SetItemAsync("UserAuthenticatedToken", res.Token);
                        if (res.IsOtpenable)
                        {
                            Navigation.NavigateTo($"{Navigation.BaseUri}TwoStep/{res.Id.ToString()}", true);
                        }
                        else
                        {
                            await ((AuthStateProvider)_authState).NotifyUserAuthentication(res.Token);
                            var authState = await _authState.GetAuthenticationStateAsync();
                            var user = authState.User;
                            if (user.Identity.IsAuthenticated)
                            {
                                await _cookie.SetCookie("optPortalid", res.Token);
                                Navigation.NavigateTo(Navigation.BaseUri + "Dashboard", true);
                            }
                            else
                            {
                                Navigation.NavigateTo(Navigation.BaseUri, true);
                            }
                        }
                    }
                    else if (res != null && res.Id == 0)
                    {
                        if (res.Token == "01") //Profile Deactivate
                        {
                            Snackbar.Add(res.Name, Severity.Error);
                        }
                        else if (res.Token == "02") // Account Deactivate
                        {
                            Snackbar.Add(res.Name, Severity.Error);
                        }
                        else
                        {
                            Snackbar.Add("Incorrect email/password", Severity.Error);
                        }
                    }
                    else
                    {
                        Snackbar.Add("Incorrect email/password", Severity.Error);
                    }
                }
                else
                {
                    Snackbar.Add("Email and Password required", Severity.Error);
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return res;
        }

        #endregion

        #region Events

        protected async override Task OnInitializedAsync()
        {
            try
            {
                await Task.Delay(1);
                string MachineName = Environment.GetEnvironmentVariable("COMPUTERNAME");
                if (MachineName == "HAMZAQAZI")
                {
                    oModel.Email = "iamhamzaqazi@yahoo.com";
                    oModel.Password = "Super@12345";
                }
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
using AppDBContext.VMModels;
using System.Text.RegularExpressions;

namespace UI.Pages.Account
{
    public partial class ResetPassword
    {
        #region Variables

        private MudTheme _theme = new();

        private bool loading = false;
        private bool IsSent = false;
        private bool IsVerified = false;
        private bool IsSet = false;
        private bool EmailExist = false;
        private string Email;
        private string Code;
        private string NewPassword;

        private bool PasswordVisible = false;

        MudForm formEmail;
        MudForm formPassword;

        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        bool successEmail;
        bool successPassword;
        string[] errors = { };
        MudTextField<string> pwField1;

        MstUser oModel = new MstUser();
        UserPasswordRequest oModelPasswordRequest = new UserPasswordRequest();
        IEnumerable<UserPasswordRequest> oListPasswordRequest = new List<UserPasswordRequest>();

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

        private async Task<bool> VerifyCode()
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;
                string ClientCode = Code.Replace(" ", "");
                string Clause = $@" AND IsActive = 'True' and EncryptKey = '{ClientCode}' and Email = '{Email}' AND UserKey = '{oModel.UniqueKey}'";
                oListPasswordRequest = await _authenticate.GetAllUserPasswordDataByClause(Clause);
                oModelPasswordRequest = oListPasswordRequest.FirstOrDefault();
                if (oModelPasswordRequest == null)
                {
                    Snackbar.Add("Invalid code, try again!", Severity.Error);
                    IsVerified = false;
                    IsSent = true;
                }
                else
                {
                    IsVerified = true;
                    IsSent = false;
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return IsVerified;
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
                string Clause = $@" AND EMAIL = '{Email}'";
                var obj = await _authenticate.VerifyUser(Clause);
                if (obj?.Count > 0)
                {
                    oModel = obj.FirstOrDefault();
                }
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
                if (!string.IsNullOrWhiteSpace(Email))
                {
                    var res = await _authenticate.CheckEmail(Email);
                    if (res.Id > 0)
                    {
                        await CallAPI();
                        EmailExist = true;
                    }
                    else
                    {
                        EmailExist = false;
                        Snackbar.Add("No Email found.", Severity.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return EmailExist;
        }
        public void SetPasswordResetModelValues()
        {
            try
            {
                if (oModel != null && oModel.Id > 0)
                {
                    oModelPasswordRequest = new UserPasswordRequest();
                    oModelPasswordRequest.UserKey = oModel.UniqueKey;
                    oModelPasswordRequest.Email = oModel.Email;                    
                    oModelPasswordRequest.AddedBy = oModel.Name;
                    IsSet = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task<APIResponseModel> SentCode()
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;
                if (successEmail)
                {
                    await CheckEmail();
                    if (EmailExist)
                    {
                        SetPasswordResetModelValues();
                        if (IsSet)
                        {
                            res = await _authenticate.Crud(oModelPasswordRequest);
                            if (res.Id > 0)
                            {
                                Snackbar.Add("Code sent!", Severity.Success);
                                IsSent = true;
                            }
                            else
                            {
                                Snackbar.Add(res.Message, Severity.Error);
                            }
                        }
                    }
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                loading = false;
            }
            return res;
        }
        private async Task<APIResponseModel> ChangePassword()
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;
                if (successPassword)
                {
                    if (oModel != null && oModel.Id > 0)
                    {
                        oModel.Password = NewPassword;                        
                        oModel.UpdatedBy = oModel.Name;
                        res = await _authenticate.ChangePassword(oModel);
                        if (res.Id > 0)
                        {
                            Snackbar.Add("Password changed, you can now login with your new password", Severity.Success);
                            Navigation.NavigateTo(Navigation.BaseUri + "/SignIn");
                        }
                        else
                        {
                            Snackbar.Add(res.Message, Severity.Error);
                        }
                    }
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                loading = false;
            }
            return res;
        }

        #endregion

        #region Events

        protected async override Task OnInitializedAsync()
        {
            try
            {
                loading = true;
                await Task.Delay(1);
                //await CallAPI();
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
using AppDBContext.VMModels;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using UI.Authentication;

namespace UI.Pages.Account
{
    public partial class TwoStep
    {
        #region Variables

        [Parameter]
        public string UserID { get; set; }

        private MudTheme _theme = new();

        private bool loading = false;
        private bool IsSent = false;
        private bool IsVerified = false;
        private string Code;
        private string Type = "";
        private string ModifiedContact = "";
        private string ModifiedEmail = "";

        MstUser oModel = new MstUser();
        CfgTwoFa oModelTwoFA = new CfgTwoFa();

        #endregion

        #region Functions

        private async Task<bool> VerifyCode()
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;
                if (oModelTwoFA != null && oModelTwoFA.Id > 0)
                {
                    if (Type == "Google")
                    {
                        GoogleAuthenticator Authenticator = new GoogleAuthenticator();
                        if (!string.IsNullOrWhiteSpace(Code))
                        {
                            string ClientCode = Code.Replace(" ", "");
                            if (Authenticator.VerifyCode(oModelTwoFA.SecretKey, ClientCode))
                            {
                                Snackbar.Add("Code verified!", Severity.Success);
                                IsVerified = true;
                            }
                            else
                            {
                                Snackbar.Add("Invalid code or code expire!", Severity.Error);
                            }
                        }
                    }
                    else if (Type == "SMS" || Type == "Email")
                    {
                        if (!string.IsNullOrWhiteSpace(Code))
                        {
                            string ClientCode = Code;

                            string Clause = $@" AND UserKey = '{oModel.UniqueKey}' And IsActive = 'True' AND IsOtpenable = 'True' AND OtpCode = '{ClientCode}'";

                            var oList = await _cfgUser.GetAllTwoFADataByClause(Clause);
                            var objTwoFA = oList?.Where(x => DateTime.Now <= x.CodeExpiry).FirstOrDefault();
                            if (objTwoFA != null && objTwoFA.Id > 0)
                            {
                                Snackbar.Add("Code verified!", Severity.Success);
                                IsVerified = true;
                            }
                            else
                            {
                                Snackbar.Add("Invalid code or code expire!", Severity.Error);
                            }
                        }
                    }
                    if (IsVerified)
                    {
                        string Token = await LocalStorage.GetItemAsync<string>("UserAuthenticatedToken");
                        await ((AuthStateProvider)_authState).NotifyUserAuthentication(Token);
                        var authState = await _authState.GetAuthenticationStateAsync();
                        var user = authState.User;
                        if (user.Identity.IsAuthenticated)
                        {
                            await _cookie.SetCookie("optPortalid", Token);
                            Navigation.NavigateTo(Navigation.BaseUri + "Dashboard", true);
                        }
                        else
                        {
                            Navigation.NavigateTo(Navigation.BaseUri + "SignIn");
                        }
                    }
                }
                loading = false;
                return IsVerified;
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
                string Clause = $@" AND Id = {UserID} And IsActive = 'True' And IsOtpenable = 'True'";
                var Obj = await _mstUser.GetAllData(Clause);
                if (Obj?.Count() > 0)
                {
                    oModel = Obj.FirstOrDefault();
                }
                ModifiedContact = new string('●', oModel.Contact.Length - 4) + oModel.Contact.Substring(oModel.Contact.Length - 4);
                string pattern = @"(?<=[\w]{3})[\w\-._\+%]*(?=[\w]{2}@)";
                ModifiedEmail = Regex.Replace(oModel.Email, pattern, m => new string('●', m.Length));
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
                string Clause = $@" AND UserKey = '{oModel.UniqueKey}' And IsActive = 'True' And IsOtpenable = 'True'";

                var oList = await _cfgUser.GetAllTwoFADataByClause(Clause);
                if (oList?.Count() > 0)
                {
                    oModelTwoFA = oList.FirstOrDefault();
                }
                Type = oModelTwoFA.Otptype;
                if (Type == "SMS" || Type == "Email")
                {
                    await SentCode();
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        public async Task SentCode()
        {
            try
            {
                loading = true;
                Type = oModelTwoFA.Otptype;
                if (!string.IsNullOrWhiteSpace(Type))
                {
                    CfgTwoFa oModel2FA = new CfgTwoFa();
                    oModel2FA.UserKey = oModel.UniqueKey;
                    oModel2FA.Otptype = Type;
                    if (Type == "SMS")
                    {
                        oModel2FA.Otpcode = "1234";
                    }
                    oModel2FA.IsOtpenable = true;
                    oModel2FA.CodeExpiry = DateTime.SpecifyKind(DateTime.Now.AddMinutes(2), DateTimeKind.Unspecified);
                    oModel2FA.AddedBy = oModel.Name;
                    var res = await _cfgUser.Crud(oModel2FA);
                    if (res.Id > 0)
                    {
                        if (this.Type != "Google")
                        {
                            Snackbar.Add("Code sent!", Severity.Success);
                        }
                    }
                    else
                    {
                        IsSent = false;
                        Snackbar.Add(res.Message, Severity.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
            loading = false;
        }

        #endregion

        #region Events

        protected async override Task OnInitializedAsync()
        {
            try
            {
                loading = true;
                await CallAPI();
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
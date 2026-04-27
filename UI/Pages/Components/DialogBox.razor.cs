using AppDBContext.Models;
using AppDBContext.VMModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.RegularExpressions;
using UI.Authentication;

namespace UI.Pages.Components
{
    public partial class DialogBox
    {

        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        private bool loading = false;
        private bool IsSet = false;
        private bool IsSent = false;

        private int UserID = 0;
        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";

        private string OtpCode = "";
        private string QRCodeURL = "";
        private string SecretKey = "";
        private string ManualCode = "";
        private string Type = "";

        private string searchString1 = "";

        //private bool FilterFuncCustomer(MstBusinessCustomer element) => FilterFuncCustomer(element, searchStringCustomer);
        //private IEnumerable<MstBusinessCustomer> oListCustomer = new List<MstBusinessCustomer>();

        //HubConnection _hubConnection;
        //public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;


        [Parameter] public string DialogFor { get; set; }
        [Parameter] public bool DialogMulti { get; set; }
        [Parameter] public string CategoryType { get; set; }
        [Parameter] public string DocEntry { get; set; }

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }
        void Cancel() => MudDialog.Cancel();

        private int selectedRowNumber = -1;
        private List<string> clickedEvents = new();

        HubConnection _hubConnection;
        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        MstUser oModelUser = new MstUser();
        MstBusiness oModelBusiness = new MstBusiness();
        CfgEmailVerification oModelEmailVerification = new CfgEmailVerification();
        CfgContactVerification oModelContactVerification = new CfgContactVerification();
        CfgTwoFa oModel2FA = new CfgTwoFa();

        private IEnumerable<UserAlert> oListUserAlert = new List<UserAlert>();

        private IEnumerable<MstUserMessage> oListUserMessage = new List<MstUserMessage>();

        private bool FilterFuncMstAssociates(MstAssociates element) => FilterFuncMstAssociates(element, searchString1);
        private MudTable<MstAssociates> _tableMstAssociates;
        MstAssociates oModelMstAssociates = new MstAssociates();
        private IEnumerable<MstAssociates> oListMstAssociates = new List<MstAssociates>();
        private HashSet<MstAssociates> oListSelectedMstAssociates = new HashSet<MstAssociates>();

        private bool FilterFuncMstAssociatesAvailability(MstAssociatesAvailability element) => FilterFuncMstAssociatesAvailability(element, searchString1);
        private MudTable<MstAssociatesAvailability> _tableMstAssociatesAvailability;
        MstAssociatesAvailability oModelMstAssociatesAvailability = new MstAssociatesAvailability();
        private IEnumerable<MstAssociatesAvailability> oListMstAssociatesAvailability = new List<MstAssociatesAvailability>();
        private HashSet<MstAssociatesAvailability> oListSelectedMstAssociatesAvailability = new HashSet<MstAssociatesAvailability>();

        private bool FilterFuncMstAssociatesPackages(MstAssociatesPackages element) => FilterFuncMstAssociatesPackages(element, searchString1);
        private MudTable<MstAssociatesPackages> _tableMstAssociatesPackages;
        MstAssociatesPackages oModelMstAssociatesPackages = new MstAssociatesPackages();
        private IEnumerable<MstAssociatesPackages> oListMstAssociatesPackages = new List<MstAssociatesPackages>();
        private HashSet<MstAssociatesPackages> oListSelectedMstAssociatesPackages = new HashSet<MstAssociatesPackages>();

        private bool FilterFuncTrnsPackages(TrnsPackages element) => FilterFuncTrnsPackages(element, searchString1);
        private MudTable<TrnsPackages> _tableTrnsPackages;
        TrnsPackages oModelTrnsPackages = new TrnsPackages();
        private IEnumerable<TrnsPackages> oListTrnsPackages = new List<TrnsPackages>();
        private HashSet<TrnsPackages> oListSelectedTrnsPackages = new HashSet<TrnsPackages>();

        #region Deactivate profile

        bool successUser;
        string[] errors = { };
        MudForm formUser;

        MudTextField<string> pwField1;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private bool PasswordVisible = false;
        private string Password = "";

        #endregion

        #region Update Payment

        //[Parameter] public VMBookingPayments DialogData { get; set; }

        bool successPayment;
        MudForm formPayment;

        private DateTime? PaymentDate = DateTime.Today;
        private DateTime? SecondPaymentDate = DateTime.Today;
        //VMBookingPayments oModelBookingPayment = new VMBookingPayments();

        #endregion

        #endregion

        #region Functions

        public async Task LogoutConfirm()
        {
            await Task.Delay(1);
            MudDialog.Close(DialogResult.Ok(true));
        }
        public async Task ConfirmSignUp()
        {
            await Task.Delay(1);
            MudDialog.Close(DialogResult.Ok(true));
        }

        #region Deactivate Profile
        public async Task DeactivateProfile()
        {
            await formUser.Validate();
            if (successUser)
            {
                if (Password == oModelUser.Password)
                {
                    MudDialog.Close(DialogResult.Ok<MstUser>(oModelUser));
                }
                else
                {
                    Snackbar.Add("Invalid password.", MudBlazor.Severity.Error);
                }
            }
        }
        public async Task DeactivateAccount()
        {
            await formUser.Validate();
            if (successUser)
            {
                if (Password == oModelUser.Password)
                {
                    MudDialog.Close(DialogResult.Ok<MstUser>(oModelUser));
                }
                else
                {
                    Snackbar.Add("Invalid password.", MudBlazor.Severity.Error);
                }
            }
        }
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

        #endregion

        #region Payment

        private void CalculateRemainingAmount()
        {
            try
            {
                //oModelBookingPayment.Remaining = oModelBookingPayment.Amount - oModelBookingPayment.Deposit;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async void SetSecondPaymentDate()
        {
            try
            {
                //oModelBookingPayment.SecondPaymentDate = Convert.ToDateTime(SecondPaymentDate);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            await Task.CompletedTask;
        }
        public async Task UpdatePayment()
        {
            await formPayment.Validate();
            if (successPayment)
            {
                CalculateRemainingAmount();
                SetSecondPaymentDate();
                //MudDialog.Close(DialogResult.Ok<VMBookingPayments>(oModelBookingPayment));
            }
        }
        private async Task SetPaymentModel()
        {
            try
            {
                //oModelBookingPayment = DialogData;
                //PaymentDate = oModelBookingPayment.PaymentDate;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            await Task.CompletedTask;
        }

        #endregion

        private async Task GetUser()
        {
            try
            {
                string Clause = $@"AND UniqueKey = '{UserKey}'";
                var obj = await _mstUser.GetAllData(Clause);
                if (obj?.Count() > 0)
                {
                    oModelUser = obj.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetBusiness()
        {
            try
            {
                string Clause = $@"AND UniqueKey = '{UserKey}'";
                var obj = await _businessData.GetAllBusinessData(Clause);
                if (obj?.Count() > 0)
                {
                    oModelBusiness = obj.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }

        #region User Alert and Notifications
        private async Task GetUserAlert()
        {
            try
            {
                //oListUserAlert = await _mstUserMessage.GetUserMessage.GetAllUserAlertDataByBusiness(BusinessID);
                UpdateTimeStamp();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            await Task.CompletedTask;
        }
        private void UpdateTimeStamp()
        {
            try
            {
                if (DialogFor == "Notifications")
                {
                    if (oListUserAlert.Count() > 0)
                    {
                        oListUserAlert.ToList().ForEach(x =>
                        {
                            x.TimeCalculate = TimeCalculation(x.AddedDt.GetValueOrDefault());
                        });
                    }
                }
                else if (DialogFor == "Messages")
                {
                    if (oListUserMessage.Count() > 0)
                    {
                        oListUserMessage.ToList().ForEach(x =>
                        {
                            x.TimeCalculate = TimeCalculation(x.AddedDt.GetValueOrDefault());
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task<APIResponseModel> MarkasRead(int ID)
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;
                if (DialogFor == "Notifications")
                {
                    var SelectedAlert = oListUserAlert.Where(x => x.Id == ID).FirstOrDefault();
                    if (SelectedAlert != null && SelectedAlert.Id > 0)
                    {
                        SelectedAlert.MarkAsRead = true;
                        //res = await _mstEmailNotificationPreferences.Update(SelectedAlert);
                    }
                    if (res.Id > 0)
                    {
                        Snackbar.Add("Marked as read", Severity.Success);
                        //await GetUserAlert();
                    }
                    else
                    {
                        Snackbar.Add(res.Message, Severity.Error);
                    }
                }
                else if (DialogFor == "Messages")
                {
                    var SelectedMessage = oListUserMessage.Where(x => x.Id == ID).FirstOrDefault();
                    if (SelectedMessage != null && SelectedMessage.Id > 0)
                    {
                        SelectedMessage.MarkAsRead = true;
                        res = await _mstUserMessage.Crud(SelectedMessage);
                    }
                    if (res.Id > 0)
                    {
                        Snackbar.Add("Marked as read", Severity.Success);
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
        private string TimeCalculation(DateTime AddedDT)
        {
            try
            {
                const int SECOND = 1;
                const int MINUTE = 60 * SECOND;
                const int HOUR = 60 * MINUTE;
                const int DAY = 24 * HOUR;
                const int MONTH = 30 * DAY;

                var ts = new TimeSpan(DateTime.Now.Ticks - AddedDT.Ticks);
                double delta = Math.Abs(ts.TotalSeconds);

                if (delta < 1 * MINUTE)
                    return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

                if (delta < 2 * MINUTE)
                    return "a minute ago";

                if (delta < 45 * MINUTE)
                    return ts.Minutes + " minutes ago";

                if (delta < 90 * MINUTE)
                    return "an hour ago";

                if (delta < 24 * HOUR)
                    return ts.Hours + " hours ago";

                if (delta < 48 * HOUR)
                    return "yesterday";

                if (delta < 30 * DAY)
                    return ts.Days + " days ago";

                if (delta < 12 * MONTH)
                {
                    int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    return months <= 1 ? "one month ago" : months + " months ago";
                }
                else
                {
                    int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                    return years <= 1 ? "one year ago" : years + " years ago";
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                return null;
            }
        }
        public async Task SetModelValues(string OTPType)
        {
            try
            {
                await Task.Delay(1);
                if (DialogFor == "Verify Email")
                {
                    oModelEmailVerification.UserKey = oModelUser.UniqueKey;
                    oModelEmailVerification.UserEmail = oModelUser.Email;
                    oModelEmailVerification.Code = "1234";
                    oModelEmailVerification.IsVerify = false;
                    oModelEmailVerification.AddedBy = LoggedInUser;
                    IsSet = true;
                }
                if (DialogFor == "Verify Contact")
                {
                    oModelContactVerification.UserKey = oModelUser.UniqueKey;
                    oModelContactVerification.UserContact = oModelUser.Contact;
                    oModelContactVerification.Code = "1234";
                    oModelContactVerification.IsVerify = false;
                    oModelContactVerification.AddedBy = LoggedInUser;
                    IsSet = true;
                }
                if (DialogFor == "Two Factor Authentication" && !string.IsNullOrWhiteSpace(OTPType))
                {
                    oModel2FA.UserKey = oModelUser.UniqueKey;
                    oModel2FA.Otptype = OTPType;
                    oModel2FA.IsOtpenable = false;
                    oModel2FA.AddedBy = LoggedInUser;
                    IsSet = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task<APIResponseModel> SentCode(string OTPType)
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;
                Type = OTPType;
                await SetModelValues(OTPType);
                if (IsSet && DialogFor == "Verify Email")
                {
                    res = await _cfgUser.Crud(oModelEmailVerification);
                }
                if (IsSet && DialogFor == "Verify Contact")
                {
                    res = await _cfgUser.Crud(oModelContactVerification);
                }
                if (IsSet && DialogFor == "Two Factor Authentication")
                {
                    if (Type == "Google")
                    {
                        GoogleAuthenticator Authenticator = new GoogleAuthenticator();

                        ManualCode = Authenticator.GenerateCode(out QRCodeURL, out SecretKey);

                        if (!string.IsNullOrWhiteSpace(SecretKey) && !string.IsNullOrWhiteSpace(ManualCode) && !string.IsNullOrWhiteSpace(QRCodeURL))
                        {
                            oModel2FA.SecretKey = SecretKey;
                            oModel2FA.ManualCode = ManualCode;
                            oModel2FA.Otpcode = "";
                        }
                    }
                    else if (Type == "SMS")
                    {
                        oModel2FA.Otpcode = "1234";
                        oModel2FA.CodeExpiry = DateTime.SpecifyKind(DateTime.Now.AddMinutes(2), DateTimeKind.Unspecified);
                    }
                    else if (Type == "Email")
                    {
                        //oModel2FA.Otpcode = "123456";
                        oModel2FA.CodeExpiry = DateTime.SpecifyKind(DateTime.Now.AddMinutes(2), DateTimeKind.Unspecified);
                    }
                    res = await _cfgUser.Crud(oModel2FA);
                }
                if (res.Id > 0)
                {
                    IsSent = true;
                    oModelEmailVerification.Code = "";
                    oModelContactVerification.Code = "";
                    if (Type != "Google")
                    {
                        Snackbar.Add("Code sent!", Severity.Success);
                    }
                }
                else
                {
                    IsSent = false;
                    Snackbar.Add(res.Message, Severity.Error);
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return res;
        }
        private async Task<APIResponseModel> VerifyCode()
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;
                bool IsVerified = false;
                if (DialogFor == "Verify Email" && !string.IsNullOrWhiteSpace(oModelEmailVerification.Code))
                {
                    string Code = oModelEmailVerification.Code;

                    string Clause = $@" AND UserEmail = '{oModelUser.Email}' AND Code = '{Code}' And IsActive = 'True' And IsVerify = 'False'";
                    var oList = await _cfgUser.GetAllEmailVerificationDataByClause(Clause);
                    if (oList?.Count() > 0)
                    {
                        var cfgEmailVerification = oList.FirstOrDefault();
                        cfgEmailVerification.UserKey = oModelUser.UniqueKey;
                        cfgEmailVerification.IsVerify = true;
                        cfgEmailVerification.IsActive = false;
                        cfgEmailVerification.UpdatedBy = oModelUser.UpdatedBy = LoggedInUser;
                        oModelUser.IsEmailVerify = true;


                        res = await _cfgUser.Crud(cfgEmailVerification);
                    }
                    else
                    {
                        Snackbar.Add("Incorrect code, try again!", Severity.Error);
                    }
                }
                if (DialogFor == "Verify Contact" && !string.IsNullOrWhiteSpace(oModelContactVerification.Code))
                {
                    string Code = oModelContactVerification.Code;

                    string Clause = $@" AND UserContact = '{oModelUser.Contact}' AND Code = '{Code}' And IsActive = 'True' And IsVerify = 'False'";
                    var oList = await _cfgUser.GetAllContactVerificationDataByClause(Clause);
                    if (oList?.Count() > 0)
                    {
                        var cfgContactVerification = oList.FirstOrDefault();
                        cfgContactVerification.UserKey = oModelUser.UniqueKey;
                        cfgContactVerification.IsVerify = true;
                        cfgContactVerification.IsActive = false;
                        cfgContactVerification.UpdatedBy = oModelUser.UpdatedBy = LoggedInUser;
                        oModelUser.IsContactVerify = true;

                        res = await _cfgUser.Crud(cfgContactVerification);
                    }
                    else
                    {
                        Snackbar.Add("Incorrect code, try again!", Severity.Error);
                    }
                }
                if (DialogFor == "Two Factor Authentication" && !string.IsNullOrWhiteSpace(OtpCode))
                {


                    string Clause = $@" AND UserKey = '{oModelUser.UniqueKey}' And IsActive = 'True'";

                    var oList = await _cfgUser.GetAllTwoFADataByClause(Clause);
                    var cfgTwoFAVerification = new CfgTwoFa();
                    string ClientCode = OtpCode.Replace(" ", "");
                    if (Type == "Google")
                    {
                        GoogleAuthenticator Authenticator = new GoogleAuthenticator();
                        if (!string.IsNullOrWhiteSpace(SecretKey) && !string.IsNullOrWhiteSpace(oModel2FA.ManualCode) && Authenticator.VerifyCode(SecretKey, ClientCode))
                        {
                            cfgTwoFAVerification = oList.Where(x => x.SecretKey == SecretKey && x.Otptype == Type
                                                                    && x.ManualCode == oModel2FA.ManualCode).FirstOrDefault();
                            if (cfgTwoFAVerification.Id > 0)
                            {
                                IsVerified = true;
                            }
                        }
                        else
                        {
                            Snackbar.Add("Incorrect code, try again!", Severity.Error);
                        }
                    }
                    else if (Type == "SMS")
                    {
                        cfgTwoFAVerification = oList.Where(x => x.Otptype == Type && DateTime.Now <= x.CodeExpiry
                                                                    && x.Otpcode == ClientCode).FirstOrDefault();
                        if (cfgTwoFAVerification != null && cfgTwoFAVerification.Id > 0)
                        {
                            IsVerified = true;
                        }
                    }
                    else if (Type == "Email")
                    {
                        cfgTwoFAVerification = oList.Where(x => x.Otptype == Type && DateTime.Now <= x.CodeExpiry
                                                                    && x.Otpcode == ClientCode).FirstOrDefault();
                        if (cfgTwoFAVerification != null && cfgTwoFAVerification.Id > 0)
                        {
                            IsVerified = true;
                        }
                    }
                    if (IsVerified)
                    {
                        cfgTwoFAVerification.UpdatedBy = oModelUser.UpdatedBy = LoggedInUser;
                        cfgTwoFAVerification.IsOtpenable = true;

                        res = await _cfgUser.Crud(cfgTwoFAVerification);
                        oModelUser.IsOtpenable = true;
                        res = await _mstUser.Crud(oModelUser);
                        Snackbar.Add("2FA Enabled!", Severity.Success);
                        await LocalStorage.RemoveItemAsync("UserAuthenticatedToken");
                        ((AuthStateProvider)_authState).NotifyUserLogout();
                        Snackbar.Add("Logged out", Severity.Info);
                        Navigation.NavigateTo(Navigation.BaseUri + "/SignIn");
                    }
                }
                if (res.Id > 0 && !IsVerified)
                {
                    res = await _mstUser.Crud(oModelUser);
                    MudDialog.Close(DialogResult.Ok(true));
                    Snackbar.Add("Code Verified!", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Incorrect code, try again!", Severity.Error);
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return res;
        }
        private async Task<APIResponseModel> DisableTwoFA()
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;

                if (DialogFor == "Disbale Two Factor Authentication")
                {
                    string Clause = $@" AND UserKey = '{oModelUser.UniqueKey}' And IsActive = 'True'  And IsOtpenable = 'True'";

                    var oList = await _cfgUser.GetAllTwoFADataByClause(Clause);

                    if (oList?.Count() > 0)
                    {
                        var cfgTwoFAVerification = oList.FirstOrDefault();
                        cfgTwoFAVerification.UpdatedBy = oModelUser.UpdatedBy = LoggedInUser;
                        cfgTwoFAVerification.IsOtpenable = false;
                        cfgTwoFAVerification.IsActive = false;

                        res = await _cfgUser.Crud(cfgTwoFAVerification);
                        oModelUser.IsOtpenable = false;
                        res = await _mstUser.Crud(oModelUser);
                        Snackbar.Add("2FA Disabled!", Severity.Success);
                        MudDialog.Close(DialogResult.Ok(true));
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

        private string VerificationCode
        {
            get => DialogFor == "Verify Email"
                ? oModelEmailVerification.Code
                : oModelContactVerification.Code;

            set
            {
                if (DialogFor == "Verify Email")
                    oModelEmailVerification.Code = value;
                else if (DialogFor == "Verify Contact")
                    oModelContactVerification.Code = value;
            }
        }

        #endregion

        #region Mst Business Customer

        //private async Task GetAllCustomers()
        //{
        //    try
        //    {
        //        string Clause = $@" AND BusinessKey = '{BusinessKey}'";
        //        //oListCustomer = await _businessData.GetAllDataByBusinessID(Clause);
        //        if (DialogFor == "NotCompletedCustomers")
        //        {
        //            oListCustomer = oListCustomer.Where(x => x.IsBookingCompleted == false).ToList();
        //        }
        //        if (DialogFor == "CompletedCustomers")
        //        {
        //            oListCustomer = oListCustomer.Where(x => x.IsBookingCompleted == true).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogsUI.GenerateLogs(ex);
        //    }
        //}
        //private bool FilterFuncCustomer(MstBusinessCustomer element, string searchString1)
        //{
        //    if (string.IsNullOrWhiteSpace(searchString1))
        //        return true;
        //    if (element.CustomerName.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.CustomerEmail.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.CustomerContact.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.CustomerNic.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.IsActive.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.IsBookingCompleted.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    return false;
        //}
        //public void RowClickEventCustomer(TableRowClickEventArgs<MstBusinessCustomer> tableRowClickEventArgs)
        //{
        //    try
        //    {
        //        clickedEvents.Add("Row has been clicked");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogsUI.GenerateLogs(ex);
        //    }

        //}
        //private string SelectedRowClassFuncCustomer(MstBusinessCustomer element, int rowNumber)
        //{
        //    if (selectedRowNumber == rowNumber)
        //    {
        //        selectedRowNumber = -1;
        //        clickedEvents.Add("Selected Row: None");
        //        return string.Empty;
        //    }
        //    else if (_tableCustomers.SelectedItem != null && _tableCustomers.SelectedItem.Equals(element))
        //    {
        //        selectedRowNumber = rowNumber;
        //        clickedEvents.Add($"Selected Row: {rowNumber}");
        //        return "selected";
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        #endregion

        #region MstAssociates

        private async Task GetMstAssociates()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}' ";
                if (DialogFor == "ActiveMstAssociates")
                {
                    Clause += $"And CategoryType = '{CategoryType}' AND IsActive = 'True'";
                }
                oListMstAssociates = await _masterData.GetAllAssociatesData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private bool FilterFuncMstAssociates(MstAssociates element, string searchString1)
        {
            if (string.IsNullOrWhiteSpace(searchString1))
                return true;
            if (element.BusinessName.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.CategoryType.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.City.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Area.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.AddedBy.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.IsActive.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        public void RowClickEventMstAssociates(TableRowClickEventArgs<MstAssociates> tableRowClickEventArgs)
        {
            try
            {
                clickedEvents.Add("Row has been clicked");
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }

        }
        private string SelectedRowClassFuncMstAssociates(MstAssociates element, int rowNumber)
        {
            if (selectedRowNumber == rowNumber)
            {
                selectedRowNumber = -1;
                clickedEvents.Add("Selected Row: None");
                return string.Empty;
            }
            else if (_tableMstAssociates.SelectedItem != null && _tableMstAssociates.SelectedItem.Equals(element))
            {
                selectedRowNumber = rowNumber;
                clickedEvents.Add($"Selected Row: {rowNumber}");
                return "selected";
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region MstAssociatesAvailability

        private async Task GetMstAssociatesAvailability()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}' and DocEntry = {DocEntry}";
                if (DialogFor == "ActiveMstAssociatesAvailability")
                {
                    Clause += " AND IsActive = 'True'";
                }
                oListMstAssociatesAvailability = await _masterData.GetAllAssociatesAvailabilityData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private bool FilterFuncMstAssociatesAvailability(MstAssociatesAvailability element, string searchString1)
        {
            if (string.IsNullOrWhiteSpace(searchString1))
                return true;
            if (element.AvailableDays.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.TimeSlots.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.AddedBy.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.IsActive.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        public void RowClickEventMstAssociatesAvailability(TableRowClickEventArgs<MstAssociatesAvailability> tableRowClickEventArgs)
        {
            try
            {
                clickedEvents.Add("Row has been clicked");
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }

        }
        private string SelectedRowClassFuncMstAssociatesAvailability(MstAssociatesAvailability element, int rowNumber)
        {
            if (selectedRowNumber == rowNumber)
            {
                selectedRowNumber = -1;
                clickedEvents.Add("Selected Row: None");
                return string.Empty;
            }
            else if (_tableMstAssociatesAvailability.SelectedItem != null && _tableMstAssociatesAvailability.SelectedItem.Equals(element))
            {
                selectedRowNumber = rowNumber;
                clickedEvents.Add($"Selected Row: {rowNumber}");
                return "selected";
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region MstAssociatesPackages

        private async Task GetMstAssociatesPackages()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}' and DocEntry = {DocEntry}";
                if (DialogFor == "ActiveMstAssociatesPackages")
                {
                    Clause += " AND IsActive = 'True'";
                }
                oListMstAssociatesPackages = await _masterData.GetAllAssociatesPackagesData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private bool FilterFuncMstAssociatesPackages(MstAssociatesPackages element, string searchString1)
        {
            if (string.IsNullOrWhiteSpace(searchString1))
                return true;
            if (element.ItemName.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.ItemPrice.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.ItemsCount.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.MinHead.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Remarks.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.AddedBy.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.IsActive.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        public void RowClickEventMstAssociatesPackages(TableRowClickEventArgs<MstAssociatesPackages> tableRowClickEventArgs)
        {
            try
            {
                clickedEvents.Add("Row has been clicked");
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }

        }
        private string SelectedRowClassFuncMstAssociatesPackages(MstAssociatesPackages element, int rowNumber)
        {
            if (selectedRowNumber == rowNumber)
            {
                selectedRowNumber = -1;
                clickedEvents.Add("Selected Row: None");
                return string.Empty;
            }
            else if (_tableMstAssociatesPackages.SelectedItem != null && _tableMstAssociatesPackages.SelectedItem.Equals(element))
            {
                selectedRowNumber = rowNumber;
                clickedEvents.Add($"Selected Row: {rowNumber}");
                return "selected";
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region TrnsPackages

        private async Task GetTrnsPackages()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}' ";
                if (DialogFor == "ActiveTrnsPackages")
                {
                    Clause += $" AND IsActive = 'True'";
                }
                oListTrnsPackages = await _masterData.GetAllPackagesData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private bool FilterFuncTrnsPackages(TrnsPackages element, string searchString1)
        {
            if (string.IsNullOrWhiteSpace(searchString1))
                return true;
            if (element.CategoryType.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.PackageType.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.PackageName.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.BasePrice.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.AddedBy.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.IsActive.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        public void RowClickEventTrnsPackages(TableRowClickEventArgs<TrnsPackages> tableRowClickEventArgs)
        {
            try
            {
                clickedEvents.Add("Row has been clicked");
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }

        }
        private string SelectedRowClassFuncTrnsPackages(TrnsPackages element, int rowNumber)
        {
            if (selectedRowNumber == rowNumber)
            {
                selectedRowNumber = -1;
                clickedEvents.Add("Selected Row: None");
                return string.Empty;
            }
            else if (_tableTrnsPackages.SelectedItem != null && _tableTrnsPackages.SelectedItem.Equals(element))
            {
                selectedRowNumber = rowNumber;
                clickedEvents.Add($"Selected Row: {rowNumber}");
                return "selected";
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region User Message

        private async Task GetUserMessage()
        {
            try
            {
                string Clause = $@" AND UserKey = '{UserKey}'";
                oListUserMessage = await _mstUserMessage.GetUserMessage(Clause);
                if (oListUserMessage.Where(x => x.FktoUserId == UserID).DistinctBy(x => x.FkfromUserId).Count() > 0)
                {
                    oListUserMessage = oListUserMessage.Where(x => x.FktoUserId == UserID).DistinctBy(x => x.FkfromUserId).ToList();
                    oListUserMessage = oListUserMessage.TakeLast(5);
                }
                if (oListUserMessage.Where(x => x.FkfromUserId == UserID).DistinctBy(x => x.FktoUserId).Count() > 0)
                {
                    oListUserMessage = oListUserMessage.Where(x => x.FkfromUserId == UserID).DistinctBy(x => x.FktoUserId).ToList();
                    oListUserMessage = oListUserMessage.TakeLast(5);
                }
                UpdateTimeStamp();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            await Task.CompletedTask;
        }

        private async void OpenChat()
        {
            try
            {
                await Task.Delay(1);
                Navigation.NavigateTo(Navigation.BaseUri + "Chat", true);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }

        #endregion

        private void Submit()
        {
            try
            {
                if (DialogFor.Contains("MstAssociates") && oModelMstAssociates.Id > 0 && !DialogMulti)
                {
                    MudDialog.Close(DialogResult.Ok<MstAssociates>(oModelMstAssociates));
                }
                else if (DialogFor.Contains("MstAssociates") && oListSelectedMstAssociates.Count() > 0 && DialogMulti)
                {
                    MudDialog.Close(DialogResult.Ok<HashSet<MstAssociates>>(oListSelectedMstAssociates));
                }
                else if (DialogFor.Contains("MstAssociatesAvailability") && oModelMstAssociatesAvailability.Id > 0 && !DialogMulti)
                {
                    MudDialog.Close(DialogResult.Ok<MstAssociatesAvailability>(oModelMstAssociatesAvailability));
                }
                else if (DialogFor.Contains("MstAssociatesAvailability") && oListSelectedMstAssociatesAvailability.Count() > 0 && DialogMulti)
                {
                    MudDialog.Close(DialogResult.Ok<HashSet<MstAssociatesAvailability>>(oListSelectedMstAssociatesAvailability));
                }
                else if (DialogFor.Contains("MstAssociatesPackages") && oModelMstAssociatesPackages.Id > 0 && !DialogMulti)
                {
                    MudDialog.Close(DialogResult.Ok<MstAssociatesPackages>(oModelMstAssociatesPackages));
                }
                else if (DialogFor.Contains("MstAssociatesPackages") && oListSelectedMstAssociatesPackages.Count() > 0 && DialogMulti)
                {
                    MudDialog.Close(DialogResult.Ok<HashSet<MstAssociatesPackages>>(oListSelectedMstAssociatesPackages));
                }
                else if (DialogFor.Contains("TrnsPackages") && oModelTrnsPackages.Id > 0 && !DialogMulti)
                {
                    MudDialog.Close(DialogResult.Ok<TrnsPackages>(oModelTrnsPackages));
                }
                else if (DialogFor.Contains("TrnsPackages") && oListSelectedTrnsPackages.Count() > 0 && DialogMulti)
                {
                    MudDialog.Close(DialogResult.Ok<HashSet<TrnsPackages>>(oListSelectedTrnsPackages));
                }
                else
                {
                    Snackbar.Add("Select row first", MudBlazor.Severity.Error);
                }
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
                if (DialogFor != "Signup")
                {
                    var authState = await _authState.GetAuthenticationStateAsync();
                    var user = authState.User;
                    if (user.Identity.IsAuthenticated)
                    {
                        UserID = Convert.ToInt32(user.Claims.Where(x => x.Type == "UserID").Select(x => x.Value).FirstOrDefault());
                        LoggedInUser = user.Claims.Where(x => x.Type == "Username").Select(x => x.Value).FirstOrDefault();
                        UserKey = user.Claims.Where(x => x.Type == "UserKey").Select(x => x.Value).FirstOrDefault();
                        BusinessKey = user.Claims.Where(x => x.Type == "BusinessKey").Select(x => x.Value).FirstOrDefault();

                        if (DialogFor == "Verify Email" || DialogFor == "Verify Contact" || DialogFor == "Two Factor Authentication"
                            || DialogFor == "Disbale Two Factor Authentication"
                            || DialogFor == "Deactivate Profile" || DialogFor == "Deactivate Account")
                        {
                            await GetUser();
                        }
                        else if (DialogFor == "Notifications")
                        {
                            await GetUserAlert();
                            _hubConnection = new HubConnectionBuilder().WithUrl(UIConfig.NotificationBaseURL).Build();
                            _hubConnection.On<List<UserAlert>>("Alert", (IncomingAlert) =>
                            {
                                oListUserAlert = IncomingAlert;
                                UpdateTimeStamp();
                                StateHasChanged();
                            });
                            await _hubConnection.StartAsync();
                        }
                        else if (DialogFor == "Messages")
                        {
                            await GetUserMessage();
                            _hubConnection = new HubConnectionBuilder().WithUrl(UIConfig.MessageBaseURL).Build();
                            _hubConnection.On<List<MstUserMessage>>("Message", (IncomingMessage) =>
                            {
                                if (IncomingMessage.Where(x => x.FktoUserId == UserID).DistinctBy(x => x.FkfromUserId).Count() > 0)
                                {
                                    oListUserMessage = IncomingMessage.Where(x => x.FktoUserId == UserID).DistinctBy(x => x.FkfromUserId).ToList();
                                    oListUserMessage = oListUserMessage.TakeLast(5);
                                }
                                if (IncomingMessage.Where(x => x.FkfromUserId == UserID).DistinctBy(x => x.FktoUserId).Count() > 0)
                                {
                                    oListUserMessage = IncomingMessage.Where(x => x.FkfromUserId == UserID).DistinctBy(x => x.FktoUserId).ToList();
                                    oListUserMessage = oListUserMessage.TakeLast(5);
                                }
                                UpdateTimeStamp();
                                StateHasChanged();
                            });
                            await _hubConnection.StartAsync();
                        }
                        else if (DialogFor == "CompletedCustomers" || DialogFor == "NotCompletedCustomers")
                        {
                            //await GetAllCustomers();
                        }
                        else if (DialogFor == "Update Payment")
                        {
                            await SetPaymentModel();
                        }
                        else if (DialogFor.Contains("MstAssociates") && DialogFor != "ActiveMstAssociatesAvailability" && DialogFor != "ActiveMstAssociatesPackages")
                        {
                            await GetMstAssociates();
                        }
                        else if (DialogFor.Contains("MstAssociatesAvailability"))
                        {
                            await GetMstAssociatesAvailability();
                        }
                        else if (DialogFor.Contains("MstAssociatesPackages"))
                        {
                            await GetMstAssociatesPackages();
                        }
                        else if (DialogFor.Contains("TrnsPackages"))
                        {
                            await GetTrnsPackages();
                        }
                    }
                    else
                    {
                        Navigation.NavigateTo(Navigation.BaseUri + "/SignIn");
                    }
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                loading = false;
            }
        }

        public async void Dispose()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
using AppDBContext.Models;
using AppDBContext.VMModels;
using Microsoft.AspNetCore.SignalR.Client;
using UI.Authentication;
using UI.Pages.Components;

namespace UI.Shared
{
    public partial class MainLayout
    {
        #region Variables      

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        DialogOptions NotificationBoxSetting = new DialogOptions() { MaxWidth = MaxWidth.False, DisableBackdropClick = false, Position = DialogPosition.TopRight };
        bool open = true;
        private int NotificationCount = 0;
        private int MessageCount = 0;
        private int UserID = 0;
        private string UserKey = "";
        private int BusinessID = 0;
        private string BusinessName = "";
        private string BusinessLogo = "";
        private string UserName = "";
        private string UserEmail = "";
        private string DialogFor = "";
        private MudTheme _theme = new();
        private bool ThemeMode;
        private bool LightMode;
        private bool DarkMode;
        private bool SystemMode;
        private MudThemeProvider _mudThemeProvider;

        CfgThemeMode oModelCfgTheme = new CfgThemeMode();
        private IEnumerable<UserAlert> oListUserAlert = new List<UserAlert>();
        private IEnumerable<MstUserMessage> oListUserMessage = new List<MstUserMessage>();

        HubConnection _hubConnection;
        HubConnection _hubConnectionMessage;
        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
        public bool IsConnectedMessage => _hubConnectionMessage?.State == HubConnectionState.Connected;

        #endregion

        #region Functions

        void ToggleDrawer()
        {
            open = !open;
        }
        private async Task OpenNotificationsDialog(DialogOptions options)
        {
            try
            {
                DialogFor = "Notifications";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>("", parameters, options);
                var result = await dialog.Result;

                if (!result.Canceled)
                {

                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task OpenMessageDialog(DialogOptions options)
        {
            try
            {
                DialogFor = "Messages";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>("", parameters, options);
                var result = await dialog.Result;

                if (!result.Canceled)
                {

                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task OpenLogoutDialog(DialogOptions options)
        {
            try
            {
                DialogFor = "Logout";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>(DialogFor, parameters, options);
                var result = await dialog.Result;

                if (!result.Canceled)
                {
                    await LocalStorage.RemoveItemAsync("UserAuthenticatedToken");
                    ((AuthStateProvider)_authState).NotifyUserLogout();
                    Snackbar.Add("Logged out", Severity.Info);
                    Navigation.NavigateTo(Navigation.BaseUri + "/SignIn");
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async void LightTheme()
        {
            try
            {
                var res = new APIResponseModel();
                if (oModelCfgTheme?.Id > 0)
                {
                    oModelCfgTheme.FKUserId = UserID;
                    oModelCfgTheme.IsLightMode = LightMode = true;
                    oModelCfgTheme.IsDarkMode = DarkMode = false;
                    oModelCfgTheme.IsSystemMode = ThemeMode = false;
                    oModelCfgTheme.UpdatedBy = UserName;
                }
                else
                {
                    oModelCfgTheme = new CfgThemeMode();
                    oModelCfgTheme.FKUserId = UserID;
                    oModelCfgTheme.IsLightMode = LightMode = true;
                    oModelCfgTheme.IsDarkMode = DarkMode = false;
                    oModelCfgTheme.IsSystemMode = ThemeMode = false;
                    oModelCfgTheme.AddedBy = UserName;
                }
                res = await _cfgUser.Crud(oModelCfgTheme);
                if (res.Id > 0)
                {
                    Snackbar.Add("Theme Changed.", Severity.Success);
                }
                //Navigation.NavigateTo(Navigation.Uri, true);
                //await GetUserConfiguration();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async void DarkTheme()
        {
            try
            {
                var res = new APIResponseModel();
                if (oModelCfgTheme?.Id > 0)
                {
                    oModelCfgTheme.FKUserId = UserID;
                    oModelCfgTheme.IsDarkMode = ThemeMode = DarkMode = true;
                    oModelCfgTheme.IsSystemMode = false;
                    oModelCfgTheme.IsLightMode = LightMode = false;
                    oModelCfgTheme.UpdatedBy = UserName;
                }
                else
                {
                    oModelCfgTheme = new CfgThemeMode();
                    oModelCfgTheme.FKUserId = UserID;
                    oModelCfgTheme.IsDarkMode = ThemeMode = DarkMode = true;
                    oModelCfgTheme.IsSystemMode = false;
                    oModelCfgTheme.IsLightMode = LightMode = false;
                    oModelCfgTheme.AddedBy = UserName;
                }
                res = await _cfgUser.Crud(oModelCfgTheme);
                if (res.Id > 0)
                {
                    Snackbar.Add("Theme Changed.", Severity.Success);
                }
                //Navigation.NavigateTo(Navigation.Uri, true);
                //await GetUserConfiguration();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async void SystemTheme()
        {
            try
            {
                var res = new APIResponseModel();
                if (oModelCfgTheme?.Id > 0)
                {
                    oModelCfgTheme.FKUserId = UserID;
                    SystemMode = await _mudThemeProvider.GetSystemPreference();
                    ThemeMode = SystemMode;
                    oModelCfgTheme.IsSystemMode = true;
                    oModelCfgTheme.IsLightMode = LightMode = false;
                    oModelCfgTheme.IsDarkMode = DarkMode = false;
                    oModelCfgTheme.UpdatedBy = UserName;
                }
                else
                {
                    oModelCfgTheme = new CfgThemeMode();
                    oModelCfgTheme.FKUserId = UserID;
                    SystemMode = await _mudThemeProvider.GetSystemPreference();
                    ThemeMode = SystemMode;
                    oModelCfgTheme.IsSystemMode = true;
                    oModelCfgTheme.IsLightMode = LightMode = false;
                    oModelCfgTheme.IsDarkMode = DarkMode = false;
                    oModelCfgTheme.AddedBy = UserName;
                }
                res = await _cfgUser.Crud(oModelCfgTheme);
                if (res.Id > 0)
                {
                    Snackbar.Add("Theme Changed.", Severity.Success);
                }
                //Navigation.NavigateTo(Navigation.Uri, true);
                //await GetUserConfiguration();
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
                await GetUserConfiguration();
                await GetUserAlert();
                await GetUserMessage();
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
                string Clause = $@" AND FKUserId = {UserID}";
                oModelCfgTheme = await _cfgUser.GetThemeSettingDataByClause(Clause);
                if (oModelCfgTheme?.Id > 0)
                {
                    LightMode = oModelCfgTheme.IsLightMode;
                    if (oModelCfgTheme.IsDarkMode)
                    {
                        ThemeMode = DarkMode = oModelCfgTheme.IsDarkMode;
                    }
                    if (oModelCfgTheme.IsSystemMode)
                    {
                        SystemMode = await _mudThemeProvider.GetSystemPreference();
                        ThemeMode = SystemMode;
                    }

                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetUserAlert()
        {
            try
            {
                //oListUserAlert = await _mstEmailNotificationPreferences.GetAllUserAlertDataByBusiness(BusinessID);
                NotificationCount = oListUserAlert.Where(x => x.UserKey == UserKey && !x.MarkAsRead).Count();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetUserMessage()
        {
            try
            {
                string Clause = $@" AND FKFromUserID = {UserID} or FKToUserID = {UserID})";
                oListUserMessage = await _mstUserMessage.GetUserMessage(Clause);
                if (oListUserMessage.Where(x => x.FktoUserId == UserID && x.MarkAsRead == false).DistinctBy(x => x.FkfromUserId).Count() > 0)
                {
                    MessageCount = oListUserMessage.Where(x => x.FktoUserId == UserID && x.MarkAsRead == false).DistinctBy(x => x.FkfromUserId).Count();
                }
                else
                {
                    MessageCount = 0;
                }
                //if (oListUserMessage.Where(x => x.FkfromUserId == UserID && x.MarkAsRead == false).DistinctBy(x => x.FktoUserId).Count() > 0)
                //{
                //    MessageCount = oListUserMessage.Where(x => x.FkfromUserId == UserID && x.MarkAsRead == false).DistinctBy(x => x.FktoUserId).Count();
                //}
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async void PasswordSetting()
        {
            await Task.Delay(1);
            Navigation.NavigateTo("PasswordSetting");
        }
        private async void AccountSetting()
        {
            await Task.Delay(1);
            Navigation.NavigateTo("AccountSetting");
        }

        #endregion

        #region Events

        protected async override Task OnInitializedAsync()
        {
            try
            {
                var authState = await _authState.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity.IsAuthenticated)
                {
                    UserID = Convert.ToInt32(user.Claims.Where(x => x.Type == "UserID").Select(x => x.Value).FirstOrDefault());
                    UserKey = user.Claims.Where(x => x.Type == "UserKey").Select(x => x.Value).FirstOrDefault().ToString();
                    BusinessID = Convert.ToInt32(user.Claims.Where(x => x.Type == "BusinessID").Select(x => x.Value).FirstOrDefault());

                    //_hubConnection = new HubConnectionBuilder().WithUrl(UIConfig.NotificationBaseURL).Build();
                    //_hubConnection.On<List<UserAlert>>("Alert", (IncomingAlert) =>
                    //{
                    //    NotificationCount = IncomingAlert.Where(x => x.UserKey == UserKey && !x.MarkAsRead).Count();
                    //    StateHasChanged();
                    //    if (NotificationCount > 0)
                    //    {
                    //        Snackbar.Add("You got new notification", Severity.Info);
                    //    }
                    //});
                    //await _hubConnection.StartAsync();

                    //_hubConnectionMessage = new HubConnectionBuilder().WithUrl(UIConfig.MessageBaseURL).Build();
                    //_hubConnectionMessage.On<List<MstUserMessage>>("Message", (IncomingMessage) =>
                    //{
                    //    if (IncomingMessage.Where(x => x.FktoUserId == UserID && x.MarkAsRead == false).DistinctBy(x => x.FkfromUserId).Count() > 0)
                    //    {
                    //        MessageCount = IncomingMessage.Where(x => x.FktoUserId == UserID && x.MarkAsRead == false).DistinctBy(x => x.FkfromUserId).Count();
                    //    }
                    //    else
                    //    {
                    //        MessageCount = 0;
                    //    }
                    //    //if (IncomingMessage.Where(x => x.FkfromUserId == UserID && x.MarkAsRead == false).DistinctBy(x => x.FktoUserId).Count() > 0)
                    //    //{
                    //    //    MessageCount = IncomingMessage.Where(x => x.FkfromUserId == UserID && x.MarkAsRead == false).DistinctBy(x => x.FktoUserId).Count();
                    //    //}
                    //    StateHasChanged();
                    //    if (MessageCount > 0)
                    //    {
                    //        Snackbar.Add("You got new message", Severity.Info);
                    //    }
                    //});
                    //await _hubConnectionMessage.StartAsync();
                    await CallAPI();
                    UserName = user.Claims.Where(x => x.Type == "Username").Select(x => x.Value).FirstOrDefault();
                    UserEmail = user.Claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
                    BusinessName = user.Claims.Where(x => x.Type == "BusinessName").Select(x => x.Value).FirstOrDefault();
                    BusinessLogo = user.Claims.Where(x => x.Type == "BusinessLogo").Select(x => x.Value).FirstOrDefault();
                }
                else
                {
                    Navigation.NavigateTo(Navigation.BaseUri + "/SignIn");
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        public async void Dispose()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
            if (_hubConnectionMessage is not null)
            {
                await _hubConnectionMessage.DisposeAsync();
            }
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
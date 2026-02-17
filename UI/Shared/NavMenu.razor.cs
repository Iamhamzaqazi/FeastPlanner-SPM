using AppDBContext.VMModels;

namespace UI.Shared
{
    public partial class NavMenu
    {
        #region Variables

        private int FKUserID = 0;
        private bool IsSuper = false;
        private string SearchString = "";
        private string UserName = "A";
        private string UserEmail = "";
        string URL = $@"{UIConfig.CRBaseURL}?AppType=2&ReportName=";

        private IEnumerable<VMMstUserAuthorization> AuthMenus { get; set; }
        private IEnumerable<VMMstUserAuthorization> FilteredMenus { get; set; }

        #endregion

        #region Function
        private string GetChildMenuIcon(string menuName)
        {
            return menuName.ToLower() switch
            {
                var name when name.Contains("user") => Icons.Material.Filled.People,
                var name when name.Contains("profile") => Icons.Material.Filled.AccountCircle,
                var name when name.Contains("setting") => Icons.Material.Filled.Settings,
                var name when name.Contains("report") => Icons.Material.Filled.Assessment,
                var name when name.Contains("analytics") => Icons.Material.Filled.Analytics,
                var name when name.Contains("payment") => Icons.Material.Filled.Payment,
                var name when name.Contains("booking") => Icons.Material.Filled.CalendarToday,
                var name when name.Contains("invoice") => Icons.Material.Filled.Receipt,
                _ => Icons.Material.Filled.Folder
            };
        }

        private bool IsNewFeature(string menuName)
        {
            var newFeatures = new[] { "AI", "Analytics", "Reports", "Dashboard" };
            return newFeatures.Any(f => menuName.Contains(f, StringComparison.OrdinalIgnoreCase));
        }
        private string GetMenuIcon(int id)
        {
            return id switch
            {
                1 => Icons.Material.Sharp.AdminPanelSettings,
                5 => Icons.Material.Sharp.DataExploration,
                7 => Icons.Material.Sharp.Savings,
                10 => Icons.Material.Sharp.AccountBalanceWallet,
                14 => Icons.Material.Sharp.Book,
                _ => Icons.Material.Outlined.Menu
            };
        }
        private async Task SearchMenu()
        {
            if (string.IsNullOrWhiteSpace(SearchString))
            {
                FilteredMenus = new List<VMMstUserAuthorization>();
                FilteredMenus = AuthMenus;
            }
            else
            {
                FilteredMenus = new List<VMMstUserAuthorization>();
                FilteredMenus = AuthMenus?.Where(x => x.PMenuName.ToLower().Contains(SearchString.ToLower()) || x.CMenuName.ToLower().Contains(SearchString.ToLower())).ToList();
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Events

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var authState = await _authState.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity.IsAuthenticated)
                {
                    FKUserID = Convert.ToInt32(user.Claims.Where(x => x.Type == "UserID").Select(x => x.Value).FirstOrDefault());
                    IsSuper = Convert.ToBoolean(user.Claims.Where(x => x.Type == "IsSuper").Select(x => x.Value).FirstOrDefault());
                    UserName = user.Claims.Where(x => x.Type == "Username").Select(x => x.Value).FirstOrDefault();
                    UserEmail = user.Claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
                    string Clause = $@" AND ""FKUSERID"" = '{FKUserID}'";
                    var res = await _mstUserAuthorization.GetAllData(Clause, IsSuper);
                    AuthMenus = res?.Where(x => x.UserRights != false).ToList();
                    FilteredMenus = AuthMenus;
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

        #endregion
    }
}
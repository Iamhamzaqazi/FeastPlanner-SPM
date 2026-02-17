using AppDBContext.VMModels;
using Microsoft.AspNetCore.Components.Routing;

namespace UI.Pages.Administration
{
    public partial class UserAuthorization
    {
        #region Variables 

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        private bool loading = false;
        private bool CheckedAll = false;
        private bool IsSet = false;

        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";

        MstUser oMstUser = new MstUser();
        private IEnumerable<MstUser> oMstUserList = new List<MstUser>();

        private IEnumerable<VMMstUserAuthorization> oVMUserAuthorizationList = new List<VMMstUserAuthorization>();

        List<MstUserAuthorization> oMstUserAuthorizationList = new List<MstUserAuthorization>();

        MudTable<VMMstUserAuthorization> TableRef { get; set; }

        #endregion

        #region Functions

        private TableGroupDefinition<VMMstUserAuthorization> _groupDefinition = new()
        {
            GroupName = "Module Name ",
            Indentation = false,
            Expandable = true,
            IsInitiallyExpanded = true,
            Selector = (e) => e.PMenuName

        };
        private async Task<IEnumerable<MstUser>> SearchUser(string SearchString)
        {
            List<MstUser> oList = new List<MstUser>();
            try
            {
                if (oMstUserList.Count() == 0)
                {
                    string Clause = $@"AND BusinessKey = '{BusinessKey}'";
                    oMstUserList = await _mstUser.GetAllData(Clause);
                    oMstUserList = oMstUserList.Where(x => x.UniqueKey != UserKey).ToList();
                }

                if (string.IsNullOrEmpty(SearchString))
                {
                    oList = oMstUserList.ToList();
                }
                else
                {
                    oList = oMstUserList.Where(x => x.Name.ToUpper().Contains(SearchString.ToUpper())).ToList();
                    oList.Select(x => new MstUser
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return oList;
        }
        private async Task GetAllUserAuthorizeMenu()
        {
            try
            {
                loading = true;
                if (oMstUser.Id > 0)
                {
                    string Clause = $@"AND FKUSERID = {oMstUser.Id}";
                    oVMUserAuthorizationList = await _mstUserAuthorization.GetAllData(Clause, oMstUser.IsSuper);
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
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
        public async Task SetModelValues()
        {
            try
            {
                await Task.Delay(1);
                if (oMstUser.Id > 0)
                {
                    foreach (var Authorize in oVMUserAuthorizationList)
                    {
                        var checkParent = oMstUserAuthorizationList.Where(x => x.MenuName == Authorize.MenuParentName).FirstOrDefault();
                        MstUserAuthorization oUserAuthorizationParent = new MstUserAuthorization();
                        oUserAuthorizationParent.Id = Authorize.ID;
                        oUserAuthorizationParent.FkuserId = oMstUser.Id;
                        oUserAuthorizationParent.MenuName = Authorize.CMenuName;
                        if (Authorize.UserRights)
                        {
                            oUserAuthorizationParent.UserRights = 2;
                        }
                        else
                        {
                            oUserAuthorizationParent.UserRights = 1;
                        }
                        oUserAuthorizationParent.FkmenuId = Authorize.CMenuID;
                        if (Authorize.ID == 0)
                        {
                            oUserAuthorizationParent.AddedBy = LoggedInUser;
                        }
                        else
                        {
                            oUserAuthorizationParent.UpdatedBy = LoggedInUser;
                        }
                        oMstUserAuthorizationList.Add(oUserAuthorizationParent);
                    }
                    IsSet = true;
                }
                else
                {
                    Snackbar.Add("Select user first.", Severity.Error);
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
                if (IsSet)
                {
                    res = await _mstUserAuthorization.Crud(oMstUserAuthorizationList);
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
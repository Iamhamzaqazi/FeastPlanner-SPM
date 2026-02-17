using AppDBContext.Models;
using AppDBContext.VMModels;
using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
using UI.Authentication;
using UI.Pages.Components;

namespace UI.Pages.Administration
{
    public partial class ProfileSetting
    {
        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        private bool loading = false;
        private bool IsSet = false;
        private bool ConfirmDisable = false;
        private string DialogFor = "";

        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";
        private string OldLogo = "";
        private string OldLogoPath = "";

        private int ProfileCompletion = 0;
        private double ProfileCompletionPercentage = 0;

        bool successBusiness;
        string[] errors = { };
        MudForm formBusiness;

        private string FileName = "";

        FluentValueValidator<string> cnValidator = new FluentValueValidator<string>(x => x
      .NotEmpty()
      .Length(1, 100)
      .Matches("^((\\+92)|(0092))-{0,1}\\d{3}-{0,1}\\d{7}$|^\\d{11}$|^\\d{4}-\\d{7}$")
      .WithMessage("Contact format incorrect"));
        private CultureInfo CulturePKR = CultureInfo.GetCultureInfo("ur-PK");

        MstBusiness oModel = new MstBusiness();

        MstCity oMstCity = new MstCity();
        private IEnumerable<MstCity> oCityList = new List<MstCity>();

        MstArea oMstArea = new MstArea();
        private IEnumerable<MstArea> oAreaList = new List<MstArea>();
        private IEnumerable<MstArea> oAreaListByCity = new List<MstArea>();

        private IEnumerable<MstBusinessLog> oListBusinessLogs = new List<MstBusinessLog>();

        private string searchString1 = "";

        #endregion

        #region Functions

        //private bool FilterFunc(MstBusinessEmployee element, string searchString1)
        //{
        //    if (string.IsNullOrWhiteSpace(searchString1))
        //        return true;
        //    if (element.EmployeeName.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.EmployeeContact.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.EmployeeSalary.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.EmployeeDateOfJoining.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.EmployeeAddress.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.IsActive.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    return false;
        //}
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
                DialogFor = "Deactivate Profile";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>("Deactivate Profile", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    var oUser = (MstUser)result.Data;
                    oUser.IsActive = false;
                    oUser.UpdatedBy = LoggedInUser;
                    var res = await _mstUser.Crud(oUser);
                    if (res.Id > 0)
                    {
                        Snackbar.Add("Profile Deactivated", MudBlazor.Severity.Success);
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
                await GetAllBusinessData();
                await GetAllBusinessDataLogs();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetAllBusinessData()
        {
            try
            {
                string Clause = $@" AND UniqueKey = '{BusinessKey}'";
                var obj = await _businessData.GetAllBusinessData(Clause);
                if (obj?.Count() > 0)
                {
                    oModel = obj.FirstOrDefault();
                    oMstCity.Name = oModel.City;
                    oMstArea.Name = oModel.Area;
                    OldLogo = $"{WebHostEnviroment.WebRootPath}\\{oModel.Logo}";
                    OldLogoPath = oModel.Logo;
                    if (!string.IsNullOrWhiteSpace(oModel.Logo))
                    {
                        ProfileCompletion += 1;
                    }
                    if (!string.IsNullOrWhiteSpace(oModel.BusinessName))
                    {
                        ProfileCompletion += 1;
                    }
                    if (oModel.BusinessStartingPrice > 0)
                    {
                        ProfileCompletion += 1;
                    }
                    if (!string.IsNullOrWhiteSpace(oModel.BusinessContact))
                    {
                        ProfileCompletion += 1;
                    }
                    if (!string.IsNullOrWhiteSpace(oModel.City))
                    {
                        ProfileCompletion += 1;
                    }
                    if (!string.IsNullOrWhiteSpace(oModel.Area))
                    {
                        ProfileCompletion += 1;
                    }
                    if (!string.IsNullOrWhiteSpace(oModel.BusinessAddress))
                    {
                        ProfileCompletion += 1;
                    }
                    ProfileCompletionPercentage = (Convert.ToDouble(ProfileCompletion) / Convert.ToDouble(UIConfig.TotalProfileCompletion)) * 100;
                }

            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetAllBusinessDataLogs()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}'";
                oListBusinessLogs = await _businessData.GetAllBusinessLogData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task<IEnumerable<MstCity>> SearchCity(string SearchString)
        {
            List<MstCity> oList = new List<MstCity>();
            try
            {
                if (oCityList.Count() == 0)
                {
                    oCityList = await _masterData.GetAllCityData("");
                }
                if (oAreaList.Count() == 0)
                {
                    oAreaList = await _masterData.GetAllAreaData("");
                }
                if (string.IsNullOrEmpty(SearchString))
                {
                    oList = oCityList.ToList();
                }
                else
                {
                    oList = oCityList.Where(x => x.Name.ToUpper().Contains(SearchString.ToUpper())).ToList();
                    oList.Select(x => new MstCity
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
        private async Task<IEnumerable<MstArea>> SearchArea(string SearchString)
        {
            List<MstArea> oList = new List<MstArea>();
            try
            {
                await Task.Delay(1);
                if (string.IsNullOrEmpty(SearchString))
                {
                    oList = oAreaListByCity.ToList();
                }
                else
                {
                    oList = oAreaListByCity.Where(x => x.Name.ToUpper().Contains(SearchString.ToUpper())).ToList();
                    oList.Select(x => new MstArea
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
        private async Task RedirectToMembership()
        {
            try
            {
                await Task.Delay(1);
                Navigation.NavigateTo(Navigation.BaseUri + "/MembershipPlan", true);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetAreaByCity()
        {
            try
            {
                if (oMstCity.Id > 0)
                {
                    await Task.Delay(1);
                    oAreaListByCity = oAreaList.Where(x => x.FKCityID == oMstCity.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async void OnInputFileChanged(InputFileChangeEventArgs e)
        {
            try
            {
                loading = true;
                if (e.File.ContentType.Contains("image"))
                {
                    FileName = e.File.Name;
                    string Path = "";
                    if (!string.IsNullOrWhiteSpace(FileName))
                    {
                        Stream stream;
                        FileStream fs;
                        string CreatePath = $"{WebHostEnviroment.WebRootPath}\\images\\attachments\\{oModel.BusinessName}";
                        if (!Directory.Exists(CreatePath))
                        {
                            Directory.CreateDirectory(CreatePath);
                            stream = e.File.OpenReadStream();
                            FileName = FileName.Replace(FileName, oModel.BusinessName + "-logo-" + DateTime.Now.ToFileTime() + "." + e.File.ContentType).Replace("image/", "");
                            Path = $"{CreatePath}\\{FileName}";
                            fs = System.IO.File.Create(Path);
                            stream = e.File.OpenReadStream(2000000);
                            await stream.CopyToAsync(fs);
                            stream.Close();
                            fs.Close();
                            oModel.Logo = $"\\images\\attachments\\{oModel.BusinessName}\\{FileName}";
                        }
                        else
                        {
                            stream = e.File.OpenReadStream();
                            FileName = FileName.Replace(FileName, oModel.BusinessName + "-logo-" + DateTime.Now.ToFileTime() + "." + e.File.ContentType).Replace("image/", "");
                            Path = $"{CreatePath}\\{FileName}";
                            fs = System.IO.File.Create(Path);
                            stream = e.File.OpenReadStream(2000000);
                            await stream.CopyToAsync(fs);
                            stream.Close();
                            fs.Close();
                            oModel.Logo = $"\\images\\attachments\\{oModel.BusinessName}\\{FileName}";
                        }
                    }
                }
                else
                {
                    Snackbar.Add("Only .jpg, .jpeg, .png files are allowed.", MudBlazor.Severity.Error);
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                loading = false;
            }
            _ = InvokeAsync(StateHasChanged);
        }
        public void SetModelValues()
        {
            try
            {
                oModel.UpdatedBy = LoggedInUser;
                IsSet = true;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
                IsSet = false;
            }
        }
        private async Task<APIResponseModel> Save()
        {
            var res = new APIResponseModel();
            try
            {
                loading = true;
                SetModelValues();
                await formBusiness.Validate();
                if (IsSet && successBusiness)
                {
                    res = await _businessData.Crud(oModel);
                    if (res.Id > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(OldLogo) && OldLogoPath != oModel.Logo)
                        {
                            System.IO.File.Delete(OldLogo);
                        }
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
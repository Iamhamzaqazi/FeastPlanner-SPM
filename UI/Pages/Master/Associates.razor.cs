using AppDBContext.VMModels;
using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
using UI.Pages.Components;
using Severity = MudBlazor.Severity;

namespace UI.Pages.Master
{
    public partial class Associates
    {
        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.ExtraLarge, CloseButton = true, DisableBackdropClick = true };
        private string DialogFor = "";

        private bool loading = false;
        private bool IsSet = false;
        private bool IsEdit = false;

        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";

        private string OldLogo = "";
        private string OldLogoPath = "";
        private string FileName = "";

        FluentValueValidator<string> cnValidator = new FluentValueValidator<string>(x => x
      .NotEmpty()
      .Length(1, 100)
      .Matches("^((\\+92)|(0092))-{0,1}\\d{3}-{0,1}\\d{7}$|^\\d{11}$|^\\d{4}-\\d{7}$")
      .WithMessage("Contact format incorrect"));

        private CultureInfo CulturePKR = CultureInfo.GetCultureInfo("ur-PK");

        bool successUser;
        string[] errors = { };
        MudForm formUser;

        private IEnumerable<CfgDefaultValue> oCfgDefaultValueList = new List<CfgDefaultValue>();

        MstCity oMstCity = new MstCity();
        private IEnumerable<MstCity> oCityList = new List<MstCity>();

        MstArea oMstArea = new MstArea();
        private IEnumerable<MstArea> oAreaList = new List<MstArea>();
        private IEnumerable<MstArea> oAreaListByCity = new List<MstArea>();

        MstAssociates oModel = new MstAssociates();

        private IEnumerable<MstAssociates> oList = new List<MstAssociates>();
        private List<MstAssociatesAvailability> oListAvailability = new List<MstAssociatesAvailability>();
        private List<MstAssociatesPackages> oListPackages = new List<MstAssociatesPackages>();

        private string searchString1 = "";

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
        private async Task<IEnumerable<MstCity>> SearchCity(string SearchString)
        {
            List<MstCity> oList = new List<MstCity>();
            try
            {
                if (oCityList?.Count() == 0)
                {
                    oCityList = await _masterData.GetAllCityData("");
                }
                if (oAreaList?.Count() == 0)
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

        private TableGroupDefinition<MstAssociatesAvailability> _groupDefinition = new()
        {
            GroupName = "Days",
            Indentation = false,
            Expandable = true,
            IsInitiallyExpanded = true,
            Selector = (e) => e.AvailableDays

        };
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
        public void DeleteRecord(int Id)
        {
            try
            {
                var res = oListPackages.Where(x => x.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    oListPackages = oListPackages.Where(x => x.Id != Id).ToList();
                }
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
                await GetAllAssociates();
                await GetAllDefaultValue();
                SetAvailbilityList();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetAllAssociates()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}'";
                OldLogo = $"{WebHostEnviroment.WebRootPath}\\{oModel.Logo}";
                OldLogoPath = oModel.Logo;
                oList = await _masterData.GetAllAssociatesData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetAllDefaultValue()
        {
            try
            {
                string Clause = "AND IsActive = 'True'";
                oCfgDefaultValueList = await _masterData.GetAllDefaultValueData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        public void SetAvailbilityList()
        {
            try
            {
                if (oCfgDefaultValueList.Count() > 0)
                {
                    foreach (var defaultValueTimeSlots in oCfgDefaultValueList.Where(x => x.Type == "TimeSlots"))
                    {
                        foreach (var defaultValueDays in oCfgDefaultValueList.Where(x => x.Type == "AvailabilityDays"))
                        {
                            MstAssociatesAvailability availability = new MstAssociatesAvailability();
                            availability.TimeSlots = defaultValueTimeSlots.Name;
                            availability.AvailableDays = defaultValueDays.Name;
                            oListAvailability.Add(availability);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        public async Task AddPackageRow()
        {
            try
            {
                await Task.Delay(1);
                MstAssociatesPackages packages = new MstAssociatesPackages();
                if (oListPackages.Any(x => string.IsNullOrWhiteSpace(x.ItemName) && x.ItemsCount == 0 && x.ItemPrice == 0 && x.MinHead == 0))
                {
                    Snackbar.Add("Fill the detail first", Severity.Error);
                }
                else
                {
                    packages.Id = oListPackages.Count() + 1;
                    oListPackages.Add(packages);
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        public void SetModelValues()
        {
            try
            {
                if (oModel.Id == 0)
                {
                    oModel.BusinessKey = BusinessKey;
                    oModel.AddedBy = LoggedInUser;
                    oListPackages.ForEach(x =>
                    {
                        x.AddedBy = LoggedInUser;
                        x.BusinessKey = BusinessKey;
                        x.AssociateKey = null;
                    });
                    oListAvailability.ForEach(x =>
                    {
                        x.AddedBy = LoggedInUser;
                        x.BusinessKey = BusinessKey;
                    });
                    IsSet = true;
                }
                else
                {
                    oModel.BusinessKey = BusinessKey;
                    oModel.UpdatedBy = LoggedInUser;
                    oListPackages.ForEach(x =>
                    {
                        x.UpdatedBy = LoggedInUser;
                        x.BusinessKey = BusinessKey;
                    });
                    oListAvailability.ForEach(x =>
                    {
                        x.UpdatedBy = LoggedInUser;
                        x.BusinessKey = BusinessKey;
                    });
                    IsSet = true;
                }
                oModel.Area = oMstArea.Name;
                oModel.City = oMstCity.Name;
                oModel.AssociatesPackages = oListPackages;
                oModel.AssociatesAvailability = oListAvailability;
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
                SetModelValues();
                await formUser.Validate();
                if (IsSet && successUser)
                {
                    res = await _masterData.Crud(oModel);
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
        private async Task OpenDialog(DialogOptions options)
        {
            try
            {
                DialogFor = "MstAssociates";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>("Associates", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    oModel = (MstAssociates)result.Data;
                    if (oCityList?.Count() == 0)
                    {
                        oCityList = await _masterData.GetAllCityData("");
                    }
                    if (oAreaList?.Count() == 0)
                    {
                        oAreaList = await _masterData.GetAllAreaData("");
                    }
                    oMstCity = oCityList.Where(x => x.Name == oModel.City).FirstOrDefault();
                    oMstArea = oAreaList.Where(x => x.Name == oModel.Area).FirstOrDefault();
                    oListPackages = oModel.AssociatesPackages;
                    oListAvailability = oModel.AssociatesAvailability;
                    IsEdit = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
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
                    oModel.CategoryType = "Banquets";
                    oModel.MinGathering = 0;
                    oModel.MaxGathering = 0;
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
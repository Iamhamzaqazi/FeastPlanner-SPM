using AppDBContext.Interfaces.MasterData;
using AppDBContext.VMModels;
using FluentValidation;
using System.Globalization;
using UI.Pages.Components;
using static MudBlazor.CategoryTypes;

namespace UI.Pages.Booking
{
    public partial class AddBooking
    {
        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.ExtraLarge, CloseButton = true, DisableBackdropClick = true };
        private string DialogFor = "";


        private bool EmailExist = false;
        private bool NICExist = false;
        private bool loading = false;
        private bool IsSet = false;
        private bool IsEdit = false;
        private bool DialogMulti = false;

        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";

        private CultureInfo CulturePKR = CultureInfo.GetCultureInfo("ur-PK");

        bool successUser;
        string[] errors = { };
        MudForm formUser;

        private IEnumerable<CfgDefaultValue> oCfgDefaultValueList = new List<CfgDefaultValue>();


        MstBusinessCustomer oModelCustomer = new MstBusinessCustomer();
        TrnsPackages oModel = new TrnsPackages();
        TrnsPackagesDetail oModelDetail = new TrnsPackagesDetail();

        private IEnumerable<TrnsPackages> oList = new List<TrnsPackages>();
        private List<TrnsPackagesDetail> oListPackagesDetail = new List<TrnsPackagesDetail>();
        private List<TrnsPackagesDetail2> oListPackagesDetail2 = new List<TrnsPackagesDetail2>();

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

        FluentValueValidator<string> NICValidator = new FluentValueValidator<string>(x => x
        .NotEmpty()
        .Length(1, 100)
        .Matches("^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$")
        .WithMessage("NIC format incorrect"));

        FluentValueValidator<string> cnValidator = new FluentValueValidator<string>(x => x
        .NotEmpty()
        .Length(1, 100)
        .Matches("^((\\+92)|(0092))-{0,1}\\d{3}-{0,1}\\d{7}$|^\\d{11}$|^\\d{4}-\\d{7}$")
        .WithMessage("Contact format incorrect"));

        private async Task<bool> CheckEmail()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(oModelCustomer.CustomerEmail))
                {
                    var res = await _authenticate.CheckEmail(oModelCustomer.CustomerEmail);
                    if (res.Id == 0)
                    {
                        EmailExist = false;
                    }
                    else
                    {
                        EmailExist = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return EmailExist;
        }
        private async Task<bool> CheckNIC()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(oModelCustomer.CustomerNIC))
                {
                    var res = await _authenticate.CheckEmail(oModelCustomer.CustomerNIC);
                    if (res.Id == 0)
                    {
                        NICExist = false;
                    }
                    else
                    {
                        NICExist = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return NICExist;
        }

        public void EditRecord(int Id)
        {
            try
            {
                var res = oList.Where(x => x.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    oModel = res;
                    //oList = oList.Where(x => x.Id != Id);
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
                await GetAllPackages();
                await GetAllDefaultValue();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetAllPackages()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}'";
                oList = await _masterData.GetAllPackagesData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        public async Task AddPackageRow()
        {
            try
            {
                await Task.Delay(1);
                oModelDetail = new TrnsPackagesDetail();
                if (oListPackagesDetail.Any(x => string.IsNullOrWhiteSpace(x.AssociateKey) && string.IsNullOrWhiteSpace(x.AssociateAvailabilityKey) && x.MinGathering == 0 && x.MaxGathering == 0 && x.Price == 0))
                {
                    Snackbar.Add("Fill the detail first", MudBlazor.Severity.Error);
                }
                else
                {
                    oModelDetail.Id = oListPackagesDetail.Count() + 1;
                    oListPackagesDetail.Add(oModelDetail);
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        public void DeleteRecord(int Id)
        {
            try
            {
                var res = oListPackagesDetail.Where(x => x.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    oListPackagesDetail = oListPackagesDetail.Where(x => x.Id != Id).ToList();
                }
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
        public void SetModelValues()
        {
            try
            {
                if (oModel.Id == 0)
                {
                    oModel.BusinessKey = BusinessKey;
                    oModel.AddedBy = LoggedInUser;
                    oListPackagesDetail.ForEach(x =>
                    {
                        x.AddedBy = LoggedInUser;
                        x.BusinessKey = BusinessKey;
                    });
                    oModel.PackagesDetail = oListPackagesDetail;
                    IsSet = true;
                }
                else
                {
                    oModel.BusinessKey = BusinessKey;
                    oModel.UpdatedBy = LoggedInUser;
                    oListPackagesDetail.ForEach(x =>
                    {
                        x.UpdatedBy = LoggedInUser;
                    });
                    oModel.PackagesDetail = oListPackagesDetail;
                    IsSet = true;
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
                DialogFor = "TrnsPackages";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>("Packages", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    oModel = (TrnsPackages)result.Data;
                    oListPackagesDetail = oModel.PackagesDetail;
                    IsEdit = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task OpenDialogAssociates(DialogOptions options)
        {
            try
            {
                DialogFor = "ActiveMstAssociates";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                parameters.Add("CategoryType", oModel.CategoryType);
                DialogMulti = false;
                parameters.Add("DialogMulti", DialogMulti);
                var dialog = Dialog.Show<DialogBox>("Associates", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    var oModelAssociates = (MstAssociates)result.Data;
                    if (oModelAssociates?.Id > 0)
                    {
                        oModelDetail.AssociateKey = oModelAssociates.Id.ToString();
                        oModelDetail.AssociateBusinessName = oModelAssociates.BusinessName;
                    }
                    IsEdit = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task OpenDialogAssociatesAvailability(DialogOptions options, string DocEntry)
        {
            try
            {
                DialogFor = "ActiveMstAssociatesAvailability";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                parameters.Add("DocEntry", DocEntry);
                DialogMulti = false;
                parameters.Add("DialogMulti", DialogMulti);
                var dialog = Dialog.Show<DialogBox>("Availability", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    var oModelAssociatesAvailability = (MstAssociatesAvailability)result.Data;
                    if (oModelAssociatesAvailability?.Id > 0)
                    {
                        oModelDetail.AssociateAvailabilityKey = oModelAssociatesAvailability.UniqueKey;
                        oModelDetail.AvailableDays = oModelAssociatesAvailability.AvailableDays;
                        oModelDetail.TimeSlots = oModelAssociatesAvailability.TimeSlots;
                    }
                    IsEdit = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task OpenDialogAssociatesPackages(DialogOptions options, string DocEntry)
        {
            try
            {
                DialogFor = "ActiveMstAssociatesPackages";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                parameters.Add("DocEntry", DocEntry);
                DialogMulti = false;
                parameters.Add("DialogMulti", DialogMulti);
                var dialog = Dialog.Show<DialogBox>("Packages", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    var oModelAssociatesPackages = (MstAssociatesPackages)result.Data;
                    if (oModelAssociatesPackages?.Id > 0)
                    {
                        oModelDetail.AssociatePackagesKey = oModelAssociatesPackages.UniqueKey;
                        oModelDetail.ItemName = oModelAssociatesPackages.ItemName;
                        oModelDetail.MinGathering = (decimal)oModelAssociatesPackages.MinHead;
                        oModelDetail.RatePerHead = (decimal)oModelAssociatesPackages.ItemPrice;
                    }
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

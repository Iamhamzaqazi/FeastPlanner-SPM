using AppDBContext.Interfaces.MasterData;
using AppDBContext.VMModels;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Diagnostics;
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

        [Parameter]
        public string Key { get; set; }


        private bool EmailExist = false;
        private bool NICExist = false;
        private bool loading = false;
        private bool IsSet = false;
        private bool IsEdit = false;
        private bool DialogMulti = false;

        private int TabIndex { get; set; }
        private bool DisabledTime = false;
        private List<DateTime> EventDates = new List<DateTime>();
        private int InvoiceNo = 0;

        bool successCustomer;
        bool successBooking;
        bool successPayment;
        bool successInvoice;

        MudForm formCustomer;
        MudForm formBooking;
        MudForm formPayment;
        MudForm formInvoice;

        string[] errors = { };

        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";

        private CultureInfo CulturePKR = CultureInfo.GetCultureInfo("ur-PK");



        private IEnumerable<CfgDefaultValue> oCfgDefaultValueList = new List<CfgDefaultValue>();
        private IEnumerable<MstAssociates> oListAssociates = new List<MstAssociates>();
        private IEnumerable<MstAssociatesAvailability> oListAssociatesAvailability = new List<MstAssociatesAvailability>();
        private IEnumerable<MstFacility> oListFacility = new List<MstFacility>();


        MstBusinessCustomer oModelCustomer = new MstBusinessCustomer();
        TrnsBusinessBooking oModel = new TrnsBusinessBooking();
        TrnsBusinessBookingDetail oModelDetail = new TrnsBusinessBookingDetail();
        TrnsBusinessBookingPayment oModelPayment = new TrnsBusinessBookingPayment();

        private IEnumerable<TrnsBusinessBooking> oList = new List<TrnsBusinessBooking>();
        private List<TrnsBusinessBookingDetail> oBookingDetail = new List<TrnsBusinessBookingDetail>();
        private List<TrnsBusinessBookingPayment> oBookingPayment = new List<TrnsBusinessBookingPayment>();

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
        public async void Next(int Index)
        {
            try
            {
                await formCustomer.Validate();
                await formBooking.Validate();
                await formPayment.Validate();
                await formInvoice.Validate();
                switch (Index)
                {
                    case 0:
                        if (successCustomer && !EmailExist)
                        {
                            TabIndex = 1;
                        }
                        else
                        {
                            Snackbar.Add("Email already exist", MudBlazor.Severity.Error);
                        }
                        break;
                    case 1:
                        if (!successBooking && oBookingDetail.Count > 0)
                        {
                            TabIndex = 2;
                        }
                        if (successBooking)
                        {
                            if (oBookingDetail.Count > 0)
                            {
                                TabIndex = 2;
                            }
                            else
                            {
                                Snackbar.Add("Add item into cart first.", MudBlazor.Severity.Info);
                            }
                        }
                        else
                        {
                            Snackbar.Add("Please fill the required field(s)", MudBlazor.Severity.Error);
                        }
                        break;
                    case 2:
                        if (successPayment)
                        {
                            TabIndex = 3;
                            //await GetInvoiceNo();
                        }
                        else
                        {
                            Snackbar.Add("Please fill the required field(s)", MudBlazor.Severity.Error);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        public async void Back(int Index)
        {
            try
            {
                await formCustomer.Validate();
                await formBooking.Validate();
                await formPayment.Validate();
                await formInvoice.Validate();
                switch (Index)
                {
                    case 0:
                        TabIndex = 0;
                        break;
                    case 1:
                        TabIndex = 1;
                        if (oBookingDetail.Count > 0)
                        {
                            successBooking = true;
                        }
                        else
                        {
                            successBooking = false;
                        }
                        break;
                    case 2:
                        TabIndex = 2;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private string CheckDate(DateTime date)
        {
            try
            {
                //if (oListDetailFunctionDate != null && oListDetailFunctionDate.Count() > 0)
                //{
                //    var Date = oListDetailFunctionDate.GroupBy(p => p.FunctionDate)
                //                                       .Select(g => new
                //                                       {
                //                                           FunctionDate = g.Key,
                //                                           TimeofEvent = g.Select(x => x.TimeOfEvent),
                //                                           RecordCount = g.Count()
                //                                       }).OrderBy(x => x.FunctionDate);
                //    if (Date != null && Date.Count() > 0)
                //    {
                //        var SelectedRow = Date.Where(x => x.FunctionDate == date.Date).FirstOrDefault();
                //        if (SelectedRow != null)
                //        {
                //            if (EventDates.Contains(date.Date) && SelectedRow.RecordCount == 2)
                //            {
                //                return "day-booked";
                //            }
                //            else if (EventDates.Contains(date.Date) && SelectedRow.RecordCount == 1)
                //            {
                //                return "day-booked-partial";
                //            }
                //            else
                //            {
                //                return string.Empty;
                //            }
                //        }
                //        else
                //        {
                //            return string.Empty;
                //        }
                //    }
                //    else
                //    {
                //        return string.Empty;
                //    }
                //}
                //else
                //{
                //    return string.Empty;
                //}
                return string.Empty;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                return string.Empty;
            }
        }
        private async void PickValue()
        {
            try
            {
                //await GetAllFunctionDates();
                //if (oListDetailFunctionDate != null && oListDetailFunctionDate.Count() > 0)
                //{
                //    var Date = oListDetailFunctionDate.GroupBy(p => p.FunctionDate)
                //                                       .Select(g => new
                //                                       {
                //                                           FunctionDate = g.Key,
                //                                           TimeofEvent = g.Select(x => x.TimeOfEvent),
                //                                           RecordCount = g.Count()
                //                                       }).OrderBy(x => x.FunctionDate);
                //    if (Date != null && Date.Count() > 0)
                //    {
                //        var SelectedRow = Date.Where(x => x.FunctionDate == oModelDetail.EventDate).FirstOrDefault();
                //        if (SelectedRow != null && !string.IsNullOrWhiteSpace(SelectedRow.TimeofEvent.ToString()))
                //        {
                //            DisabledTime = true;
                //            string Time = SelectedRow.TimeofEvent.FirstOrDefault().ToString();
                //            if (Time == "Day")
                //            {
                //                oModelDetail.TimeOfEvent = "Night";
                //            }
                //            else if (Time == "Night")
                //            {
                //                oModelDetail.TimeOfEvent = "Day";
                //            }
                //            else
                //            {
                //                DisabledTime = false;
                //                oModelDetail.TimeOfEvent = "";
                //            }
                //        }
                //        else
                //        {
                //            DisabledTime = false;
                //            oModelDetail.TimeOfEvent = "";
                //        }
                //    }
                //    else
                //    {
                //        DisabledTime = false;
                //        oModelDetail.TimeOfEvent = "";
                //    }
                //}
                //else
                //{
                //    DisabledTime = false;
                //    oModelDetail.TimeOfEvent = "";
                //}
                _ = InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
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
                await GetAllBooking();
                await GetAllDefaultValue();
                await GetAllFacility();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetAllBooking()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}' and CustomerKey = '{Key}'";
                oList = await _trnsBooking.GetAllBusinessBookingData(Clause);
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
                if (!string.IsNullOrWhiteSpace(oModelDetail.CategoryType))
                {
                    string Clause = $@" AND BusinessKey = '{BusinessKey}' and CategoryType = '{oModelDetail.CategoryType}'";
                    oListAssociates = await _masterData.GetAllAssociatesData(Clause);
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }

            //_ = InvokeAsync(StateHasChanged);
        }
        private async Task GetAllAssociatesAvailability()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(oModelDetail.AssociateKey))
                {
                    string Clause = $@" AND BusinessKey = '{BusinessKey}' and DocEntry = '{oModelDetail.AssociateKey}'";
                    oListAssociatesAvailability = await _masterData.GetAllAssociatesAvailabilityData(Clause);
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            //_ = InvokeAsync(StateHasChanged);
        }
        private async Task GetAllFacility()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}' and IsActive = 'True'";
                oListFacility = await _masterData.GetAllFacilityData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            //_ = InvokeAsync(StateHasChanged);
        }
        public async Task AddBookingDetailRow()
        {
            try
            {
                await Task.Delay(1);
                if (oBookingDetail.Any(x => string.IsNullOrWhiteSpace(x.AssociateKey) && string.IsNullOrWhiteSpace(x.AssociateAvailabilityKey) && x.Gathering == 0 && x.FoodItems == 0))
                {
                    Snackbar.Add("Fill the detail first", MudBlazor.Severity.Error);
                }
                else
                {
                    oModelDetail.Id = oBookingDetail.Count() + 1;
                    oBookingDetail.Add(oModelDetail);
                    oModelDetail = new TrnsBusinessBookingDetail();
                    successBooking = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private void CalculateRemainingAmount()
        {
            try
            {
                oModelPayment.Remaining = oModelPayment.Amount - oModelPayment.Deposit;
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
                var res = oBookingDetail.Where(x => x.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    oBookingDetail = oBookingDetail.Where(x => x.Id != Id).ToList();
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
                if (oModelCustomer.Id == 0)
                {
                    oModelCustomer.IsBookingCompleted = true;
                    oModelCustomer.BusinessKey = BusinessKey;
                    oModelCustomer.AddedBy = LoggedInUser;
                }
                else
                {
                    oModelCustomer.UpdatedBy = LoggedInUser;
                }
                if (oModel.Id == 0)
                {
                    oModel.BusinessKey = BusinessKey;
                    oModel.AddedBy = LoggedInUser;
                    oBookingDetail.ForEach(x =>
                    {
                        x.AddedBy = LoggedInUser;
                        x.BusinessKey = BusinessKey;
                    });
                    oBookingPayment.Add(oModelPayment);
                    oBookingPayment.ForEach(x =>
                    {
                        x.AddedBy = LoggedInUser;
                        x.BusinessKey = BusinessKey;
                    });
                    oModel.oBookingDetail = oBookingDetail;
                    oModel.oBookingPayment = oBookingPayment;
                    IsSet = true;
                }
                else
                {
                    oModel.BusinessKey = BusinessKey;
                    oModel.UpdatedBy = LoggedInUser;
                    oBookingDetail.ForEach(x =>
                    {
                        x.UpdatedBy = LoggedInUser;
                        x.BusinessKey = BusinessKey;
                    });
                    oBookingPayment.Add(oModelPayment);
                    oBookingPayment.ForEach(x =>
                    {
                        x.UpdatedBy = LoggedInUser;
                        x.BusinessKey = BusinessKey;
                    });
                    oModel.oBookingDetail = oBookingDetail;
                    oModel.oBookingPayment = oBookingPayment;
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
                await formCustomer.Validate();
                await formPayment.Validate();
                if (IsSet && successCustomer && successBooking && successPayment)
                {
                    CalculateRemainingAmount();
                    oModelCustomer.IsBookingCompleted = true;
                    res = await _masterData.Crud(oModelCustomer);
                    if (res.Id > 0)
                    {
                        oModel.CustomerKey = oModelCustomer.UniqueKey;
                        oModel.oBookingDetail.ForEach(x =>
                        {
                            x.CustomerKey = oModelCustomer.UniqueKey;
                        });
                        oModel.oBookingPayment.ForEach(x =>
                        {
                            x.CustomerKey = oModelCustomer.UniqueKey;
                        });
                        res = await _trnsBooking.Crud(oModel);
                        if (res.Id > 0)
                        {
                            Snackbar.Add(res.Message, MudBlazor.Severity.Success);
                            Navigation.NavigateTo($"AddBooking", true);
                        }
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
                DialogFor = "TrnsBooking";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>("Booking", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    oModel = (TrnsBusinessBooking)result.Data;
                    oBookingDetail = oModel.oBookingDetail;
                    oModelPayment = oModel.oBookingPayment.FirstOrDefault();
                    IsEdit = true;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task OpenDialogFacility(DialogOptions options)
        {
            try
            {
                DialogFor = "ActiveMstFacility";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                DialogMulti = false;
                parameters.Add("DialogMulti", DialogMulti);
                var dialog = Dialog.Show<DialogBox>("Facility", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    var oModelFacility = (MstFacility)result.Data;
                    if (oModelFacility?.Id > 0)
                    {
                        oModelDetail.FacilityKey = oModelFacility.UniqueKey.ToString();
                        oModelDetail.Facility = oModelFacility.Name.ToString();
                    }
                    IsEdit = true;
                }

                _ = InvokeAsync(StateHasChanged);
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
                parameters.Add("CategoryType", oModelDetail.CategoryType);
                DialogMulti = false;
                parameters.Add("DialogMulti", DialogMulti);
                var dialog = Dialog.Show<DialogBox>("Associates", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    var oModelAssociates = (MstAssociates)result.Data;
                    if (oModelAssociates?.Id > 0)
                    {
                        oModelDetail.AssociateKey = oModelAssociates.DocEntry.ToString();
                        oModelDetail.AssociateName = oModelAssociates.BusinessName.ToString();
                    }
                    IsEdit = true;
                }

                _ = InvokeAsync(StateHasChanged);
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
                        oModelDetail.AssociateAvailabilityKey = oModelAssociatesAvailability.DocEntry.ToString();
                        oModelDetail.AssociateAvailability = $@"{oModelAssociatesAvailability.AvailableDays} - {oModelAssociatesAvailability.TimeSlots}";
                    }
                    IsEdit = true;
                }
                _ = InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
        }
        private async Task GetBusinessCustomer()
        {
            try
            {
                //string Clause = $@" AND BusinessKey = '{BusinessKey}' and IsBookingCompleted = 'True'";
                string Clause = $@" AND BusinessKey = '{BusinessKey}' AND UniqueKey = '{Key}'";
                var oCustomer = await _masterData.GetAllBusinessCustomerData(Clause);
                if (oCustomer != null && oCustomer.Count() > 0)
                {
                    oModelCustomer = oCustomer.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task EditByCustomerKey()
        {
            try
            {
                await GetBusinessCustomer();
                if (oModelCustomer != null && oModelCustomer.Id > 0)
                {
                    oModel = oList.FirstOrDefault();
                    oBookingDetail = oModel.oBookingDetail.ToList();
                    InvoiceNo = oModel.Id;
                    successCustomer = true;
                    successBooking = true;
                    successPayment = true;
                    successInvoice = true;
                    oModelPayment = oModel.oBookingPayment.FirstOrDefault();
                }
                else
                {
                    Snackbar.Add("No Customer found, Invalid Key", MudBlazor.Severity.Error);
                    Navigation.NavigateTo("/AddBooking", true);
                }
                _ = InvokeAsync(StateHasChanged);
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
                    formCustomer = new MudForm();
                    formBooking = new MudForm();
                    formPayment = new MudForm();
                    formInvoice = new MudForm();
                    await CallAPI();
                    if (!string.IsNullOrWhiteSpace(Key))
                    {
                        await EditByCustomerKey();
                    }
                    else
                    {
                        oModelCustomer.IsActive = true;
                    }
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
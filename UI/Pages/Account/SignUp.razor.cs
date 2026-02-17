using AppDBContext.Models;
using AppDBContext.VMModels;
using FluentValidation;
using System.Globalization;
using System.Text.RegularExpressions;
using UI.Pages.Components;

namespace UI.Pages.Account
{
    public partial class SignUp
    {
        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        private bool loading = false;
        private bool EmailExist = false;
        private bool ContactExist = false;
        private bool PasswordVisible = false;
        private bool AcceptLicense = false;

        private string DialogFor = "";

        bool successUser;
        bool successBusiness;
        string[] errors = { };
        MudTextField<string> pwField1;
        MudForm formUser;
        MudForm formBusiness;

        FluentValueValidator<string> ccValidator = new FluentValueValidator<string>(x => x
        .NotEmpty()
        .Length(1, 100)
        .CreditCard()
        .WithMessage("Credit Card format incorrect"));

        FluentValueValidator<string> cnValidator = new FluentValueValidator<string>(x => x
       .NotEmpty()
       .Length(1, 100)
       .Matches("^((\\+92)|(0092))-{0,1}\\d{3}-{0,1}\\d{7}$|^\\d{11}$|^\\d{4}-\\d{7}$")
       .WithMessage("Contact format incorrect"));

        private CultureInfo CulturePKR = CultureInfo.GetCultureInfo("ur-PK");
        private int TabIndex { get; set; }
        private bool StepUserInfo { get; set; } = true;
        private bool StepBusinessInfo { get; set; }
        private Size TimeLineUserSize { get; set; } = Size.Large;
        private Size TimeLineBusinessSize { get; set; } = Size.Medium;

        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        SignUpRequest oModel = new SignUpRequest();
        MstBusiness oMstBusiness = new MstBusiness();

        MstUser oMstUser = new MstUser();

        MstCity oMstCity = new MstCity();
        private IEnumerable<MstCity> oCityList = new List<MstCity>();

        MstArea oMstArea = new MstArea();
        private IEnumerable<MstArea> oAreaList = new List<MstArea>();
        private IEnumerable<MstArea> oAreaListByCity = new List<MstArea>();

        UserAlert oUserAlert = new UserAlert();
        MstBusinessLog oBusinessLog = new MstBusinessLog();

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

        public async Task Next(int Index)
        {

            switch (Index)
            {
                case 0:
                    await CheckEmail();
                    if (successUser && !EmailExist)
                    //if (successUser && !EmailExist && !ContactExist)
                    {
                        TabIndex = 1;
                        TimeLineBusinessSize = Size.Large;
                        TimeLineUserSize = Size.Medium;
                        StepBusinessInfo = true;
                        StepUserInfo = false;
                    }
                    else
                    {
                        if (EmailExist)
                        {
                            Snackbar.Add("Email already exist", MudBlazor.Severity.Error);
                            return;
                        }
                        //if (ContactExist)
                        //{
                        //    Snackbar.Add("Contact already exist", MudBlazor.Severity.Error);
                        //    return;
                        //}
                    }
                    oMstCity = new MstCity();
                    oMstArea = new MstArea();
                    break;
                case 1:
                    if (successBusiness)
                    {
                        TabIndex = 2;
                        TimeLineBusinessSize = Size.Medium;
                        StepBusinessInfo = false;
                    }
                    else
                    {
                        Snackbar.Add("Please fill the required field(s)", MudBlazor.Severity.Error);
                    }
                    break;
            }
        }

        public void Back(int Index)
        {
            switch (Index)
            {
                case 0:
                    TabIndex = 0;
                    TimeLineBusinessSize = Size.Medium;
                    TimeLineUserSize = Size.Large;
                    StepBusinessInfo = false;
                    StepUserInfo = true;
                    break;
                case 1:
                    TabIndex = 1;
                    TimeLineBusinessSize = Size.Large;
                    StepUserInfo = true;
                    oMstCity = new MstCity();
                    oMstArea = new MstArea();
                    break;
            }
        }

        private async Task<bool> CheckEmail()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(oMstUser.Email))
                {
                    var res = await _authenticate.CheckEmail(oMstUser.Email);
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
        private async Task<bool> CheckContact()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(oMstUser.Contact))
                {
                    var res = await _authenticate.CheckContact(oMstUser.Contact);
                    if (res.Id == 0)
                    {
                        ContactExist = false;
                    }
                    else
                    {
                        ContactExist = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return ContactExist;
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

        public bool SetModelValues()
        {
            bool IsSet = false;
            try
            {

                if (!string.IsNullOrWhiteSpace(oMstCity.Name))
                {
                    oMstBusiness.City = oMstCity.Name;
                }
                else
                {
                    IsSet = false;
                    Snackbar.Add("City required", MudBlazor.Severity.Error);
                    return IsSet;
                }
                if (!string.IsNullOrWhiteSpace(oMstArea.Name))
                {
                    oMstBusiness.Area = oMstArea.Name;
                }
                else
                {
                    IsSet = false;
                    Snackbar.Add("Area required", MudBlazor.Severity.Error);
                    return IsSet;
                }
                oMstBusiness.AddedBy = "Feast Planner";

                oMstUser.BusinessKey = oMstBusiness.UniqueKey;
                oMstUser.AddedBy = oMstBusiness.AddedBy;

                oUserAlert.BusinessKey = oMstBusiness.UniqueKey;
                oUserAlert.UserKey = oMstUser.UniqueKey;
                oUserAlert.Type = "Alert";
                oUserAlert.Title = "Signup";
                oUserAlert.AlertMessage = "Welcome to Feast Planner";
                oUserAlert.AddedBy = oMstBusiness.AddedBy;

                oBusinessLog.BusinessKey = oMstBusiness.UniqueKey;
                oBusinessLog.UserKey = oMstUser.UniqueKey;
                oBusinessLog.Type = "Alert";
                oBusinessLog.Title = "Signup";
                oBusinessLog.Description = "Your Profile and account created Welcome to Feast Planner";
                oBusinessLog.AddedBy = oMstBusiness.AddedBy;

                oModel.Business = oMstBusiness;
                oModel.User = oMstUser;
                oModel.UserAlert = oUserAlert;
                oModel.BusinessLog = oBusinessLog;

                IsSet = true;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
            }
            return IsSet;
        }

        private async Task OpenMessageDialog(DialogOptions options)
        {
            try
            {
                DialogFor = "Signup";
                var parameters = new DialogParameters();
                parameters.Add("DialogFor", DialogFor);
                var dialog = Dialog.Show<DialogBox>("Registration completed", parameters, options);
                var result = await dialog.Result;

                if (!result.Canceled)
                {
                    Navigation.NavigateTo("VerifyEmail");
                }
                else
                {
                    Navigation.NavigateTo("SignIn");
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
                await formUser.Validate();
                await formBusiness.Validate();
                if (SetModelValues() && successUser && successBusiness)
                {
                    res = await _authenticate.SignUp(oModel);
                    if (res.Id > 0)
                    {
                        Snackbar.Add("Registration Completed, Sign in to conitnue...", MudBlazor.Severity.Success);
                        await Task.Delay(3000);
                        Navigation.NavigateTo(Navigation.BaseUri + "/SignIn");
                        //await OpenMessageDialog(maxWidth);
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
                await Task.Delay(1);
                oMstUser.Name = "Hamza Qazi";
                oMstUser.Email = "iamhamzaqazi@yahoo.com";
                oMstUser.Password = "Super@12345";
                oMstUser.Contact = "";
                oMstUser.IsSuper = true;
                oMstBusiness.BusinessStartingPrice = 0;
                oMstBusiness.Logo = "";

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
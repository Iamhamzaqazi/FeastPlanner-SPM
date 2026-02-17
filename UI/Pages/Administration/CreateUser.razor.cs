using AppDBContext.VMModels;
using FluentValidation;
using System.Text.RegularExpressions;

namespace UI.Pages.Administration
{
    public partial class CreateUser
    {

        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        private bool loading = false;
        private bool EmailExist = false;
        private bool ContactExist = false;
        private bool IsSet = false;
        private bool PasswordVisible = false;
        private bool IsEdit = false;

        private string UserKey = "";
        private string BusinessKey = "";
        private string LoggedInUser = "";

        bool successUser;
        string[] errors = { };
        MudForm formUser;

        FluentValueValidator<string> cnValidator = new FluentValueValidator<string>(x => x
        .NotEmpty()
        .Length(1, 100)
        .Matches("^((\\+92)|(0092))-{0,1}\\d{3}-{0,1}\\d{7}$|^\\d{11}$|^\\d{4}-\\d{7}$")
        .WithMessage("Contact format incorrect"));

        MudTextField<string> pwField1;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        MstUser oModel = new MstUser();

        private IEnumerable<MstUser> oList = new List<MstUser>();

        private string searchString1 = "";
        private bool FilterFunc(MstUser element) => FilterFunc(element, searchString1);

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
        private bool FilterFunc(MstUser element, string searchString1)
        {
            if (string.IsNullOrWhiteSpace(searchString1))
                return true;
            if (element.Name.ToString().Contains(searchString1))
                return true;
            if (element.Email.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Contact.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.AddedBy.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.IsActive.ToString().Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        public async void EditRecord(int Id)
        {
            try
            {
                var res = oList.Where(x => x.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    oModel = res;
                    //oList = oList.Where(x => x.Id != Id);
                    IsEdit = true;
                    Snackbar.Add("You can only Active/In Active selected user.", MudBlazor.Severity.Info);
                    //pwField1.Value = oModel.Password;
                    //PasswordMatch(oModel.Password);
                    await formUser.Validate();
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
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
        private async Task<bool> CheckEmail()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(oModel.Email))
                {
                    var res = await _authenticate.CheckEmail(oModel.Email);
                    if (res.Id == 0)
                    {
                        EmailExist = false;
                        IsSet = true;
                    }
                    else
                    {
                        EmailExist = true;
                        Snackbar.Add("Email already in used for account.", MudBlazor.Severity.Error);
                        IsSet = false;
                    }
                }
                else
                {
                    IsSet = false;
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
                if (!string.IsNullOrWhiteSpace(oModel.Contact))
                {
                    var res = await _authenticate.CheckContact(oModel.Contact);
                    if (res.Id == 0)
                    {
                        ContactExist = false;
                        IsSet = true;
                    }
                    else
                    {
                        ContactExist = true;
                        Snackbar.Add("Contact already in used for account.", MudBlazor.Severity.Error);
                        IsSet = false;
                    }
                }
                else
                {
                    IsSet = false;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
            return EmailExist;
        }
        private async Task CallAPI()
        {
            try
            {
                await GetAllUser();
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        private async Task GetAllUser()
        {
            try
            {
                string Clause = $@" AND BusinessKey = '{BusinessKey}'";
                oList = await _mstUser.GetAllData(Clause);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        public async void SetModelValues()
        {
            try
            {
                if (oModel.Id == 0)
                {
                    await CheckEmail();
                    await CheckContact();
                    if (!EmailExist && !ContactExist && IsSet)
                    {
                        oModel.BusinessKey = BusinessKey;
                        oModel.AddedBy = LoggedInUser;
                        oModel.IsOtpenable = false;
                        oModel.IsEmailVerify = false;
                        oModel.IsContactVerify = false;
                        oModel.IsSuper = false;
                        IsSet = true;
                    }
                }
                else
                {
                    oModel.UpdatedBy = LoggedInUser;
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
                    if (string.IsNullOrWhiteSpace(oModel.Contact))
                    {
                        oModel.Contact = "";
                    }
                    res = await _mstUser.Crud(oModel);
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
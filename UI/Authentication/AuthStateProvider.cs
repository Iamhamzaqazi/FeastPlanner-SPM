using AppDBContext.General;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace UI.Authentication
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _oAuthenticationState;
        private readonly ActiveUsersService _activeUsersService;
        public AuthStateProvider(ILocalStorageService localStorage, ActiveUsersService activeUsersService)
        {
            _localStorage = localStorage;
            _oAuthenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            _activeUsersService = activeUsersService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string Token = await _localStorage.GetItemAsync<string>("UserAuthenticatedToken");
                if (!string.IsNullOrWhiteSpace(Token))
                {
                    var claims = JwtParser.ParseClaimsFromJwt(Token);
                    string LoggedInUser = claims.Where(x => x.Type == "UserID").Select(x => x.Value).FirstOrDefault();
                    if (!string.IsNullOrEmpty(LoggedInUser))
                    {
                        _activeUsersService.AddUser(LoggedInUser); // Add user to active list
                    }
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(Token), CookieAuthenticationDefaults.AuthenticationScheme)));
                }
                else
                {
                    return _oAuthenticationState;
                }
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex.Message);
                return null;
            }
        }
        public async Task NotifyUserAuthentication(string Token)
        {
            await Task.Delay(1);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(Token), CookieAuthenticationDefaults.AuthenticationScheme));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            var claims = JwtParser.ParseClaimsFromJwt(Token);
            string LoggedInUser = claims.Where(x => x.Type == "UserID").Select(x => x.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(LoggedInUser))
            {
                _activeUsersService.AddUser(LoggedInUser); // Add user to active list
            }
            NotifyAuthenticationStateChanged(authState);
        }
        public async Task NotifyUserLogout()
        {
            var user = await GetAuthenticationStateAsync();
            string LoggedInUser = user.User.Claims.Where(x => x.Type == "UserID").Select(x => x.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(LoggedInUser))
            {
                _activeUsersService.RemoveUser(LoggedInUser); // Remove user from active list
            }
            var authState = Task.FromResult(_oAuthenticationState);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
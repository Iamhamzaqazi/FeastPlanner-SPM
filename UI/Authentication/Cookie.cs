using AppDBContext.Interfaces.Cookies;
using Microsoft.JSInterop;
using System.Net;

namespace UI.Authentication
{
    public class Cookie : ICookie
    {
        private readonly IJSRuntime JSRuntime;
        string expires = "";

        public Cookie(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
            ExpireDays = 300;
        }
        public async Task SetCookie(string key, string value, int? days = null)
        {
            var curExp = (days != null) ? (days > 0 ? DateToUTC(days.Value) : "") : expires;
            await SetCookie($"{key}={value}; expires={curExp}; path=/");
        }
        public async Task<string> GetCookie(string key, string def = "")
        {
            var cValue = await GetCookie();
            if (string.IsNullOrEmpty(cValue)) return def;

            var vals = cValue.Split(';');
            foreach (var val in vals)
                if (!string.IsNullOrEmpty(val) && val.IndexOf('=') > 0)
                    if (val.Substring(0, val.IndexOf('=')).Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                        return val.Substring(val.IndexOf('=') + 1);
            return def;
        }
        private async Task SetCookie(string value)
        {
            await JSRuntime.InvokeVoidAsync("eval", $"document.cookie = \"{value}\"");
        }
        private async Task<string> GetCookie()
        {
            return await JSRuntime.InvokeAsync<string>("eval", $"document.cookie");
        }
        public async Task RemoveCookie(string key)
        {
            // Get the current UTC date and subtract 2 days
            var expirationDate = DateTime.UtcNow.AddDays(-2).ToString("R"); // "R" for RFC1123 format
                                                                            // Set the cookie with the same key and the calculated expiration date
            await SetCookie($"{key}=; expires={expirationDate}; path=/");
        }
        public int ExpireDays
        {
            set => expires = DateToUTC(value);
        }
        private static string DateToUTC(int days) => DateTime.Now.AddDays(days).ToUniversalTime().ToString("R");
    }
}
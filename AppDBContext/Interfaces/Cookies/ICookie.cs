using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.Cookies
{
    public interface ICookie
    {
        public Task SetCookie(string key, string value, int? days = null);
        public Task<string> GetCookie(string key, string def = "");
        public Task RemoveCookie(string key);
    }
}

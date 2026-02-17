using AppDBContext.Models;
using AppDBContext.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.Authentication
{
    public interface ITokenManager
    {
        string GenerateToken(MstUser oUser, List<VMMstUserAuthorization> userAuthorizations, MstBusiness oBusiness);
        ClaimsPrincipal VerifyToken(string Token);
    }
}
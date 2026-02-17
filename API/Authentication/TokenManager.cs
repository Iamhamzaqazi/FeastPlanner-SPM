using AppDBContext.General;
using AppDBContext.Interfaces.Authentication;
using AppDBContext.Models;
using AppDBContext.VMModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Authentication
{
    public class TokenManager : ITokenManager
    {
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        public TokenManager()
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateToken(MstUser oUser, List<VMMstUserAuthorization> userAuthorizations, MstBusiness oBusiness)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim("UserID", oUser.Id.ToString()),
                    new Claim("UserKey", oUser.UniqueKey.ToString()),
                    new Claim("Username", oUser.Name),
                    new Claim("Email", oUser.Email),
                    new Claim("Password", oUser.Password),
                    new Claim("Contact", oUser.Contact),
                    new Claim("IsEmailVerify", oUser.IsEmailVerify.ToString()),
                    new Claim("IsSuper", oUser.IsSuper.ToString()),
                    new Claim("IsOtpenable", oUser.IsOtpenable.ToString()),
                    new Claim("IsActive", oUser.IsActive.ToString()),
                    new Claim("BusinessKey", oBusiness.UniqueKey.ToString()),
                    new Claim("BusinessName", oBusiness.BusinessName.ToString()),
                    new Claim("BusinessLogo", oBusiness.Logo.ToString()),
                    new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddHours(1)).ToUnixTimeSeconds().ToString()),
                };
                foreach (var UserAuthroization in userAuthorizations)
                {
                    if (UserAuthroization.UserRights)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, UserAuthroization.CMenuName));
                    }
                }
                var Token = new JwtSecurityToken(
                    new JwtHeader(
                        new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ApplicationAPITokenMhq29062018++")), SecurityAlgorithms.HmacSha256Signature)),
                    new JwtPayload(claims)
                    );
                var jwtString = _jwtSecurityTokenHandler.WriteToken(Token);
                return jwtString;
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return null;
            }
        }
        public ClaimsPrincipal VerifyToken(string Token)
        {
            try
            {
                var claims = _jwtSecurityTokenHandler.ValidateToken(Token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ApplicationAPITokenMhq29062018++")),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validateToken);
                return claims;
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return null;
            }
        }
    }
}
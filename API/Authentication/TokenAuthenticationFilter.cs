using AppDBContext.General;
using AppDBContext.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Authentication
{
    public class TokenAuthenticationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext authorizationFilterContext)
        {
            try
            {
                string EndPointName = authorizationFilterContext.ActionDescriptor.RouteValues.Where(x => x.Key.ToLower() == "action").Select(x => x.Value).FirstOrDefault();
                if (EndPointName.ToLower() == "getallareadata" || EndPointName.ToLower() == "getallcitydata")
                {
                    return;
                }
                var tokenManager = (ITokenManager)authorizationFilterContext.HttpContext.RequestServices.GetService(typeof(ITokenManager));
                var result = true;
                if (!authorizationFilterContext.HttpContext.Request.Headers.ContainsKey("Authorization"))
                    result = false;

                string Token = string.Empty;
                if (result)
                {
                    Token = authorizationFilterContext.HttpContext.Request.Headers.First(x => x.Key == "Authorization").Value;
                    try
                    {
                        var claimPrinciple = tokenManager.VerifyToken(Token);
                        if (claimPrinciple == null)
                            result = false;
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        authorizationFilterContext.ModelState.AddModelError("UnAuthorized", "Token Validation failed!");
                    }
                }
                if (!result)
                {
                    authorizationFilterContext.Result = new UnauthorizedObjectResult(authorizationFilterContext.ModelState);
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return;
            }
        }
    }
}
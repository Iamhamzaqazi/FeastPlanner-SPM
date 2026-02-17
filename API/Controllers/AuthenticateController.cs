using AppDBContext.General;
using AppDBContext.Interfaces.Authentication;
using AppDBContext.Interfaces.User;
using AppDBContext.Models;
using AppDBContext.VMModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private IAuthenticate _authenticate;
        public AuthenticateController(ITokenManager tokenManager, IAuthenticate authenticate)
        {
            _authenticate = authenticate;
        }

        #region Accounts

        [Route("verifyUser")]
        [HttpGet]
        public async Task<IActionResult> VerifyUser(string Clause)
        {
            List<MstUser> oModel = new List<MstUser>();
            try
            {
                oModel = await _authenticate.VerifyUser(Clause);
                if (oModel == null)
                {
                    return BadRequest(oModel);
                }
                else
                {
                    return Ok(oModel);
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return BadRequest("Something went wrong.");
            }
        }

        [Route("login")]
        [HttpGet]
        public async Task<IActionResult> Login([FromBody] MstUser oModel)
        {
            MstUser model = new MstUser();
            model = await _authenticate.Login(oModel);
            if (model != null && !string.IsNullOrWhiteSpace(model.Token))
            {
                return Ok(model);
            }
            else
            {
                ModelState.AddModelError("UnAuthorized", "Token Validation failed!");
                return Unauthorized(ModelState);
            }
        }

        [Route("signUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _authenticate.SignUp(oModel);
                if (response == null || response.Id == 0)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return BadRequest("Something went wrong.");
            }
        }

        [Route("checkEmail")]
        [HttpGet]
        public async Task<IActionResult> CheckEmail([FromQuery] string Email)
        {
            APIResponseModel model = new APIResponseModel();
            try
            {
                model = await _authenticate.CheckEmail(Email);
                if (model == null)
                {
                    return BadRequest(model);
                }
                else
                {
                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return BadRequest("Something went wrong.");
            }
        }

        [Route("checkContact")]
        [HttpGet]
        public async Task<IActionResult> CheckContact([FromQuery] string Contact)
        {
            APIResponseModel model = new APIResponseModel();
            try
            {
                model = await _authenticate.CheckContact(Contact);
                if (model == null)
                {
                    return BadRequest(model);
                }
                else
                {
                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return BadRequest("Something went wrong.");
            }
        }

        [Route("getAllUserPasswordRequestByClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllUserPasswordDataByClause(string Clause)
        {
            List<UserPasswordRequest> oModel = new List<UserPasswordRequest>();
            try
            {
                oModel = await _authenticate.GetAllUserPasswordDataByClause(Clause);
                if (oModel == null)
                {
                    return BadRequest(oModel);
                }
                else
                {
                    return Ok(oModel);
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return BadRequest("Something went wrong.");
            }
        }

        [Route("crudUserPasswordRequest")]
        [HttpPost]
        public async Task<IActionResult> UserPasswordRequest([FromBody] UserPasswordRequest oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _authenticate.Crud(oModel);
                if (response == null)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return BadRequest("Something went wrong.");
            }
        }

        [Route("changePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] MstUser oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _authenticate.ChangePassword(oModel);
                if (response == null || response.Id == 0)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return BadRequest("Something went wrong.");
            }
        }

        #endregion
    }
}
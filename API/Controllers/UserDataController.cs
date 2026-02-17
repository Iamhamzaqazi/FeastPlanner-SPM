using API.Authentication;
using API.SignalHub;
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
    [TokenAuthenticationFilter]
    public class UserDataController : ControllerBase
    {
        private IMstUser _mstUser;
        private ICfgUser _cfgUser;
        private IMstUserMessage _mstUserMessage;
        private readonly IHubContext<NotificationHub> _hubContext;
        private IMstUserAuthorization _mstUserAuthorization;

        public UserDataController(IMstUser mstUser, IMstUserAuthorization mstUserAuthorization, ICfgUser cfgUser, IHubContext<NotificationHub> hubContext, IMstUserMessage mstUserMessage)
        {
            _mstUser = mstUser;
            _mstUserAuthorization = mstUserAuthorization;
            _cfgUser = cfgUser;
            _hubContext = hubContext;
            _mstUserMessage = mstUserMessage;
        }

        #region MST User        

        [Route("getAllUserClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllUser(string Clause)
        {
            List<MstUser> oModel = new List<MstUser>();
            try
            {
                oModel = await _mstUser.GetAllData(Clause);
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

        [Route("crudUser")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] MstUser oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _mstUser.Crud(oModel);
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

        #region MST User Authorization


        [Route("getAllUserAuthorizationClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllUserAuthorization(string Clause, bool IsSuper)
        {
            List<VMMstUserAuthorization> oModel = new List<VMMstUserAuthorization>();
            try
            {
                oModel = await _mstUserAuthorization.GetAllData(Clause, IsSuper);
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

        [Route("crudUserAuthorization")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] List<MstUserAuthorization> oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _mstUserAuthorization.Crud(oModel);
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

        #region Cfg User

        #region Contact Verification

        [Route("getAllContactVerificationDataByUser")]
        [HttpGet]
        public async Task<IActionResult> GetAllContactVerificationDataByClause(string Clause)
        {
            List<CfgContactVerification> oModel = new List<CfgContactVerification>();
            try
            {
                oModel = await _cfgUser.GetAllContactVerificationDataByClause(Clause);
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

        [Route("crudCfgContactVerification")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] CfgContactVerification oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _cfgUser.Crud(oModel);
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

        #region Email Verification

        [Route("getAllEmailVerificationDataByUser")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmailVerificationDataByClause(string Clause)
        {
            List<CfgEmailVerification> oModel = new List<CfgEmailVerification>();
            try
            {
                oModel = await _cfgUser.GetAllEmailVerificationDataByClause(Clause);
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

        [Route("crudCfgEmailVerification")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] CfgEmailVerification oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _cfgUser.Crud(oModel);
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

        #region TwoFa Verification

        [Route("getAllTwoFADataByClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllTwoFADataByClause(string Clause)
        {
            List<CfgTwoFa> oModel = new List<CfgTwoFa>();
            try
            {
                oModel = await _cfgUser.GetAllTwoFADataByClause(Clause);
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

        [Route("crudCfgTwoFA")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] CfgTwoFa oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _cfgUser.Crud(oModel);
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

        #region Email And Notifcation Preferences

        [Route("getAllVMPreferencesDataByClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllVMPreferencesDataByClause(string Clause)
        {
            List<VMUserEmailNotificationPreference> oModel = new List<VMUserEmailNotificationPreference>();
            try
            {
                oModel = await _cfgUser.GetAllVMPreferencesDataByClause(Clause);
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

        [Route("getAllPreferencesDataByClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllPreferencesDataByClause(string Clause)
        {
            List<CfgEmailNotificationPreference> oModel = new List<CfgEmailNotificationPreference>();
            try
            {
                oModel = await _cfgUser.GetAllPreferencesDataByClause(Clause);
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

        [Route("crudEmailNotificationPreference")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] CfgEmailNotificationPreference oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _cfgUser.Crud(oModel);
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

        [Route("crudListEmailNotificationPreference")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] List<CfgEmailNotificationPreference> oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _cfgUser.Crud(oModel);
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

        #region Theme Setting

        [Route("getCfgThemeModeDataByClause")]
        [HttpGet]
        public async Task<IActionResult> GetThemeSettingDataByClause(string Clause)
        {
            CfgThemeMode oModel = new CfgThemeMode();
            try
            {
                oModel = await _cfgUser.GetThemeSettingDataByClause(Clause);
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

        [Route("crudCfgThemeMode")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] CfgThemeMode oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _cfgUser.Crud(oModel);
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

        #endregion

        #region Mst User Message

        [Route("getAllUserMessageClause")]
        [HttpGet]
        public async Task<IActionResult> GetUserMessage(string Clause)
        {
            List<MstUserMessage> oModel = new List<MstUserMessage>();
            try
            {
                oModel = await _mstUserMessage.GetUserMessage(Clause);
                if (oModel == null)
                {
                    return BadRequest(oModel);
                }
                else
                {
                    await _hubContext.Clients.All.SendAsync("Message", oModel);
                    return Ok(oModel);
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return BadRequest("Something went wrong.");
            }
        }

        [Route("getAllUserMessageDetailClause")]
        [HttpGet]
        public async Task<IActionResult> GetUserMessageDetail(string Clause)
        {
            List<MstUserMessage> oModel = new List<MstUserMessage>();
            try
            {
                oModel = await _mstUserMessage.GetUserMessageDetail(Clause);
                if (oModel == null)
                {
                    return BadRequest(oModel);
                }
                else
                {
                    await _hubContext.Clients.All.SendAsync("MessageDetail", oModel);
                    return Ok(oModel);
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                return BadRequest("Something went wrong.");
            }
        }

        [Route("crudUserMessage")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MstUserMessage oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _mstUserMessage.Crud(oModel);
                if (response == null)
                {
                    return BadRequest(response);
                }
                else
                {
                    List<MstUserMessage> oList = new List<MstUserMessage>();
                    List<MstUserMessage> oListDetail = new List<MstUserMessage>();
                    string Clause = $@"FKFromUserID = {oModel.FkfromUserId} or FKToUserID = {oModel.FkfromUserId})";
                    oList = await _mstUserMessage.GetUserMessage(Clause);
                    oListDetail = await _mstUserMessage.GetUserMessageDetail(Clause);
                    if (oList.Count() > 0)
                    {
                        await _hubContext.Clients.All.SendAsync("Message", oList);
                        await _hubContext.Clients.All.SendAsync("MessageDetail", oListDetail);
                    }
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
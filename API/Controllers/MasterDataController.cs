using API.Authentication;
using AppDBContext.General;
using AppDBContext.Interfaces.Authentication;
using AppDBContext.Interfaces.MasterData;
using AppDBContext.Models;
using AppDBContext.VMModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TokenAuthenticationFilter]
    public class MasterDataController : ControllerBase
    {
        private IMasterData _masterData;

        public MasterDataController(IMasterData masterData)
        {
            _masterData = masterData;
        }

        #region Mst Menu

        [Route("getAllMenuClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllMenuData(string Clause)
        {
            List<MstMenu> oModel = new List<MstMenu>();
            try
            {
                oModel = await _masterData.GetAllMenuData(Clause);
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

        [Route("crudMenu")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] MstMenu oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _masterData.Crud(oModel);
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

        #region Mst Form

        [Route("getAllFormClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllFormData(string Clause)
        {
            List<MstForm> oModel = new List<MstForm>();
            try
            {
                oModel = await _masterData.GetAllFormData(Clause);
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

        [Route("crudForm")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] MstForm oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _masterData.Crud(oModel);
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

        #region Mst Area

        [Route("getAllAreaClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllAreaData(string Clause)
        {
            List<MstArea> oModel = new List<MstArea>();
            try
            {
                oModel = await _masterData.GetAllAreaData(Clause);
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

        [Route("crudArea")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] MstArea oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _masterData.Crud(oModel);
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

        #region Mst City

        [Route("getAllCityClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllCityData(string Clause)
        {
            List<MstCity> oModel = new List<MstCity>();
            try
            {
                oModel = await _masterData.GetAllCityData(Clause);
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

        [Route("crudCity")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] MstCity oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _masterData.Crud(oModel);
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

        #region Default Value

        [Route("getAllDefaultValueClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllDefaultValueData(string Clause)
        {
            List<CfgDefaultValue> oModel = new List<CfgDefaultValue>();
            try
            {
                oModel = await _masterData.GetAllDefaultValueData(Clause);
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

        #endregion

        #region Mst Facility

        [Route("getAllFacilityClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllFacilityData(string Clause)
        {
            List<MstFacility> oModel = new List<MstFacility>();
            try
            {
                oModel = await _masterData.GetAllFacilityData(Clause);
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

        [Route("crudFacility")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] MstFacility oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _masterData.Crud(oModel);
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

        #region MstReport

        [Route("getAllReportSetupClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllReportSetup(string Clause)
        {
            List<MstReport> oModel = new List<MstReport>();
            try
            {
                oModel = await _masterData.GetAllReport(Clause);
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

        [Route("crudReportSetup")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] MstReport oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _masterData.Crud(oModel);
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

        [Route("crudListReportSetup")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] List<MstReport> oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _masterData.Crud(oModel);
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

        [Route("DeleteReport")]
        [HttpGet]
        public async Task<IActionResult> DeleteReport(int ID, string UserCode, string ReportCode)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _masterData.DeleteReport(ID, UserCode, ReportCode);
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

        #endregion
    }
}

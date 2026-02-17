using API.Authentication;
using AppDBContext.General;
using AppDBContext.Interfaces.Authentication;
using AppDBContext.Interfaces.Business;
using AppDBContext.Models;
using AppDBContext.VMModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TokenAuthenticationFilter]
    public class BusinessDataController : ControllerBase
    {
        private IBusinessData _businessData;

        public BusinessDataController(IBusinessData businessData)
        {
            _businessData = businessData;
        }


        #region Mst Business

        [Route("getAllBusinessClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllBusinessData(string Clause)
        {
            List<MstBusiness> oModel = new List<MstBusiness>();
            try
            {
                oModel = await _businessData.GetAllBusinessData(Clause);
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

        [Route("crudBusiness")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] MstBusiness oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _businessData.Crud(oModel);
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

        #region Mst BusinessLog

        [Route("getAllBusinessLogClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllBusinessLogData(string Clause)
        {
            List<MstBusinessLog> oModel = new List<MstBusinessLog>();
            try
            {
                oModel = await _businessData.GetAllBusinessLogData(Clause);
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

        [Route("crudBusinessLog")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] MstBusinessLog oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _businessData.Crud(oModel);
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
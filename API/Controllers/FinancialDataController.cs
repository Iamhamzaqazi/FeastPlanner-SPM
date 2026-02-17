using API.Authentication;
using API.SignalHub;
using AppDBContext.General;
using AppDBContext.Interfaces.Authentication;
using AppDBContext.Interfaces.Financials;
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
    public class FinancialDataController : ControllerBase
    {
        private ITrnsFinancial _trnsFinancial;

        public FinancialDataController(ITrnsFinancial trnsFinancial)
        {
            _trnsFinancial = trnsFinancial;
        }

        #region Income        

        [Route("getAllIncomeClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllIncome(string Clause)
        {
            List<TrnsBusinessIncome> oModel = new List<TrnsBusinessIncome>();
            try
            {
                oModel = await _trnsFinancial.GetAllIncomeData(Clause);
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

        [Route("crudIncome")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] TrnsBusinessIncome oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _trnsFinancial.Crud(oModel);
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

        #region Expense        

        [Route("getAllExpenseClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllExpense(string Clause)
        {
            List<TrnsBusinessExpense> oModel = new List<TrnsBusinessExpense>();
            try
            {
                oModel = await _trnsFinancial.GetAllExpenseData(Clause);
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

        [Route("crudExpense")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] TrnsBusinessExpense oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _trnsFinancial.Crud(oModel);
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
using API.Authentication;
using AppDBContext.General;
using AppDBContext.Interfaces.Authentication;
using AppDBContext.Interfaces.Booking;
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
    public class BookingDataController : ControllerBase
    {
        private IBooking _bookingData;

        public BookingDataController(IBooking bookingData)
        {
            _bookingData = bookingData;
        }

        #region Trns BusinessBooking

        [Route("getAllBusinessBookingClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllBusinessBookingData(string Clause)
        {
            List<TrnsBusinessBooking> oModel = new List<TrnsBusinessBooking>();
            try
            {
                oModel = await _bookingData.GetAllBusinessBookingData(Clause);
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

        [Route("getAllBusinessBookingDetailClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllBusinessBookingDetailData(string Clause)
        {
            List<TrnsBusinessBookingDetail> oModel = new List<TrnsBusinessBookingDetail>();
            try
            {
                oModel = await _bookingData.GetAllBusinessBookingDetailData(Clause);
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

        [Route("getAllBusinessBookingPaymentClause")]
        [HttpGet]
        public async Task<IActionResult> GetAllBusinessBookingPaymentData(string Clause)
        {
            List<TrnsBusinessBookingPayment> oModel = new List<TrnsBusinessBookingPayment>();
            try
            {
                oModel = await _bookingData.GetAllBusinessBookingPaymentData(Clause);
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


        [Route("crudBusinessBooking")]
        [HttpPost]
        public async Task<IActionResult> Crud([FromBody] TrnsBusinessBooking oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                response = await _bookingData.Crud(oModel);
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
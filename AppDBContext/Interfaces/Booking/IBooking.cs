using AppDBContext.Models;
using AppDBContext.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.Booking
{
    public interface IBooking
    {
        
        #region Trns Booking

        Task<List<TrnsBusinessBooking>> GetAllBusinessBookingData(string Clause);
        Task<List<TrnsBusinessBookingDetail>> GetAllBusinessBookingDetailData(string Clause);
        Task<List<TrnsBusinessBookingPayment>> GetAllBusinessBookingPaymentData(string Clause);
        Task<APIResponseModel> Crud(TrnsBusinessBooking oTrnsBusinessBooking);

        #endregion
    }
}

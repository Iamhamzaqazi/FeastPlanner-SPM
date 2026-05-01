using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class TrnsBusinessBooking : BaseEntity
    {
        public TrnsBusinessBooking()
        {
            oBookingDetail = new List<TrnsBusinessBookingDetail>();
            oBookingPayment = new List<TrnsBusinessBookingPayment>();
        }
        public int DocEntry { get; set; }
        public string CustomerKey { get; set; }
        public string BusinessKey { get; set; }

        public List<TrnsBusinessBookingDetail> oBookingDetail { get; set; }
        public List<TrnsBusinessBookingPayment> oBookingPayment { get; set; }
    }
}
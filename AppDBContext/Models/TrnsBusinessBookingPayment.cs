using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class TrnsBusinessBookingPayment : BaseEntity
    {
        public string BookingKey { get; set; }
        public string PaymentTerms { get; set; }
        public decimal Amount { get; set; }
        public decimal Deposit { get; set; }
        public decimal Remaining { get; set; }
        public DateTime? PaymentDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
        public DateTime? SecondPaymentDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
    }
}
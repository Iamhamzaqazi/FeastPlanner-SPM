using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class TrnsBusinessBookingDetail : BaseEntity
    {
        public string CustomerKey { get; set; }
        public string BusinessKey { get; set; }
        public string BookingKey { get; set; }
        public string AssociateKey { get; set; }
        public string PackageKey { get; set; }
        public string EventType { get; set; }
        public DateTime? EventDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
        public int Gathering { get; set; }
        public int FoodItems { get; set; }
        public string TimeSlot { get; set; }
        public string ServingAs { get; set; }
        public string Facility { get; set; }
    }
}
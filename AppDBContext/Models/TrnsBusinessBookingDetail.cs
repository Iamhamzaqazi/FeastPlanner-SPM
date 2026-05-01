using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class TrnsBusinessBookingDetail : BaseEntity
    {
        public int DocEntry { get; set; }
        public string CustomerKey { get; set; }
        public string BusinessKey { get; set; }
        public string BookingKey { get; set; }
        public string AssociateKey { get; set; }
        public string AssociateName { get; set; }
        public string AssociateAvailabilityKey { get; set; }
        public string AssociateAvailability { get; set; }
        public string PackageKey { get; set; }
        public string Package { get; set; }
        public string CategoryType { get; set; }
        public string EventType { get; set; }
        public DateTime? EventDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
        public string TimeOfEvent { get; set; }
        public string ServingType { get; set; }
        public int Gathering { get; set; }
        public int FoodItems { get; set; }
        public string TimeSlot { get; set; }
        public string FacilityKey { get; set; }
        public string Facility { get; set; }
    }
}
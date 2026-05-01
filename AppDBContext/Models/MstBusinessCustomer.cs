using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstBusinessCustomer : BaseEntity
    {
        //public MstBusinessCustomer()
        //{
        //    TrnsBusinessBookings = new HashSet<TrnsBusinessBooking>();
        //}

        public string BusinessKey { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerNIC { get; set; }
        public string CustomerEmail { get; set; }
        public bool IsBookingCompleted { get; set; } = false;

        [NotMapped]
        public bool IsShow { get; set; }

        //public virtual ICollection<TrnsBusinessBooking> TrnsBusinessBookings { get; set; }
    }
}
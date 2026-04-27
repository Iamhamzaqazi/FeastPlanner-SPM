using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstBusinessCustomer : BaseEntity
    {
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerNIC { get; set; }
        public string CustomerEmail { get; set; }
        public bool IsBookingCompleted { get; set; } = false;
    }
}
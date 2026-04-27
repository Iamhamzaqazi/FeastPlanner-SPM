using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class TrnsBusinessBooking : BaseEntity
    {
        public string CustomerKey { get; set; }
        public string BusinessKey { get; set; }
    }
}
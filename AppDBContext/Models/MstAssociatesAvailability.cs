using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstAssociatesAvailability : BaseEntity
    {
        public string AssociateKey { get; set; }
        public string BusinessKey { get; set; }
        public string AvailableDays { get; set; }
        public string TimeSlots { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
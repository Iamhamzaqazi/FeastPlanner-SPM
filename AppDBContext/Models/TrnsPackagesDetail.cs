using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class TrnsPackagesDetail : BaseEntity
    {
        public int DocEntry { get; set; }
        public string BusinessKey { get; set; }
        public string PackageKey { get; set; }
        public string AssociateKey { get; set; }
        public string AssociateBusinessName { get; set; }
        public decimal MinGathering { get; set; }
        public decimal MaxGathering { get; set; }
        public string AssociateAvailabilityKey { get; set; }
        public string AssociatePackagesKey { get; set; }
        public string AvailableDays { get; set; }
        public string TimeSlots { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ItemName { get; set; }
        public decimal RatePerHead { get; set; }
        public decimal PerHead { get; set; }
        public decimal PhotosIncluded { get; set; }
        public decimal VideoLength { get; set; }
    }
}
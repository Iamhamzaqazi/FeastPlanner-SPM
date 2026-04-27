using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class TrnsPackages : BaseEntity
    {
        public TrnsPackages()
        {
            PackagesDetail = new List<TrnsPackagesDetail>();
        }
        public int DocEntry { get; set; }
        public string BusinessKey { get; set; }
        public string PackageType { get; set; }
        public string CategoryType { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public List<TrnsPackagesDetail> PackagesDetail { get; set; }
    }
}
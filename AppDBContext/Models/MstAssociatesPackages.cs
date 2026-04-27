using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstAssociatesPackages : BaseEntity
    {
        public int DocEntry { get; set; }
        public string AssociateKey { get; set; }
        public string BusinessKey { get; set; }
        public string ItemName { get; set; }
        public decimal ItemsCount { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal MinHead { get; set; }
        public string Remarks { get; set; }
    }
}
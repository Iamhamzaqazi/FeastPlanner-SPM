using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstFacility : BaseEntity
    {
        public string BusinessKey { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsComplimentary { get; set; } = true;
    }
}
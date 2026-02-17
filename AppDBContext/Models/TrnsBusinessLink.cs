using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class TrnsBusinessLink : BaseEntity
    {
        public int FkbusinessId { get; set; }
        public string Fblink { get; set; }
        public string Iglink { get; set; }
        public string Sclink { get; set; }
        public string GoogleLocationLink { get; set; }
    }
}

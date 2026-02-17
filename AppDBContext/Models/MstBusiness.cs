using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstBusiness : BaseEntity
    {
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string BusinessContact { get; set; }
        public decimal? BusinessStartingPrice { get; set; }
        public string Logo { get; set; }
    }
}
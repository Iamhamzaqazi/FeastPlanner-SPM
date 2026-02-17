using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstAssociates : BaseEntity
    {
        public string BusinessKey { get; set; }
        public string CategoryType { get; set; }
        public string ServiceType { get; set; }
        public string KitchenType { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string BusinessContact { get; set; }
        public decimal? BusinessStartingPrice { get; set; }
        public decimal? MinGathering { get; set; }
        public decimal? MaxGathering { get; set; }
        public string Logo { get; set; }
    }
}
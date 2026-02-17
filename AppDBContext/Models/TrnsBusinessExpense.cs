using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class TrnsBusinessExpense : BaseEntity
    {
        public string BusinessKey { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
        public string Comments { get; set; }
    }
}
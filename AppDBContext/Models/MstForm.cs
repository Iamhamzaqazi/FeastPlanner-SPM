using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstForm : BaseEntity
    {
        public string Description { get; set; }
        public bool IsApplicationForm { get; set; }
        public bool IsLayoutForm { get; set; }
    }
}
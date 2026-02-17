using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class CfgContactVerification : BaseEntity
    {
        public string UserKey { get; set; }
        public string UserContact { get; set; }
        public string Code { get; set; }
        public bool IsVerify { get; set; }
    }
}

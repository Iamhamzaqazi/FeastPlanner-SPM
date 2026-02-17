using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class CfgTwoFa : BaseEntity
    {
        public string UserKey { get; set; }
        public string Otptype { get; set; }
        public string SecretKey { get; set; }
        public string ManualCode { get; set; }
        public string Otpcode { get; set; }
        public DateTime? CodeExpiry { get; set; }
        public bool IsOtpenable { get; set; }
    }
}
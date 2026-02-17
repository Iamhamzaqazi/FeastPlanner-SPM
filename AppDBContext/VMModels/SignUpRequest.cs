using AppDBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.VMModels
{
    public class SignUpRequest
    {
        public MstBusiness Business { get; set; }
        public MstUser User { get; set; }
        public UserAlert UserAlert { get; set; }
        public MstBusinessLog BusinessLog { get; set; }
    }
}

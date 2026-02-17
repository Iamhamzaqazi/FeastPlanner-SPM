using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class UserPasswordRequest : BaseEntity
    {
        public string EncryptKey { get; set; }
        public string UserKey { get; set; }
        public string Email { get; set; }
    }
}
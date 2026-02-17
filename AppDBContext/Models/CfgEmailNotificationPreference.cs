using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class CfgEmailNotificationPreference : BaseEntity
    {
        public string UserKey { get; set; }
        public string PreferenceKey { get; set; }
        public bool UserRights { get; set; }
        public bool IsEmail { get; set; }
        public bool IsSms { get; set; }
        public bool IsAlert { get; set; }
    }
}

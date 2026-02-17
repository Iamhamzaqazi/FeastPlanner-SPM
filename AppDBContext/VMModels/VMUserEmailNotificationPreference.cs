using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.VMModels
{
    public class VMUserEmailNotificationPreference
    {
        public string CfgPreferenceUniqueKey { get; set; }
        public string MstPreferenceUniqueKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEmail { get; set; }
        public bool IsSms { get; set; }
        public bool IsAlert { get; set; }
        public bool UserRights { get; set; }
    }
}

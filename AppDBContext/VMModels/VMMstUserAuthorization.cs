using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.VMModels
{
    public class VMMstUserAuthorization
    {
        public int ID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string MenuLink { get; set; }
        public int PMenuID { get; set; }
        public string PMenuName { get; set; }
        public string Icon { get; set; }
        public string MenuParentName { get; set; }
        public int CMenuID { get; set; }
        public int CSortNum { get; set; }
        public string CMenuName { get; set; }
        public bool UserRights { get; set; }
    }
}

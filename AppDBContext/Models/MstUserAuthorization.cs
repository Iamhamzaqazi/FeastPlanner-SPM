using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstUserAuthorization : BaseEntity
    {
        public int FkuserId { get; set; }
        public int FkmenuId { get; set; }
        public int UserRights { get; set; }
        public int? MenuParent { get; set; }
        public string MenuName { get; set; }
        public string MenuLink { get; set; }
    }
}
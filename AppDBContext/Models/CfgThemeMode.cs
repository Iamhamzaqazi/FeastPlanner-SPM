using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class CfgThemeMode : BaseEntity
    {
        public int FKUserId { get; set; }
        public bool IsDarkMode { get; set; }
        public bool IsLightMode { get; set; }
        public bool IsSystemMode { get; set; }
    }
}
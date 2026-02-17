using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstMenu : BaseEntity
    {
        public int MenuParent { get; set; }
        public string MenuParentName { get; set; }
        public string MenuName { get; set; }
        public int SortNum { get; set; }
        public string MenuLink { get; set; }
        public string Icon { get; set; }
        public string ReportCode { get; set; }
        public bool? IsReport { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstReport : BaseEntity
    {
        public string ReportCode { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string FilePath { get; set; }
        public bool? IsLayout { get; set; }
        public bool? IsDelete { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Models
{
    public partial class MstArea : BaseEntity
    {
        public int FKCityID { get; set; }
        public string Name { get; set; }
    }
}

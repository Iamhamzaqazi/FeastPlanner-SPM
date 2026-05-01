using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.VMModels
{
    public class APIResponseModel
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? UniqueKey { get; set; }
    }
}

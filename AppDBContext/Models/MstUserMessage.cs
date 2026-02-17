using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppDBContext.Models
{
    public partial class MstUserMessage : BaseEntity
    {
        public int FkfromUserId { get; set; }
        public string FkfromUserName { get; set; }
        public int FktoUserId { get; set; }
        public string FktoUserName { get; set; }
        public string Message { get; set; }
        public bool MarkAsRead { get; set; }

        [NotMapped]
        public string TimeCalculate { get; set; }
    }
}
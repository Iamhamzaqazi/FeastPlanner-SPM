using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppDBContext.Models
{
    public partial class UserAlert : BaseEntity
    {
        public string BusinessKey { get; set; }
        public string UserKey { get; set; }        
        public string Title { get; set; }
        public string Type { get; set; }
        public string AlertMessage { get; set; }
        public bool MarkAsRead { get; set; }

        [NotMapped]
        [XmlIgnore]
        public string TimeCalculate { get; set; }
    }
}
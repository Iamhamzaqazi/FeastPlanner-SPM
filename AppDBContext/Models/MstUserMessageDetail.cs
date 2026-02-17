using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppDBContext.Models
{
    public partial class MstUserMessageDetail : BaseEntity
    {
        public string MessageKey { get; set; }
        public int FkmessageId { get; set; }
        public string Message { get; set; }
        public bool MarkAsRead { get; set; }
        public bool IsDelete { get; set; }
    }
}
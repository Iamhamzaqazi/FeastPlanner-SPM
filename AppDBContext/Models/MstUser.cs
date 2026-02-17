using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppDBContext.Models
{
    public partial class MstUser : BaseEntity
    {
        public string BusinessKey { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }
        public string ImagePath { get; set; }
        public string Gender { get; set; }        
        public bool IsEmailVerify { get; set; }
        public bool IsContactVerify { get; set; }
        public bool IsSuper { get; set; }
        public bool IsOtpenable { get; set; }

        [NotMapped]
        [XmlIgnore]
        public string Token { get; set; }
    }
}
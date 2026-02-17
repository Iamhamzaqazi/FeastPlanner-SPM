using AppDBContext.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppDBContext.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string UniqueKey { get; set; } = Guid.NewGuid().ToString();
        public bool IsActive { get; set; } = true;
        public string AddedBy { get; set; }
        public DateTime? AddedDt { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
        public string AddedWs { get; set; } = Environment.GetEnvironmentVariable("COMPUTERNAME");
        public string UpdatedBy { get; set; }
        public string UpdatedWs { get; set; } = Environment.GetEnvironmentVariable("COMPUTERNAME");
        public DateTime? UpdatedDt { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
        public string AppVersion { get; set; } = UIConfig.AppVersion;
        public int Uno { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.General
{
    public class APIConfig
    {
        public static string ConnectionString { get; set; }
        public static string RepositoryType { get; set; }

        public static string TitleConfig { get; set; }
        public static string EmailConfig { get; set; }
        public static string PasswordConfig { get; set; }
        public static string HostConfig { get; set; }
        public static int PortConfig { get; set; }
        public static bool IsSSlConfig { get; set; }
    }
}

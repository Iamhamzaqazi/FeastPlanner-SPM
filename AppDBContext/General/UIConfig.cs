using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.General
{
    public class UIConfig
    {
        public static string APIBaseURL { get; set; }
        public static string CRBaseURL { get; set; }
        public static string DialogFor { get; set; }
        public static string AppVersion { get; set; }
        public static string ReportPath { get; set; }
        public static string AttachmentPath { get; set; }
        public static bool Option1 { get; set; }
        public static string NotificationBaseURL { get; set; }
        public static string MessageBaseURL { get; set; }

        public static CultureInfo _en = CultureInfo.GetCultureInfo("en-US");

        public static int TotalAccountCompletion { get; set; }
        public static int TotalProfileCompletion { get; set; }

        public static string SMSUserNameConfig { get; set; }
        public static string SMSPasswordConfig { get; set; }
        public static string SMSURL { get; set; }
    }
}

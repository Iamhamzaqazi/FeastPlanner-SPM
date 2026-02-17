using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.General
{
    public class LogsAPI
    {
        public static void GenerateLogs(Exception LogMessage)
        {
            try
            {
                DateTime ExceptionDate = System.DateTime.Now;
                if (!File.Exists("Application_Logs_API.txt"))
                {
                    File.Create("Application_Logs_API.txt").Close();
                    using (StreamWriter sw = File.AppendText("Application_Logs_API.txt"))
                    {
                        sw.WriteLine(LogMessage + ": " + ExceptionDate, LogMessage.Message, LogMessage.InnerException == null ? "" : LogMessage.InnerException.ToString(), LogMessage.StackTrace);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText("Application_Logs_API.txt"))
                    {
                        sw.WriteLine(LogMessage + ": " + ExceptionDate, LogMessage.Message, LogMessage.InnerException == null ? "" : LogMessage.InnerException.ToString(), LogMessage.StackTrace);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public static void GenerateLogs(string LogMessage)
        {
            try
            {
                DateTime ExceptionDate = System.DateTime.Now;
                if (!File.Exists("Application_Logs_API.txt"))
                {
                    File.Create("Application_Logs_API.txt").Close();
                    using (StreamWriter sw = File.AppendText("Application_Logs_API.txt"))
                    {
                        sw.WriteLine($"{LogMessage}: {ExceptionDate}");
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText("Application_Logs_API.txt"))
                    {
                        sw.WriteLine($"{LogMessage}: {ExceptionDate}");
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
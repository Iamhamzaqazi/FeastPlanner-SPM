using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.General
{
    public class Email
    {
        #region Variables

        string TitleConfig = "";
        string EmailConfig = "";
        string PasswordConfig = "";
        string HostConfig = "";
        int PortConfig = 0;
        bool IsSSlConfig = true;

        #endregion

        #region Functions      

        public bool GetEmailSettings()
        {
            bool IsTrue = false;
            try
            {
                TitleConfig = APIConfig.TitleConfig;
                EmailConfig = APIConfig.EmailConfig;
                PasswordConfig = APIConfig.PasswordConfig;
                HostConfig = APIConfig.HostConfig;
                PortConfig = APIConfig.PortConfig;
                IsSSlConfig = APIConfig.IsSSlConfig;
                IsTrue = true;
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return IsTrue;
        }
        public bool SentEmail(string Subject, string Body, string ReceiverEmail)
        {
            // For localhost "smtp.gmail.com", Port 587
            bool IsSent = false;
            try
            {
                if (GetEmailSettings())
                {
                    SmtpClient _smtp = new SmtpClient();
                    _smtp.Host = HostConfig;
                    _smtp.Port = PortConfig;
                    _smtp.EnableSsl = IsSSlConfig;
                    NetworkCredential _network = new NetworkCredential(EmailConfig, PasswordConfig);
                    _smtp.Credentials = _network;
                    MailMessage _mailmsg = new MailMessage();
                    _mailmsg.IsBodyHtml = true;
                    _mailmsg.From = new MailAddress(EmailConfig, TitleConfig);
                    _mailmsg.To.Add(ReceiverEmail);
                    _mailmsg.Subject = Subject;

                    string MailMessage = Body;
                    _mailmsg.Body = MailMessage;

                    _smtp.Send(_mailmsg);
                    IsSent = true;
                }
                else
                {
                    IsSent = false;
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return IsSent;
        }
        public string GenerateOTP(string Email, out string OTPCode)
        {
            try
            {
                string keyString = "FeastPlannerUser29062018";
                var key = Encoding.UTF8.GetBytes(keyString);//16 bit or 32 bit key string

                using (var aesAlg = Aes.Create())
                {
                    using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(Email);
                            }

                            var iv = aesAlg.IV;

                            var decryptedContent = msEncrypt.ToArray();

                            var result = new byte[iv.Length + decryptedContent.Length];

                            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
                            string EncryptKey = Convert.ToBase64String(result);
                            OTPCode = EncryptKey.Substring(0, 4);
                            return OTPCode;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
                OTPCode = null;
                return null;
            }
        }

        #endregion
    }
}
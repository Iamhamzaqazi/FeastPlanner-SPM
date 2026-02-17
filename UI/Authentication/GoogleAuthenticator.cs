using AppDBContext.General;
using GoogleAuthenticatorService.Core;
using OtpNet;

namespace UI.Authentication
{
    public class GoogleAuthenticator
    {
        public string GenerateCode(out string QrCodeUrl, out string SecretKey)
        {
            try
            {
                TwoFactorAuthenticator Authenticator = new TwoFactorAuthenticator();
                var secretKey = KeyGeneration.GenerateRandomKey(20);
                var base32Secret = Base32Encoding.ToString(secretKey);

                var SetupResult = Authenticator.GenerateSetupCode("FeastPlanner", base32Secret, 250, 250);

                //QrCodeUrl = SetupResult.QrCodeSetupImageUrl;


                string ManualCode = SetupResult.ManualEntryKey;
                SecretKey = base32Secret;
                QrCodeUrl = $@"https://api.qrserver.com/v1/create-qr-code/?size=250x250&data=otpauth://totp/FeastPlanner?secret={SecretKey}";
                return ManualCode;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                QrCodeUrl = "";
                SecretKey = "";
                return "";
            }
        }
        public bool VerifyCode(string SecretKey, string ClientCode)
        {
            try
            {
                TwoFactorAuthenticator Authenticator = new TwoFactorAuthenticator();
                bool ValidateResult = Authenticator.ValidateTwoFactorPIN(SecretKey, ClientCode);

                return ValidateResult;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                return false;
            }
        }
    }
}
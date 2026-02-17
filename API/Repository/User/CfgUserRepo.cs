using API.DapperDAL;
using AppDBContext.General;
using AppDBContext.Interfaces.Dapper;
using AppDBContext.Interfaces.User;
using AppDBContext.Models;
using AppDBContext.VMModels;

namespace API.Repository.User
{
    public class CfgUserRepo : ICfgUser
    {
        private Email _Email;
        private IDapper _dapper;

        public CfgUserRepo(IDapper dapper)
        {
            _Email = new Email();
            _dapper = dapper;
        }
        public async Task<List<CfgContactVerification>> GetAllContactVerificationDataByClause(string Clause)
        {
            List<CfgContactVerification> oList = new List<CfgContactVerification>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<CfgContactVerification>(Clause);
                    oList = _dapper.SelectQueryList<CfgContactVerification>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(CfgContactVerification oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();

                        string xmlData = _dapper.ConvertToXml(oModel);
                        string Query = oQuery.FormatMergeQuery<CfgContactVerification>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<CfgContactVerification>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        if (oModel.Id > 0)
                        {
                            response.Message = "Update successfully";
                        }
                        else
                        {
                            response.Message = "Saved successfully";
                        }
                    }
                    else
                    {
                        if (oModel.Id > 0)
                        {
                            response.Message = "Failed to Update successfully";
                        }
                        else
                        {
                            response.Message = "Failed to Saved successfully";
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        public async Task<List<CfgEmailVerification>> GetAllEmailVerificationDataByClause(string Clause)
        {
            List<CfgEmailVerification> oList = new List<CfgEmailVerification>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<CfgEmailVerification>(Clause);
                    oList = _dapper.SelectQueryList<CfgEmailVerification>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(CfgEmailVerification oCfgEmailVerification)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    string Query = "";
                    DapperQuery oQuery = new DapperQuery();
                    if (!oCfgEmailVerification.IsVerify)
                    {
                        string Code = _Email.GenerateOTP(oCfgEmailVerification.UserEmail, out string OTPCode);

                        if (!string.IsNullOrWhiteSpace(Code))
                        {
                            string Clause = $@" and ""UserKey"" = '{oCfgEmailVerification.UserKey}' and ""UserEmail"" = '{oCfgEmailVerification.UserEmail}' AND IsActive = 'True'";
                            Query = oQuery.FormatSelectQuery<CfgEmailVerification>(Clause);
                            var oEmailVerification = _dapper.SelectQuery<CfgEmailVerification>(Query);

                            if (oEmailVerification?.Id > 0)
                            {
                                oEmailVerification.UpdatedBy = oCfgEmailVerification.UpdatedBy;
                                oEmailVerification.UpdatedDt = oCfgEmailVerification.UpdatedDt;
                                oEmailVerification.AppVersion = oCfgEmailVerification.AppVersion;
                                oCfgEmailVerification = oEmailVerification;
                            }
                            oCfgEmailVerification.Code = Code;

                            // Sending Email
                            string Subject = "Application - Email Verification";
                            string Detail = "Your One time Password (OTP) is given below;";
                            string Body = Detail + "<br/><b>" + Code + "</b>";

                            Email oEmail = new Email();
                            if (oEmail.SentEmail(Subject, Body, oCfgEmailVerification.UserEmail))
                            {

                            }
                            else
                            {
                                response.Id = 0;
                                response.Message = "Failed to initiate.";
                            }
                        }
                    }
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        string xmlData = _dapper.ConvertToXml(oCfgEmailVerification);
                        Query = oQuery.FormatMergeQuery<CfgEmailVerification>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<CfgEmailVerification>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        response.Message = "Initiated successfully";
                    }
                    else
                    {
                        response.Message = "Failed to initiate.";
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        public async Task<List<CfgTwoFa>> GetAllTwoFADataByClause(string Clause)
        {
            List<CfgTwoFa> oList = new List<CfgTwoFa>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<CfgTwoFa>(Clause);
                    oList = _dapper.SelectQueryList<CfgTwoFa>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(CfgTwoFa oCfgTwoFA)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    bool IsEmailSent = false;
                    string UserEmail = "";
                    if (oCfgTwoFA.Otptype == "Email")
                    {
                        DapperQuery oQuery = new DapperQuery();
                        string Clause = $@" and ""UniqueKey"" = '{oCfgTwoFA.UserKey}'";
                        string Query = oQuery.FormatSelectQuery<MstUser>(Clause);
                        var User = _dapper.SelectQuery<MstUser>(Query);
                        if (User != null && User.Id > 0)
                        {
                            UserEmail = User.Email;
                        }
                        if (!string.IsNullOrWhiteSpace(UserEmail))
                        {
                            string Code = _Email.GenerateOTP(UserEmail, out string OTPCode);
                            // Sending Email
                            string Subject = "Application - Two Factor Authentication";
                            string Detail = "Your Two Two Factor Authentication code is given below;";
                            string Body = Detail + "<br/><b>" + Code + "</b>";

                            Email oEmail = new Email();
                            if (oEmail.SentEmail(Subject, Body, UserEmail))
                            {
                                oCfgTwoFA.Otpcode = Code;
                                IsEmailSent = true;
                            }
                        }
                    }
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();

                        string xmlData = _dapper.ConvertToXml(oCfgTwoFA);
                        string Query = oQuery.FormatMergeQuery<CfgTwoFa>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<CfgTwoFa>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        response.Message = "Initiated successfully";
                    }
                    else
                    {
                        response.Message = "Failed to initiate";
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        public async Task<List<VMUserEmailNotificationPreference>> GetAllVMPreferencesDataByClause(string Clause)
        {
            List<VMUserEmailNotificationPreference> oList = new List<VMUserEmailNotificationPreference>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = $@"Select 
                                       a.UniqueKey as 'MstPreferenceUniqueKey',
                                       IsNull(t3.UniqueKey,'') as 'CfgPreferenceUniqueKey',
                                       a.Name,
                                       a.Description,
                                       case when IsNull(t3.IsEmail,0) = 0 then 0 else 1 end as IsEmail,
                                       case when IsNull(t3.IsSms,0) = 0 then 0 else 1 end as IsSms,
                                       case when IsNull(t3.IsAlert,0) = 0 then 0 else 1 end as IsAlert,
                                       case when isnull(t3.UserRights,0) = 0 then 0 else 1 end as UserRights
                                   from MstEmailNotificationPreference a
                                   Left Join cfgEmailNotificationPreferences t3 
                                       on t3.PreferenceKey = a.UniqueKey and UserKey = '{Clause}'
                                        where a.IsActive = 1;";
                    oList = _dapper.SelectQueryList<VMUserEmailNotificationPreference>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<List<CfgEmailNotificationPreference>> GetAllPreferencesDataByClause(string Clause)
        {
            List<CfgEmailNotificationPreference> oList = new List<CfgEmailNotificationPreference>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<CfgEmailNotificationPreference>(Clause);
                    oList = _dapper.SelectQueryList<CfgEmailNotificationPreference>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(CfgEmailNotificationPreference oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();

                        string xmlData = _dapper.ConvertToXml(oModel);
                        string Query = oQuery.FormatMergeQuery<CfgEmailNotificationPreference>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<CfgEmailNotificationPreference>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        if (oModel.Id > 0)
                        {
                            response.Message = "Update successfully";
                        }
                        else
                        {
                            response.Message = "Saved successfully";
                        }
                    }
                    else
                    {
                        if (oModel.Id > 0)
                        {
                            response.Message = "Failed to Update";
                        }
                        else
                        {
                            response.Message = "Failed to Saved";
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }
        public async Task<APIResponseModel> Crud(List<CfgEmailNotificationPreference> oList)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();

                        string xmlData = _dapper.ConvertToXml(oList);
                        string Query = oQuery.FormatMergeQuery<CfgEmailNotificationPreference>(true, false, false, false);
                        response.Id = _dapper.CRUDQuery<CfgEmailNotificationPreference>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        response.Message = "Saved successfully";
                    }
                    else
                    {
                        response.Message = "Failed to Saved";
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }

        public async Task<CfgThemeMode> GetThemeSettingDataByClause(string Clause)
        {
            CfgThemeMode oCfgThemeMode = new CfgThemeMode();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<CfgThemeMode>(Clause);
                    oCfgThemeMode = _dapper.SelectQuery<CfgThemeMode>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oCfgThemeMode;
        }
        public async Task<APIResponseModel> Crud(CfgThemeMode oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();

                        string xmlData = _dapper.ConvertToXml(oModel);
                        string Query = oQuery.FormatMergeQuery<CfgThemeMode>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<CfgThemeMode>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        if (oModel.Id > 0)
                        {
                            response.Message = "Update successfully";
                        }
                        else
                        {
                            response.Message = "Saved successfully";
                        }
                    }
                    else
                    {
                        if (oModel.Id > 0)
                        {
                            response.Message = "Failed to Update successfully";
                        }
                        else
                        {
                            response.Message = "Failed to Saved successfully";
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response;
        }
    }
}
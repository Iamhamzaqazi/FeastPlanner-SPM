using AppDBContext.Models;
using AppDBContext.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.User
{
    public interface ICfgUser
    {
        Task<List<CfgContactVerification>> GetAllContactVerificationDataByClause(string Clause);
        Task<APIResponseModel> Crud(CfgContactVerification oCfgContactVerification);

        Task<List<CfgEmailVerification>> GetAllEmailVerificationDataByClause(string Clause);
        Task<APIResponseModel> Crud(CfgEmailVerification oCfgEmailVerification);

        Task<List<CfgTwoFa>> GetAllTwoFADataByClause(string Clause);
        Task<APIResponseModel> Crud(CfgTwoFa oCfgTwoFa);

        Task<List<VMUserEmailNotificationPreference>> GetAllVMPreferencesDataByClause(string Clause);
        Task<List<CfgEmailNotificationPreference>> GetAllPreferencesDataByClause(string Clause);
        Task<APIResponseModel> Crud(CfgEmailNotificationPreference oModel);
        Task<APIResponseModel> Crud(List<CfgEmailNotificationPreference> oList);

        Task<CfgThemeMode> GetThemeSettingDataByClause(string Clause);
        Task<APIResponseModel> Crud(CfgThemeMode oCfgThemeMode);
    }
}
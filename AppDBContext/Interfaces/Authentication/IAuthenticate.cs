using AppDBContext.Models;
using AppDBContext.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.Authentication
{
    public interface IAuthenticate
    {
        Task<List<MstUser>> VerifyUser(string Clause);
        Task<APIResponseModel> SignUp(SignUpRequest oModel);
        Task<MstUser> Login(MstUser oMstUser);
        Task<APIResponseModel> CheckEmail(string Email);
        Task<APIResponseModel> CheckContact(string Contact);
        Task<List<UserPasswordRequest>> GetAllUserPasswordDataByClause(string Clause);
        Task<APIResponseModel> Crud(UserPasswordRequest oModel);
        Task<APIResponseModel> ChangePassword(MstUser oMstUser);
    }
}

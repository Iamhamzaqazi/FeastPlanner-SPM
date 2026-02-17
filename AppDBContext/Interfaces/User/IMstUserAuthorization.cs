using AppDBContext.Models;
using AppDBContext.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.User
{
    public interface IMstUserAuthorization
    {
        Task<List<VMMstUserAuthorization>> GetAllData(string Clause, bool IsSuper);
        Task<APIResponseModel> Crud(List<MstUserAuthorization> oMstUserAuthorization);
    }
}

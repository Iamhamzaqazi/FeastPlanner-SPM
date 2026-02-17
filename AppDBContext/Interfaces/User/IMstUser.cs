using AppDBContext.Models;
using AppDBContext.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.User
{
    public interface IMstUser
    {
        Task<List<MstUser>> GetAllData(string Clause);        
        Task<APIResponseModel> Crud(MstUser oMstUser);      
    }
}
using AppDBContext.Models;
using AppDBContext.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.User
{
    public interface IMstUserMessage
    {
        Task<List<MstUserMessage>> GetUserMessage(string Clause);
        Task<List<MstUserMessage>> GetUserMessageDetail(string Clause);
        Task<APIResponseModel> Crud(MstUserMessage oMstUserMessage);
    }
}
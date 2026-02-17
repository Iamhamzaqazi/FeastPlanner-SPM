using AppDBContext.Models;
using AppDBContext.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.Business
{
    public interface IBusinessData
    {
        #region Mst Business
        Task<List<MstBusiness>> GetAllBusinessData(string Clause);
        Task<APIResponseModel> Crud(MstBusiness oMstBusiness);

        #endregion

        #region Mst BusinessLog
        Task<List<MstBusinessLog>> GetAllBusinessLogData(string Clause);
        Task<APIResponseModel> Crud(MstBusinessLog oMstBusinessLog);

        #endregion
    }
}
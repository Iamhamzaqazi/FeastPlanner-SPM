using AppDBContext.Models;
using AppDBContext.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.Financials
{
    public interface ITrnsFinancial
    {
        Task<List<TrnsBusinessIncome>> GetAllIncomeData(string Clause);
        Task<APIResponseModel> Crud(TrnsBusinessIncome oModel);

        Task<List<TrnsBusinessExpense>> GetAllExpenseData(string Clause);
        Task<APIResponseModel> Crud(TrnsBusinessExpense oModel);
    }
}
using API.Authentication;
using API.DapperDAL;
using AppDBContext.General;
using AppDBContext.Interfaces.Authentication;
using AppDBContext.Interfaces.Dapper;
using AppDBContext.Interfaces.Financials;
using AppDBContext.Interfaces.User;
using AppDBContext.Models;
using AppDBContext.VMModels;

namespace API.Repository.Financials
{
    public class TrnsFinancialRepo : ITrnsFinancial
    {
        private IDapper _dapper;

        public TrnsFinancialRepo(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<TrnsBusinessIncome>> GetAllIncomeData(string Clause)
        {
            List<TrnsBusinessIncome> oList = new List<TrnsBusinessIncome>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<TrnsBusinessIncome>(Clause);
                    oList = _dapper.SelectQueryList<TrnsBusinessIncome>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(TrnsBusinessIncome oModel)
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
                        string Query = oQuery.FormatMergeQuery<TrnsBusinessIncome>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<TrnsBusinessIncome>(Query, xmlData);
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

        public async Task<List<TrnsBusinessExpense>> GetAllExpenseData(string Clause)
        {
            List<TrnsBusinessExpense> oList = new List<TrnsBusinessExpense>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<TrnsBusinessExpense>(Clause);
                    oList = _dapper.SelectQueryList<TrnsBusinessExpense>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(TrnsBusinessExpense oModel)
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
                        string Query = oQuery.FormatMergeQuery<TrnsBusinessExpense>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<TrnsBusinessExpense>(Query, xmlData);
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

    }
}
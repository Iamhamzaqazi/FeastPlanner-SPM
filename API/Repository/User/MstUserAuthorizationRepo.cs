using API.DapperDAL;
using AppDBContext.General;
using AppDBContext.Interfaces.Dapper;
using AppDBContext.Interfaces.User;
using AppDBContext.Models;
using AppDBContext.VMModels;

namespace API.Repository.User
{
    public class MstUserAuthorizationRepo : IMstUserAuthorization
    {
        private IDapper _dapper;

        public MstUserAuthorizationRepo(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<VMMstUserAuthorization>> GetAllData(string Clause, bool IsSuper)
        {
            List<VMMstUserAuthorization> oList = new List<VMMstUserAuthorization>();
            try
            {
                await Task.Run(() =>
                {
                    string qry = "";
                    if (IsSuper)
                    {
                        qry = $@"SELECT COALESCE(T3.""ID"",0) AS ""ID"",T3.""ADDEDBY"",T3.""ADDEDDT"",A.""MENULINK"",B.""ID"" AS ""PMENUID"",B.Icon as ""ICON"",
                                                     B.""MENUNAME"" AS ""PMENUNAME"",A.""ID"" AS ""CMENUID"",A.""MENUNAME"" AS ""CMENUNAME"",A.""MENUPARENTNAME"", A.""SORTNUM"" AS ""CSORTNUM"",
                                                     CASE WHEN COALESCE(""USERRIGHTS"",2) = 1 THEN 'TRUE' ELSE 'TRUE' END AS ""USERRIGHTS""
                                                     FROM ""MSTMENU"" A 
                                                     INNER JOIN ""MSTMENU"" B ON A.""MENUPARENT""=B.""ID""
                                                     LEFT JOIN ""MSTUSERAUTHORIZATION"" T3 ON T3.""FKMENUID"" =A.""ID""
                                                     WHERE A.""ISACTIVE"" = 'TRUE'
                                                     ORDER BY A.""ID"", A.""MENUNAME""";
                    }
                    else
                    {
                        qry = $@"SELECT COALESCE(T3.""ID"",0) AS ""ID"",T3.""ADDEDBY"",T3.""ADDEDDT"",A.""MENULINK"",B.""ID"" AS ""PMENUID"",B.Icon as ""ICON"",
                                                     B.""MENUNAME"" AS ""PMENUNAME"",A.""ID"" AS ""CMENUID"",A.""MENUNAME"" AS ""CMENUNAME"",A.""MENUPARENTNAME"", A.""SORTNUM"" AS ""CSORTNUM"",
                                                     CASE WHEN COALESCE(""USERRIGHTS"",1) = 1 THEN 'FALSE' ELSE 'TRUE' END AS ""USERRIGHTS""
                                                     FROM ""MSTMENU"" A 
                                                     INNER JOIN ""MSTMENU"" B ON A.""MENUPARENT""=B.""ID""
                                                     LEFT JOIN ""MSTUSERAUTHORIZATION"" T3 ON T3.""FKMENUID"" =A.""ID"" {Clause}
                                                     WHERE A.""ISACTIVE"" = 'TRUE'
                                                     ORDER BY A.""ID"", A.""MENUNAME""";
                    }
                    oList = _dapper.SelectQueryList<VMMstUserAuthorization>(qry);

                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(List<MstUserAuthorization> oMstUserAuthorization)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();
                        string xmlData = _dapper.ConvertToXml(oMstUserAuthorization);
                        string Query = oQuery.FormatMergeQuery<MstUserAuthorization>(true, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstUserAuthorization>(Query, xmlData);
                    }
                    if (response.Id > 0)
                    {
                        response.Id = 1;
                        response.Message = "Saved successfully";
                    }
                    else
                    {
                        response.Id = 0;
                        response.Message = "Failed to Save successfully";
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
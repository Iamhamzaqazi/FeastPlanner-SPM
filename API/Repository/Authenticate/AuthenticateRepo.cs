using API.DapperDAL;
using AppDBContext.General;
using AppDBContext.Interfaces.Authentication;
using AppDBContext.Interfaces.Dapper;
using AppDBContext.Interfaces.User;
using AppDBContext.Models;
using AppDBContext.VMModels;

namespace API.Repository.Authenticate
{
    public class AuthenticateRepo : IAuthenticate
    {
        private readonly ITokenManager _tokenManager;
        private readonly IMstUserAuthorization _userAuth;
        private Email _Email;
        private IDapper _dapper;

        public AuthenticateRepo(ITokenManager tokenManager, IMstUserAuthorization userAuth, IDapper dapper)
        {
            _tokenManager = tokenManager;
            _userAuth = userAuth;
            _Email = new Email();
            _dapper = dapper;
        }

        public async Task<List<MstUser>> VerifyUser(string Clause)
        {
            List<MstUser> oList = new List<MstUser>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<MstUser>(Clause);
                    oList = _dapper.SelectQueryList<MstUser>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> SignUp(SignUpRequest oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();

                        string xmlData1 = _dapper.ConvertToXml(oModel.Business);
                        string xmlData2 = _dapper.ConvertToXml(oModel.User);
                        string xmlData3 = _dapper.ConvertToXml(oModel.UserAlert);
                        string xmlData4 = _dapper.ConvertToXml(oModel.BusinessLog);

                        string Query1 = oQuery.FormatMergeQuery<MstBusiness>(false, false, false, false);
                        string Query2 = oQuery.FormatMergeQuery<MstUser>(false, false, false, false);
                        string Query3 = oQuery.FormatMergeQuery<UserAlert>(false, false, false, false);
                        string Query4 = oQuery.FormatMergeQuery<MstBusinessLog>(false, false, false, false);

                        response.Id = _dapper.CRUDQuery<MstBusiness, MstUser, UserAlert, MstBusinessLog>(Query1, Query2, Query3, Query4,
                                                                                                         xmlData1, xmlData2, xmlData3, xmlData4);
                    }
                    if (response.Id > 0)
                    {
                        if (oModel.Business.Id > 0)
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
                        if (oModel.Business.Id > 0)
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
        public async Task<MstUser> Login(MstUser oMstUser)
        {
            MstUser oUser = new MstUser();
            MstBusiness oBusiness = new MstBusiness();
            List<VMMstUserAuthorization> AuthMenus = new List<VMMstUserAuthorization>();
            try
            {
                await Task.Run(async () =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Clause = $@" and ""EMAIL"" = @EMAIL and ""PASSWORD"" = @PASSWORD and ""ISACTIVE"" = @ISACTIVE";
                    var parameters = new
                    {
                        ISACTIVE = true,
                        EMAIL = oMstUser.Email.Trim(),
                        PASSWORD = oMstUser.Password.Trim()
                    };
                    string Query = oQuery.FormatSelectQuery<MstUser>(Clause);
                    oUser = _dapper.SelectQuery<MstUser>(Query, parameters);
                    if (oUser != null && oUser.Id > 0)
                    {
                        Clause = $@"AND FKUSERID = {oUser.Id}";
                        AuthMenus = await _userAuth.GetAllData(Clause, oUser.IsSuper);
                        oUser.Token = string.Empty;

                        #region Get Business

                        Clause = $@"AND UNIQUEKEY = '{oUser.BusinessKey}'";
                        Query = oQuery.FormatSelectQuery<MstBusiness>(Clause);
                        oBusiness = _dapper.SelectQuery<MstBusiness>(Query, parameters);

                        #endregion

                        oUser.Token = _tokenManager.GenerateToken(oUser, AuthMenus, oBusiness);
                    }
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oUser;
        }
        public async Task<APIResponseModel> CheckEmail(string Email)
        {
            APIResponseModel oResponse = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Clause = $@" and ""EMAIL"" = '{Email}'";
                    string Query = oQuery.FormatSelectQuery<MstUser>(Clause);
                    var oUser = _dapper.SelectQuery<MstUser>(Query);
                    if (oUser != null && oUser.Id > 0)
                    {
                        oResponse.Id = oUser.Id;
                    }
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oResponse;
        }
        public async Task<APIResponseModel> CheckContact(string Contact)
        {
            APIResponseModel oResponse = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Clause = $@" and ""CONTACT"" = '{Contact}'";
                    string Query = oQuery.FormatSelectQuery<MstUser>(Clause);
                    var oUser = _dapper.SelectQuery<MstUser>(Query);
                    if (oUser != null && oUser.Id > 0)
                    {
                        oResponse.Id = oUser.Id;
                    }
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oResponse;
        }
        public async Task<List<UserPasswordRequest>> GetAllUserPasswordDataByClause(string Clause)
        {
            List<UserPasswordRequest> oList = new List<UserPasswordRequest>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<UserPasswordRequest>(Clause);
                    oList = _dapper.SelectQueryList<UserPasswordRequest>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(UserPasswordRequest oModel)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                await Task.Run(() =>
                {
                    if (APIConfig.RepositoryType.ToLower() == "sql")
                    {
                        DapperQuery oQuery = new DapperQuery();

                        string Code = _Email.GenerateOTP(oModel.Email, out string OTPCode);

                        if (!string.IsNullOrWhiteSpace(Code))
                        {
                            // Sending Email
                            string Subject = "Feast Planner - Reset Password Request";
                            string Detail = "Your reset password request code is given below;";
                            string Body = Detail + "<br/><b>" + Code + "</b>";

                            Email oEmail = new Email();
                            if (oEmail.SentEmail(Subject, Body, oModel.Email))
                            {

                                response.Id = 1;
                                response.Message = "Email initiated Successfully.";
                                oModel.EncryptKey = Code;
                            }
                            else
                            {
                                response.Id = 0;
                                response.Message = "Failed to initiate email.";
                            }
                        }

                        if (response.Id == 1)
                        {
                            string xmlData = _dapper.ConvertToXml(oModel);
                            string Query = oQuery.FormatMergeQuery<UserPasswordRequest>(false, false, false, false);
                            response.Id = _dapper.CRUDQuery<UserPasswordRequest>(Query, xmlData);
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
        public async Task<APIResponseModel> ChangePassword(MstUser oModel)
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
                        string Query = oQuery.FormatMergeQuery<MstUser>(false, false, false, false);
                        response.Id = _dapper.CRUDQuery<MstUser>(Query, xmlData);
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
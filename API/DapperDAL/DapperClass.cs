using AppDBContext.General;
using AppDBContext.Interfaces.Dapper;
using AppDBContext.VMModels;
using Dapper;
using Dapper.Transaction;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Serialization;

namespace API.DapperDAL
{
    public class DapperClass : IDapper
    {
        public T SelectQuery<T>(string qry, object parameters = null)
        {
            T oModel = (T)Activator.CreateInstance(typeof(T));
            try
            {
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        oModel = connection.Query<T>(qry, parameters).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oModel;
        }
        public List<T> SelectQueryList<T>(string qry)
        {
            List<T> oList = new List<T>();
            try
            {
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        oList = connection.Query<T>(qry).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public List<dynamic> SelectQueryListDynamic<dynamic>(string qry)
        {
            List<dynamic> oList = new List<dynamic>();
            try
            {
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        oList = connection.Query<dynamic>(qry).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public int CRUDQuery(string qry)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            response.Id = transaction.Execute(qry);
                            transaction.Commit();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response.Id;
        }
        public int CRUDQuery<T>(string qry, string xmlData)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                Type typeHeader = typeof(T);
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            var parameters = new DynamicParameters();
                            parameters.Add($"@{typeHeader.Name}", xmlData, DbType.Xml, ParameterDirection.Input);
                            response.Id = transaction.Execute(qry, parameters, commandTimeout: 3600, commandType: CommandType.Text);
                            transaction.Commit();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response.Id;
        }
        public int CRUDQuery<T1, T2, T3, T4>(string qry1, string qry2, string qry3, string qry4, string xmlData1, string xmlData2, string xmlData3, string xmlData4)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                Type type1 = typeof(T1);
                Type type2 = typeof(T2);
                Type type3 = typeof(T3);
                Type type4 = typeof(T4);
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            string finalQuery = $@"{qry1};
                                                   {qry2};
                                                   {qry3};
                                                   {qry4};";

                            var parameters = new DynamicParameters();
                            parameters.Add($"@{type1.Name}", xmlData1, DbType.Xml, ParameterDirection.Input);
                            parameters.Add($"@{type2.Name}", xmlData2, DbType.Xml, ParameterDirection.Input);
                            parameters.Add($"@{type3.Name}", xmlData3, DbType.Xml, ParameterDirection.Input);
                            parameters.Add($"@{type4.Name}", xmlData4, DbType.Xml, ParameterDirection.Input);
                            response.Id = transaction.Execute(finalQuery, parameters, commandTimeout: 3600, commandType: CommandType.Text);
                            transaction.Commit();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response.Id;
        }
        public int CRUDQuery<TH, TD>(string qry, string xmlDataHeader, string xmlDataDetail)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                Type typeHeader = typeof(TH);
                Type typeDetail = typeof(TD);
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            var parameters = new DynamicParameters();
                            parameters.Add($"@{typeHeader.Name}", xmlDataHeader, DbType.Xml, ParameterDirection.Input);
                            parameters.Add($"@{typeDetail.Name}", xmlDataDetail, DbType.Xml, ParameterDirection.Input);
                            response.Id = transaction.Execute(qry, parameters, commandTimeout: 3600, commandType: CommandType.Text);
                            transaction.Commit();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response.Id;
        }
        public int CRUDQuery<TH, TD1, TD2>(string qry, string xmlDataHeader, string xmlDataDetail1, string xmlDataDetail2)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                Type typeHeader = typeof(TH);
                Type typeDetail1 = typeof(TD1);
                Type typeDetail2 = typeof(TD2);
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            var parameters = new DynamicParameters();
                            parameters.Add($"@{typeHeader.Name}", xmlDataHeader, DbType.Xml, ParameterDirection.Input);
                            parameters.Add($"@{typeDetail1.Name}", xmlDataDetail1, DbType.Xml, ParameterDirection.Input);
                            parameters.Add($"@{typeDetail2.Name}", xmlDataDetail2, DbType.Xml, ParameterDirection.Input);
                            response.Id = transaction.Execute(qry, parameters, commandTimeout: 3600, commandType: CommandType.Text);
                            transaction.Commit();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response.Id;
        }
        public int CRUDQuery<TH, TD1, TD2, TD3>(string qry, string xmlDataHeader, string xmlDataDetail1, string xmlDataDetail2, string xmlDataDetail3)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                Type typeHeader = typeof(TH);
                Type typeDetail1 = typeof(TD1);
                Type typeDetail2 = typeof(TD2);
                Type typeDetail3 = typeof(TD3);
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            var parameters = new DynamicParameters();
                            parameters.Add($"@{typeHeader.Name}", xmlDataHeader, DbType.Xml, ParameterDirection.Input);
                            parameters.Add($"@{typeDetail1.Name}", xmlDataDetail1, DbType.Xml, ParameterDirection.Input);
                            parameters.Add($"@{typeDetail2.Name}", xmlDataDetail2, DbType.Xml, ParameterDirection.Input);
                            parameters.Add($"@{typeDetail3.Name}", xmlDataDetail3, DbType.Xml, ParameterDirection.Input);
                            response.Id = transaction.Execute(qry, parameters, commandTimeout: 3600, commandType: CommandType.Text);
                            transaction.Commit();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response.Id;
        }
        public int CRUDQuery<T>(string qry, T oModel)
        {
            Type typeHeader = typeof(T);
            APIResponseModel response = new APIResponseModel();
            try
            {
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            response.Id = transaction.Execute(qry);
                            transaction.Commit();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response.Id;
        }
        public int CRUDQuery<T>(string qry, List<T> oList)
        {
            Type typeHeader = typeof(T);
            APIResponseModel response = new APIResponseModel();
            try
            {
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            response.Id = transaction.Execute(qry);
                            transaction.Commit();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response.Id;
        }
        public int CRUDQuery<TH, TD>(string QueryHeader, string QueryDetail, TH oHeader, List<TD> oDetail)
        {
            APIResponseModel response = new APIResponseModel();
            try
            {
                if (APIConfig.RepositoryType.ToLower() == "sql")
                {
                    using (IDbConnection connection = new SqlConnection(APIConfig.ConnectionString))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            response.Id = transaction.Execute(QueryHeader);
                            transaction.Commit();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Id = 0;
                response.Message = ex.Message;
                LogsAPI.GenerateLogs(ex);
            }
            return response.Id;
        }
        public string ConvertToXml<T>(T oModel)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, oModel);
                return writer.ToString();
            }
        }
        public string ConvertToXml<T>(List<T> oList)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, oList);
                return writer.ToString();
            }
        }
        public string ConvertToXml<T>(IEnumerable<T> oList)  // Accepts IEnumerable<T>
        {
            if (oList == null || !oList.Any())
                return string.Empty;

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>)); // Uses List<T>
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, oList.ToList()); // Convert IEnumerable<T> to List<T>
                return writer.ToString();
            }
        }
    }
}
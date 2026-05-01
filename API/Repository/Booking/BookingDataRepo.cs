using API.DapperDAL;
using AppDBContext.General;
using AppDBContext.Interfaces.Booking;
using AppDBContext.Interfaces.Dapper;
using AppDBContext.Models;
using AppDBContext.VMModels;

namespace API.Repository.Booking
{
    public class BookingDataRepo : IBooking
    {
        private IDapper _dapper;

        public BookingDataRepo(IDapper dapper)
        {
            _dapper = dapper;
        }

        #region Trns BusinessBooking
        public async Task<List<TrnsBusinessBooking>> GetAllBusinessBookingData(string Clause)
        {
            List<TrnsBusinessBooking> oList = new List<TrnsBusinessBooking>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<TrnsBusinessBooking>(Clause);
                    string QueryDetail1 = oQuery.FormatSelectQuery<TrnsBusinessBookingDetail>(Clause);
                    string QueryDetail2 = oQuery.FormatSelectQuery<TrnsBusinessBookingPayment>(Clause);

                    var groupHeader = _dapper.SelectQueryList<TrnsBusinessBooking>(Query)
                                .GroupJoin(
                                    _dapper.SelectQueryList<TrnsBusinessBookingDetail>(QueryDetail1),
                                    header => header.DocEntry,
                                    line1 => line1.DocEntry,
                                    (header, detailGroup) =>
                                    {
                                        if (detailGroup != null && detailGroup.Any())
                                            header.oBookingDetail = detailGroup.ToList();
                                        return header;
                                    }
                                ).GroupJoin(
                                    _dapper.SelectQueryList<TrnsBusinessBookingPayment>(QueryDetail2),
                                    header => header.DocEntry,
                                    line1 => line1.DocEntry,
                                    (header, detailGroup) =>
                                    {
                                        if (detailGroup != null && detailGroup.Any())
                                            header.oBookingPayment = detailGroup.ToList();
                                        return header;
                                    }
                                ).ToList();
                    oList = groupHeader.ToList();

                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<List<TrnsBusinessBookingDetail>> GetAllBusinessBookingDetailData(string Clause)
        {
            List<TrnsBusinessBookingDetail> oList = new List<TrnsBusinessBookingDetail>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<TrnsBusinessBookingDetail>(Clause);
                    oList = _dapper.SelectQueryList<TrnsBusinessBookingDetail>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<List<TrnsBusinessBookingPayment>> GetAllBusinessBookingPaymentData(string Clause)
        {
            List<TrnsBusinessBookingPayment> oList = new List<TrnsBusinessBookingPayment>();
            try
            {
                await Task.Run(() =>
                {
                    DapperQuery oQuery = new DapperQuery();
                    string Query = oQuery.FormatSelectQuery<TrnsBusinessBookingPayment>(Clause);
                    oList = _dapper.SelectQueryList<TrnsBusinessBookingPayment>(Query);
                });
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return oList;
        }
        public async Task<APIResponseModel> Crud(TrnsBusinessBooking oModel)
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
                        string xmlDataDetail1 = _dapper.ConvertToXml(oModel.oBookingDetail.ToList());
                        string xmlDataDetail2 = _dapper.ConvertToXml(oModel.oBookingPayment.ToList());
                        oModel.oBookingDetail.ForEach(x => x.BookingKey = oModel.UniqueKey);
                        oModel.oBookingPayment.ForEach(x => x.BookingKey = oModel.UniqueKey);
                        string Query = oQuery.FormatMergeQuery<TrnsBusinessBooking>(false, false, true, false);
                        Query += oQuery.FormatMergeQuery<TrnsBusinessBookingDetail>(true, true, true, false);
                        Query += oQuery.FormatMergeQuery<TrnsBusinessBookingPayment>(true, true, true, false);
                        response.Id = _dapper.CRUDQuery<TrnsBusinessBooking, TrnsBusinessBookingDetail, TrnsBusinessBookingPayment>(Query, xmlData, xmlDataDetail1, xmlDataDetail2);
                    }
                    if (response.Id > 0)
                    {
                        response.Id = 1;
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
                        response.Id = 0;
                        if (oModel.Id > 0)
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

        #endregion
    }
}
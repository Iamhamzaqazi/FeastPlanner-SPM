using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDBContext.Interfaces.Dapper
{
    public interface IDapper
    {
        T SelectQuery<T>(string qry, object parameters = null);
        List<T> SelectQueryList<T>(string qry);        
        List<dynamic> SelectQueryListDynamic<dynamic>(string qry);
        int CRUDQuery(string qry);
        int CRUDQuery<T>(string qry, string xmlData);
        int CRUDQuery<T1, T2,T3,T4>(string qry1, string qry2, string qry3, string qry4, string xmlData1, string xmlData2, string xmlData3, string xmlData4);
        int CRUDQuery<TH, TD>(string qry, string xmlDataHeader, string xmlDataDetail);
        int CRUDQuery<TH, TD1, TD2>(string qry, string xmlDataHeader, string xmlDataDetail1, string xmlDataDetail2);
        int CRUDQuery<TH, TD1, TD2, TD3>(string qry, string xmlDataHeader, string xmlDataDetail1, string xmlDataDetail2, string xmlDataDetail3);        
        int CRUDQuery<T>(string qry, T oModel);
        int CRUDQuery<T>(string qry, List<T> oList);
        int CRUDQuery<TH, TD>(string QueryHeader, string QueryDetail, TH oHeader, List<TD> oDetail);        
        string ConvertToXml<T>(T oModel);
        string ConvertToXml<T>(List<T> oList);
        string ConvertToXml<T>(IEnumerable<T> oList);
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;
using Newtonsoft.Json;
using PSDMAG.Models;
using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Data;

namespace PSDMAG.Services
{
   
    public interface IMemberService
    {
        string Query(string Conn);
        string Insert(string Conn, MemberActionRequest InsData);
    }
    public class MemberService : IMemberService
    {
        public string Query(string Conn)
        {
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = Conn;
            // ConnectStringBulder.DataSource = _appSettings.LocalDB;
            DataTable tbResult = new DataTable();
            using (var db =  new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "select * from Member ";
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                var query = sqlcmd.ExecuteReader();
                //var Result = new List<Member>();
                if (query.HasRows) {
                    tbResult.Load(query);
                }
               
            }
            var result = new
            {
                Total = 0,
                Data = tbResult
            };

            Newtonsoft.Json.Converters.IsoDateTimeConverter idtc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(result, Formatting.None, idtc);
        }
        public string Insert(string Conn, MemberActionRequest InsData)
        {
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = Conn;
            // ConnectStringBulder.DataSource = _appSettings.LocalDB;
            DataTable tbResult = new DataTable();
            using (var db = new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "select * from Member ";
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                var query = sqlcmd.ExecuteReader();
                //var Result = new List<Member>();
                if (query.HasRows)
                {
                    tbResult.Load(query);
                }

            }
            var result = new
            {
                Total = 0,
                Data = tbResult
            };

            Newtonsoft.Json.Converters.IsoDateTimeConverter idtc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(result, Formatting.None, idtc);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;
using Newtonsoft.Json;
using PSDMAG.Models;
using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Reflection.PortableExecutable;

namespace PSDMAG.Services
{
   
    public interface IMemberService
    {
        string Query(string Conn);
        string Insert(string Conn, MemberActionRequest InsData);
        string Update(string Conn, MemberActionRequest InsData);
    }
    public class MemberService : IMemberService
    {
        public string Query(string Conn)
        {
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = Conn;
            var Result = new List<Member>();
            using (var db =  new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "select * from Member ";
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                var query = sqlcmd.ExecuteReader();
                //SQLiteDataAdapter SQLITEDataAdapter = new SQLiteDataAdapter(sqlcmd);
                //SQLITEDataAdapter.Fill(tbResult);


                if (query.HasRows) {
                    //for (int i = 0; i < query.FieldCount; i++)
                    //    tbResult.Columns.Add(new DataColumn(query.GetName(i)));
                    while (query.Read())
                    {
                        var Row = new Member ();
                        for (int i = 0; i < query.FieldCount; i++)
                        {
                            Row.ID = Convert.ToInt32(query.GetValue(0));
                            Row.Mname = (query.GetValue(1)).ToString();
                            Row.Mpswd = (query.GetValue(2)).ToString();
                        }
                        Result.Add(Row);
                    }
                }
               
            }

            var result = new
            {
                Total = Result.Count,
                data = Result
            };

            Newtonsoft.Json.Converters.IsoDateTimeConverter idtc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(result, Formatting.None, idtc);
        }
        public string Insert(string Conn, MemberActionRequest InsData)
        {
            var ExRes = 0;
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = Conn;
            // ConnectStringBulder.DataSource = _appSettings.LocalDB;
            DataTable tbResult = new DataTable();
            var salt = new Random().Next();
            var OK = encryption_sha512(InsData.Mpswd, salt.ToString().Substring(0,5));
            using (var db = new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "insert into Member (ID,Mname,Mpswd,salt) values  (@ID,@Mname,@Mpswd,@salt)  ";
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add( new SqliteParameter("ID", InsData.ID));
                sqlcmd.Parameters.Add(new SqliteParameter("Mname", InsData.Mname));
                sqlcmd.Parameters.Add(new SqliteParameter("Mpswd", OK));
                sqlcmd.Parameters.Add(new SqliteParameter("salt", salt.ToString().Substring(0, 5)));
                ExRes = sqlcmd.ExecuteNonQuery();
                
            }
            var result = new
            {
                Alert = ExRes == 0 ? "失敗" : "成功",
            };

            Newtonsoft.Json.Converters.IsoDateTimeConverter idtc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(result, Formatting.None, idtc);
        }
        public string Update(string Conn, MemberActionRequest InsData)
        {
            var ExRes = 0;
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = Conn;
            // ConnectStringBulder.DataSource = _appSettings.LocalDB;
            DataTable tbResult = new DataTable();
            var salt = new Random().Next();
            var OK = encryption_sha512(InsData.Mpswd, salt.ToString().Substring(0, 5));
            using (var db = new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "Update Member set Mname = @Mname,Mpswd = @Mpswd,salt = @salt where ID = @ID  ";
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add(new SqliteParameter("ID", InsData.ID));
                sqlcmd.Parameters.Add(new SqliteParameter("Mname", InsData.Mname));
                sqlcmd.Parameters.Add(new SqliteParameter("Mpswd", OK));
                sqlcmd.Parameters.Add(new SqliteParameter("salt", salt.ToString().Substring(0, 5)));
                ExRes = sqlcmd.ExecuteNonQuery();

            }
            var result = new
            {
                Alert = ExRes == 0 ? "失敗" : "成功",
            };

            Newtonsoft.Json.Converters.IsoDateTimeConverter idtc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(result, Formatting.None, idtc);
        }
        public static string encryption_sha512(string pwd, string salt)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            return Convert.ToBase64String(sha512.ComputeHash(Encoding.Default.GetBytes(pwd + salt)));
        }
    }
}

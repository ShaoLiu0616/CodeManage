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
using System.Transactions;

namespace PSDMAG.Services
{
   
    public interface IMemberService
    {
        string Query();
        string Insert(MemberActionRequest InsData);
        string Update(MemberActionRequest InsData);
        string Delete(MemberActionRequest InsData);
        string GetMem();
        string CheckMem(MemberActionRequest Request);
    }
    public class MemberService : IMemberService
    {
        private readonly AppSetting _appSettings;
        public MemberService(IOptions<AppSetting> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public string Query()
        {
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = _appSettings.LocalDB;
            var Result = new List<Member>();
            using (var db =  new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "select * from Member ";
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                var query = sqlcmd.ExecuteReader();
            
                if (query.HasRows) {

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
        public string Insert(MemberActionRequest InsData)
        {
            var ExRes = 0;
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = _appSettings.LocalDB;
            string Err = "";
            var salt = new Random().Next();
            var OK = encryption_sha512(InsData.Mpswd, salt.ToString().Substring(0,5));
            using (var db = new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "insert into Member (ID,Mname,Mpswd,salt) values  (@ID,@Mname,@Mpswd,@salt)  ";
                var trans = db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add(new SqliteParameter("ID", InsData.ID));
                sqlcmd.Parameters.Add(new SqliteParameter("Mname", InsData.Mname));
                sqlcmd.Parameters.Add(new SqliteParameter("Mpswd", OK));
                sqlcmd.Parameters.Add(new SqliteParameter("salt", salt.ToString().Substring(0, 5)));
                try
                {
                    ExRes = sqlcmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    Err = ex.Message;
                    trans.Rollback();
                }
                finally
                {
                    sqlcmd.Dispose();
                    trans.Dispose();
                }

            }
            var result = new
            {
                Alert = ExRes == 0 ? "失敗 " + Err : "成功",
            };

            Newtonsoft.Json.Converters.IsoDateTimeConverter idtc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(result, Formatting.None, idtc);
        }
        public string Update(MemberActionRequest InsData)
        {
            var ExRes = 0;
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = _appSettings.LocalDB;
            string Err = "";
            var salt = new Random().Next();
            var OK = encryption_sha512(InsData.Mpswd, salt.ToString().Substring(0, 5));
            using (var db = new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "Update Member set Mname = @Mname,Mpswd = @Mpswd,salt = @salt where ID = @ID  ";
             
                var trans = db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add(new SqliteParameter("ID", InsData.ID));
                sqlcmd.Parameters.Add(new SqliteParameter("Mname", InsData.Mname));
                sqlcmd.Parameters.Add(new SqliteParameter("Mpswd", OK));
                sqlcmd.Parameters.Add(new SqliteParameter("salt", salt.ToString().Substring(0, 5)));
                try
                {
                    ExRes = sqlcmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    Err = ex.Message;
                    trans.Rollback();
                }
                finally
                {
                    sqlcmd.Dispose();
                    trans.Dispose();
                }

            }
            var result = new
            {
                Alert = ExRes == 0 ? "失敗 " + Err : "成功",
            };

            Newtonsoft.Json.Converters.IsoDateTimeConverter idtc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(result, Formatting.None, idtc);
        }
        public string Delete(MemberActionRequest InsData)
        {
            var ExRes = 0;
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = _appSettings.LocalDB;
            var Err = "";
            using (var db = new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "Delete from Member where ID = @ID  ";
             
                var trans = db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add(new SqliteParameter("ID", InsData.ID));
                try
                {
                    ExRes = sqlcmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex) {
                    Err = ex.Message;
                    trans.Rollback();
                }
                finally
                {
                    sqlcmd.Dispose();
                    trans.Dispose();
                }


            }
            var result = new
            {
                Alert = ExRes == 0 ? "失敗 " + Err : "成功",
            };

            Newtonsoft.Json.Converters.IsoDateTimeConverter idtc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(result, Formatting.None, idtc);
        }
        public string GetMem()
        {
            var ExRes = 0;
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = _appSettings.LocalDB;
            var Err = "";
            var Result = new List<Member>();
            using (var db = new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "select ID,Mname from Member ";
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                var query = sqlcmd.ExecuteReader();

                if (query.HasRows)
                {
                    while (query.Read())
                    {
                        var Row = new Member();
                        for (int i = 0; i < query.FieldCount; i++)
                        {
                            Row.ID = Convert.ToInt32(query.GetValue(0));
                            Row.Mname = (query.GetValue(1)).ToString();
                        }
                        Result.Add(Row);
                    }
                }

            }

        
            var result = new
            {
                Data = Result,
            };

            Newtonsoft.Json.Converters.IsoDateTimeConverter idtc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(result, Formatting.None, idtc);
        }
        public string CheckMem(MemberActionRequest Request)
        {
            var ExRes = 0;
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = _appSettings.LocalDB;
            var Err = "";
            var pwd = "";
            var salt = "";
            var Msg = "驗證失敗";
            var Result = new List<Member>();
            using (var db = new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "select Mpswd,salt from Member where ID = @ID";
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add(new SqliteParameter("ID", Request.ID));
                var query = sqlcmd.ExecuteReader();

                if (query.HasRows)
                {
                    while (query.Read())
                    {
                        var Row = new Member();
                        for (int i = 0; i < 1; i++)
                        {
                            pwd = (query.GetValue(0)).ToString();
                            salt = (query.GetValue(1)).ToString();
                        }
                        Result.Add(Row);
                    }
                }

            }
            var test = encryption_sha512(Request.Mpswd, salt);
            if (test == pwd)
            {
                Msg = "驗證成功";
                ExRes = 1;
            }
            var result = new
            {
                Status = ExRes,
                Alert = Msg,
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

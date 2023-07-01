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
using Microsoft.AspNetCore.Components.Forms;
using System.Buffers.Text;

namespace PSDMAG.Services
{
   
    public interface ICodeMagService
    {
       // string Query(string Conn,string Uid);
        string Insert(string Conn, CodeMagActionRequest InsData);
        string Update(string Conn, CodeMagActionRequest InsData);
    }
    public class CodeMagService : ICodeMagService
    {
        public string Query(string Conn, string Uid)
        {
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = Conn;
            var Result = new List<CodeMag>();
            using (var db =  new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "select idx,AppName,Acc,Code,Code2,MemID from CodeMag ";
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                if (!string.IsNullOrEmpty(Uid))
                {
                    sqlcmd.Parameters.Add(new SqliteParameter("Uid", Uid));
                    sSQL += " where MemID = @Uid";
                }
                else { sSQL += " where 1 = 0"; }
                var query = sqlcmd.ExecuteReader();
                //SQLiteDataAdapter SQLITEDataAdapter = new SQLiteDataAdapter(sqlcmd);
                //SQLITEDataAdapter.Fill(tbResult);


                if (query.HasRows) {
                    //for (int i = 0; i < query.FieldCount; i++)
                    //    tbResult.Columns.Add(new DataColumn(query.GetName(i)));
                    while (query.Read())
                    {
                        var Row = new CodeMag ();
                        for (int i = 0; i < query.FieldCount; i++)
                        {
                            Row.idx = Convert.ToInt32(query.GetValue(0));
                            Row.AppName = (query.GetValue(1)).ToString();
                            Row.Acc = (query.GetValue(2)).ToString();
                            Row.Code = (query.GetValue(3)).ToString();
                            Row.Code2 = (query.GetValue(4)).ToString();
                            Row.MemID = (query.GetValue(5)).ToString();

                            Row.Code = B64ToStr(Row.Code);
                            Row.Code2 = B64ToStr(Row.Code2);
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
        private string StrToB64(string orgstr) {
            Byte[] bytesEncode = System.Text.Encoding.UTF8.GetBytes(orgstr); //取得 UTF8 2進位 Byte
            string resultEncode = Convert.ToBase64String(bytesEncode); // 轉換 Base64 索引表
           return resultEncode;
        }
        private string B64ToStr(string B64str)
        {
            Byte[] bytesDecode = Convert.FromBase64String(B64str); // 還原 Byte
            string resultText = System.Text.Encoding.UTF8.GetString(bytesDecode); // 還原 UTF8 字元
            return resultText;
        }
        public string Insert(string Conn, CodeMagActionRequest InsData)
        {
            var ExRes = 0;
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = Conn;
            // ConnectStringBulder.DataSource = _appSettings.LocalDB;
            string Err = "";
            var base642 = "";
            var base64 = StrToB64(InsData.Code);
            if (!string.IsNullOrEmpty(InsData.Code2)) { 
              base642 = StrToB64(InsData.Code2);
            }
            using (var db = new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "insert into CodeMag (AppName,Acc,Code,Code2,MemID) values  (@AppName,@Acc,@Code,@Code2,@MemID)  ";

                var trans = db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add(new SqliteParameter("AppName", InsData.AppName));
                sqlcmd.Parameters.Add(new SqliteParameter("Acc", InsData.Acc));
                sqlcmd.Parameters.Add(new SqliteParameter("Code", base64));
                sqlcmd.Parameters.Add(new SqliteParameter("Code2", base642));
                sqlcmd.Parameters.Add(new SqliteParameter("MemID", InsData.MemID));
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
        public string Update(string Conn, CodeMagActionRequest InsData)
        {
            var ExRes = 0;
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = Conn;
            string Err = "";
            var base642 = "";
            var base64 = StrToB64(InsData.Code);
            if (!string.IsNullOrEmpty(InsData.Code2))
            {
                base642 = StrToB64(InsData.Code2);
            }
            using (var db = new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "Update CodeMag set AppName = @AppName,Acc = @Acc,Code = @Code,Code2 = @Code2 where idx = @idx  ";

                var trans = db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add(new SqliteParameter("idx", InsData.idx));
                sqlcmd.Parameters.Add(new SqliteParameter("AppName", InsData.AppName));
                sqlcmd.Parameters.Add(new SqliteParameter("Acc", InsData.Acc));
                sqlcmd.Parameters.Add(new SqliteParameter("Code", base64));
                sqlcmd.Parameters.Add(new SqliteParameter("Code2", base642));
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
        public string Delete(string Conn, CodeMagActionRequest InsData)
        {
            var ExRes = 0;
            var ConnectStringBulder = new SqliteConnectionStringBuilder();
            ConnectStringBulder.DataSource = Conn;
            var Err = "";
            using (var db = new SqliteConnection(ConnectStringBulder.ConnectionString))
            {
                db.Open();
                string sSQL = "Delete from CodeMag where idx = @idx  ";

                var trans = db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var sqlcmd = db.CreateCommand();
                sqlcmd.CommandText = sSQL;
                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add(new SqliteParameter("idx", InsData.idx));
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
        public static string encryption_sha512(string pwd, string salt)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            return Convert.ToBase64String(sha512.ComputeHash(Encoding.Default.GetBytes(pwd + salt)));
        }
    }
}

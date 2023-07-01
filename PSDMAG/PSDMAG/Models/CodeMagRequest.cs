using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace PSDMAG.Models
{
    public class CodeMagActionRequest
    {
        public int idx { get; set; }
        public string AppName { get; set; }
        public string Acc { get; set; }
        public string Code { get; set; }
        public string Code2 { get; set; }
        public string MemID { get; set; }

    }
}

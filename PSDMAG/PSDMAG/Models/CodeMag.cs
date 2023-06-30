using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace PSDMAG.Models
{
    public class CodeMag
    {
        [Key]
        public int idx { get; set; }
        [Required]
        public string AppName { get; set; }
        [Required]
        public string Acc { get; set; }
        [Required]
        public string Code { get; set; }
        public string Code2 { get; set; }
        public string MemID { get; set; }

    }
}

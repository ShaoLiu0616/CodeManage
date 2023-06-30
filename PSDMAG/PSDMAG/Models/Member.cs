using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace PSDMAG.Models
{
    public class Member
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Mname { get; set; }
        [Required]
        public string Mpswd { get; set; }
        public string salt { get; set; }

    }
}

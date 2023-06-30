using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace PSDMAG.Models
{
    public class MemberActionRequest
    {
        public int ID { get; set; }
        public string Mname { get; set; }
        public string Mpswd { get; set; }
        public string salt { get; set; }

    }
}

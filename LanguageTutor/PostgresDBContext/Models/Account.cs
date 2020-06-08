using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBContext.Models
{
    public partial class Account
    {
        public int IdAccount { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime? DateRegistration { get; set; }

        [NotMapped]
        public string NewPassword { get; set; }

    }
}

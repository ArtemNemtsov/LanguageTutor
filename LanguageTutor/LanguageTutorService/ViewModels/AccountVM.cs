using DBContext.Models;
using System;
using System.Collections.Generic;

namespace LanguageTutorService.ViewModels
{
    public class AccountVM
    {
        public IEnumerable<TtutorAudit> History { get; set; }
        public string Login { get; set; }
        public int CountAnswer { get; set; }
        public DateTime LastVisit { get; set; }
    }
}

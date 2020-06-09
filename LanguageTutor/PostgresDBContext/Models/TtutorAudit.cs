using System;
using System.Collections.Generic;

namespace DBContext.Models
{
    public partial class TtutorAudit
    {
        public long IdTutorAudit { get; set; }
        public string NameLogin { get; set; }
        public string LanguageFrom { get; set; }
        public string LanguageTo { get; set; }
        public string Word { get; set; }
        public string CorrectTranslation { get; set; }
        public string UserTranslation { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime Time { get; set; }
    }
}

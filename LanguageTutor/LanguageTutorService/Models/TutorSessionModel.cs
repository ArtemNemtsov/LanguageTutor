using LanguageTutor.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageTutorService.Models
{
    public class TutorSessionModel
    {
        public InputLangOption LangOption { get; set; }
        public string Word { get; set; }
        public string Translation { get; set; }
    }
}

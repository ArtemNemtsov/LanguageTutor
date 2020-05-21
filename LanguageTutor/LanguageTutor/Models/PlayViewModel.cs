using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageTutor.Views.TutorMenu
{
    public class PlayViewModel
    {
        public string Translation { get; set; }
        public string Word { get; set; }
        public string WordHelp { get; set; }
        public string UserAnswer { get; set; }
        public string ExTranslate { get; set; }
        public int SumError { get; set; }
        public int SumRight { get; set; }

    }
}

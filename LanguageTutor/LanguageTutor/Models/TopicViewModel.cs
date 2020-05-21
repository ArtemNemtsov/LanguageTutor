using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageTutor.Views.TutorMenu
{
    public class TopicViewModel
    {
        public string Message { get; set; }

        public int IdTopic { get; set; }
        public SelectList TopicList { get; set; }
    }
}

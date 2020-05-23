using Microsoft.AspNetCore.Mvc.Rendering;

namespace LanguageTutorService.ViewModels
{
    public class TopicViewModel
    {
        public int IdTopic { get; set; }
        public SelectList TopicList { get; set; }
    }
}

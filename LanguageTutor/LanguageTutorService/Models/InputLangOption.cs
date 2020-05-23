using LanguageTutor.Pages;

namespace LanguageTutorService.Models
{
    public class InputLangOption
    {
        public int IdTopic { get; set; }
        public Languages LngFrom { get; set; }
        public Languages LngTo { get; set; }
    }
}

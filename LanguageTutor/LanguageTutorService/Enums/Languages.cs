using System.ComponentModel.DataAnnotations;

namespace LanguageTutor.Pages
{
    public enum Languages
    {
        [Display(Name = "Русский")]
        ru,

        [Display(Name = "Английский")]
        en,

        [Display(Name = "Немецкий")]
        de,

        [Display(Name = "Французский")]
        fr,
    }
}

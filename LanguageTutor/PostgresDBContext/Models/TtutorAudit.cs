using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBContext.Models
{
    public partial class TtutorAudit
    {
        public long IdTutorAudit { get; set; }

        [Display(Name = "Логин")]
        public string NameLogin { get; set; }

        [Display(Name = "С")]
        public string LanguageFrom { get; set; }

        [Display(Name = "На")]
        public string LanguageTo { get; set; }

        [Display(Name = "Слово")]
        public string Word { get; set; }

        [Display(Name = "Правильный ответ")]
        public string CorrectTranslation { get; set; }

        [Display(Name = "Ваш ответ")]
        public string UserTranslation { get; set; }

        [Display(Name = "Вердикт")]
        public bool IsCorrect { get; set; }

        [Display(Name = "Время")]
        public DateTime Time { get; set; }
    }
}

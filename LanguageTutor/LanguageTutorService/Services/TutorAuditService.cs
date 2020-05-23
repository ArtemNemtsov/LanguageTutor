using DBContext.Connect;
using DBContext.Models;
using LanguageTutorService.Models;

namespace LanguageTutorService.Services
{
    public class TutorAuditService
    {
        private readonly dc58kv94isevv4Context _postgres;

        public TutorAuditService(dc58kv94isevv4Context postgres)
        {
            _postgres = postgres;
        }

        public void AddAudit(TutorSessionModel sessionModel, InputUserAnswer userAnswer, string login)
        {
            var tutorAudit = new TtutorAudit
            {
                LanguageFrom = sessionModel.LangOption.LngFrom.ToString(),
                LanguageTo = sessionModel.LangOption.LngTo.ToString(),
                NameLogin = login,
                Word = sessionModel.Word,
                CorrectTranslation = sessionModel.Translation,
                UserTranslation = userAnswer.UserTranslation,
                IsCorrect = sessionModel.Translation == userAnswer.UserTranslation,
            };

            _postgres.TtutorAudit.Add(tutorAudit);
            _postgres.SaveChanges();
        }
    }
}

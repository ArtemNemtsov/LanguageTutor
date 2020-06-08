using DBContext.Connect;
using DBContext.Models;
using LanguageTutorService.Models;
using System;
using System.Linq;

namespace LanguageTutorService.Services
{
    public class TutorAuditService
    {
        private readonly dc58kv94isevv4Context _postgres;

        public TutorAuditService(dc58kv94isevv4Context postgres)
        {
            _postgres = postgres;
        }

        public IQueryable<TtutorAudit> GetHistory(string login)
        {
            return  _postgres.TtutorAudit.Where(a => a.NameLogin == login)
                .OrderByDescending(a => a.Time)
                .Take(50);
        }

        public DateTime GetLastVisit(string login)
        {
            return _postgres.TtutorAudit.Where(a => a.NameLogin == login).
                OrderByDescending(x => x.Time)
              .FirstOrDefault().Time; 
        }

        public int GetCountAnswer(string login)
        {
            return _postgres.TtutorAudit.Where(a => a.NameLogin == login).Count();            
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

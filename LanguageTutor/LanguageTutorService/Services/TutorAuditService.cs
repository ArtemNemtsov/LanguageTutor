using DBContext.Connect;
using DBContext.Models;
using LanguageTutor.Controllers;
using LanguageTutorService.Models;
using System;
using System.Collections.Generic;
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

        public IEnumerable<ExcelTutorHistory> GetHistoryForExcel(string login)
        {
            return _postgres.TtutorAudit.Where(a => a.NameLogin == login)
                .OrderByDescending(a => a.Time)
                .Take(50)
                .Select(s => new ExcelTutorHistory 
                {
                    Логин = s.NameLogin,
                    С = s.LanguageFrom,
                    На = s.LanguageTo,
                    Слово  = s.Word,
                    Правильный_ответ = s.CorrectTranslation,
                    Ваш_ответ = s.UserTranslation,
                    Вердикт = s.IsCorrect 
                    ? "Верно"
                    : "Неверно",
                    Время = s.Time,

                })
                .ToList();
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

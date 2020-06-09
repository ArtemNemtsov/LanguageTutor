using DBContext.Connect;
using DBContext.Models;
using LanguageTutor.Controllers;
using LanguageTutorService.Models;
using LanguageTutorService.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageTutorService.Services
{
    public class TutorAuditService
    {
        private readonly dc58kv94isevv4Context _postgres;

        public TutorAuditService(dc58kv94isevv4Context postgres)
        {
            _postgres = postgres;
        }

        public async Task<AccountVM> GetAccountViewModel(string userLogin)
        {
            var history = await GetHistory(userLogin).ToListAsync();

            // создаем экземпляр класса и помещаем туда данные
            return new AccountVM
            {
                History = history,
                Login = userLogin,
                LastVisit = GetLastVisitAsync(userLogin).Result,
                CountAnswer = GetCountAnswer(userLogin),

                // если история не пустая, строим рейтинг, иначе реитинг 0
                Reiting = GetReiting(userLogin),
            };
        }

        public async Task<AccountVM> GetMainViewModel(string userLogin)
        {
            // создаем экземпляр класса и помещаем туда данные
            return new AccountVM
            {
                Login = userLogin,
                LastVisit = GetLastVisitAsync(userLogin).Result,
                CountAnswer = GetCountAnswer(userLogin),
                // если история не пустая, строим рейтинг, иначе реитинг 0
                Reiting = GetReiting(userLogin),
            };
        }

        public double GetReiting(string login)
        {
            double countAllAnswer = _postgres.TtutorAudit.AsNoTracking()
                .Where(a => a.NameLogin == login ).Count();

            double countRight = _postgres.TtutorAudit.AsNoTracking()
                .Where(a => a.NameLogin == login && a.IsCorrect == true).Count();

            // если есть отвеченные слова, то считаем рейтин, иначе 0
            if (countAllAnswer > 0 && countRight > 0)
            {
                // количество верных слов делим на общее количество
                double reiting = countRight / countAllAnswer;

                //округляем до 2 чисел
                return Math.Round(reiting, 2);
            }
            else return 0;
        }


        public IQueryable<TtutorAudit> GetHistory(string login)
        {
            return  _postgres.TtutorAudit
                .Where(a => a.NameLogin == login)
                .AsNoTracking()
                .OrderByDescending(a => a.Time)
                .Take(20);
        }

        public IEnumerable<ExcelTutorHistory> GetHistoryForExcel(string login)
        {
            return _postgres.TtutorAudit.Where(a => a.NameLogin == login)
                .OrderByDescending(a => a.Time)
                .AsNoTracking()
                .Take(20)
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

        public async Task<DateTime> GetLastVisitAsync(string login)
        {
            var result  = await _postgres.TtutorAudit
               .Where(a => a.NameLogin == login)
               .AsNoTracking()
               .OrderByDescending(x => x.Time)
              .FirstOrDefaultAsync();

            return result == null
                ? DateTime.Now
                : result.Time;
        }

        public int GetCountAnswer(string login)
        {
            return _postgres.TtutorAudit
                .AsNoTracking()
                .Where(a => a.NameLogin == login)
                .Count();            
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

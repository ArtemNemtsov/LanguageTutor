using DBContext.Connect;
using LanguageTutor.Pages;
using LanguageTutorService.Models;
using LanguageTutorService.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace LanguageTutorService.Services
{
    public class TutorService
    {
        private readonly dc58kv94isevv4Context _postgres;
        private readonly TranslatorService _translator;
        private readonly TutorAuditService _audit;
        private readonly SessionService _session;
        private Random rnd;

        public TutorService(dc58kv94isevv4Context postgres, TranslatorService translator, TutorAuditService audit, SessionService session)
        {
            _postgres = postgres;
            _translator = translator;
            _audit = audit;
            _session = session;
            rnd = new Random();
        }

        public PlayVM Continue(HttpContext context, InputUserAnswer userAnswer)
        {
           var session =  _session.GetSessionModel(context);
            _audit.AddAudit(session, userAnswer, context.User.Identity.Name);

            var langOption = session.LangOption;
            var newPlayVm = NextPlayVM(langOption);

            session.Word = newPlayVm.Word;
            session.Translation = newPlayVm.Translation;

            _session.Update(context, session);
            return newPlayVm;
        }

        public PlayVM NewGame(HttpContext context, InputLangOption langOption)
        {
            var playVM = NextPlayVM(langOption);

            _session.SaveTutorContext(context, langOption, playVM);

            return playVM;
        }

        private PlayVM NextPlayVM(InputLangOption langOption)
        {
            var russVocab = langOption.IdTopic == 0
                ? _postgres.Word.ToList()
                : _postgres.Word.Where(w => w.IdTopic == langOption.IdTopic).ToList();

            var rndNum = rnd.Next(russVocab.Count);
            var rusWord = russVocab[rndNum].NameWord;

            var nativWord = langOption.LngFrom == Languages.ru
                ? rusWord
                : _translator.TranslateAsync(rusWord, Languages.ru, langOption.LngFrom).Result;

            var translation = _translator.TranslateAsync(nativWord, langOption).Result;

            return new PlayVM
            {
                Word = nativWord,
                WordHelp = GetWordHelp(translation),
                Translation = translation,
            };
        }

        private string GetWordHelp(string translate)
        {
            int lengthWord = translate.Length;
            string endWord = string.Empty;

            for (int i = 0; i < lengthWord / 2; i++)
            {
                endWord += "*";
            }
            return translate.Remove(lengthWord - lengthWord / 2) + endWord + "  ";
        }
    }
}
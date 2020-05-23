using LanguageTutorService.Models;
using LanguageTutorService.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageTutorService.Services
{
    public class SessionService
    {
        private readonly string sessionKeyEnd = "Tutor";
        public void SaveTutorContext(HttpContext context, InputLangOption langOption, PlayVM playVM)
        {
            var sessionKey = context.User.Identity.Name + sessionKeyEnd;
            ISession session = context.Session;

            var tutorSession = new TutorSessionModel
            {
                LangOption = langOption,
                Word = playVM.Word,
                Translation = playVM.Translation,
            };
            session.SetString(sessionKey, JsonConvert.SerializeObject(tutorSession));
        }

        public void Update(HttpContext context, TutorSessionModel sessionModel)
        {
            var sessionKey = context.User.Identity.Name + sessionKeyEnd;
            ISession session = context.Session;
            session.SetString(sessionKey, JsonConvert.SerializeObject(sessionModel));
        }

        public TutorSessionModel GetSessionModel(HttpContext context)
        {
            var sessionKey = context.User.Identity.Name + sessionKeyEnd;
            ISession session = context.Session;
            var value = session.GetString(sessionKey);

            return value == null 
                ? new TutorSessionModel() 
                : JsonConvert.DeserializeObject<TutorSessionModel>(value);
        }
    }
}

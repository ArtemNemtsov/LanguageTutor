using LanguageTutor.Views.TutorMenu;
using LanguageTutorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LanguageTutor.Controllers
{
    [Authorize]
    public class TutorMenuController : Controller
    {
       private  TopicViewModel tutorMenuModel;
       private readonly TopicService service;

        public TutorMenuController(TopicService topicService)
        {
            service = topicService;
        }

        [Authorize]
        public IActionResult Main()
        {
            return View();
        }

        public IActionResult SelectLang()
        {
            var topics = service.GetTopics();

            tutorMenuModel = new TopicViewModel
            {
                Message = string.Empty,
                TopicList = new SelectList(topics, "IdTopic", "NameTopic"),
            };

            return View(tutorMenuModel);
        }

        public IActionResult Play()
        {
            PlayViewModel play = new PlayViewModel
            {
                Word = "someWord",
                Translation = "Translation",
                WordHelp = "WordHelp",
                SumError = 0,
                SumRight = 0,

            };

            return View(play);
        }
    }
}
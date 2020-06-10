using LanguageTutorService;
using LanguageTutorService.Services;
using LanguageTutorService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace LanguageTutor.Controllers
{
    [Authorize]
    public class TutorMenuController : Controller
    {
       private TopicViewModel tutorMenuModel;
       private readonly TopicService _topicService;
       private readonly TutorAuditService _auditTutor;

        public TutorMenuController(TopicService topicService, TutorAuditService auditTutor)
        {
            _topicService = topicService;
            _auditTutor = auditTutor;;
        }


        public async Task<IActionResult> Main()
        {
            // из запроса получаем логин
            var userLogin = this.HttpContext.User.Identity.Name;
            var result = _auditTutor.GetMainViewModel(userLogin).Result;

            // во вьюху передаем созданный класс с данными
            return View(result);
        }


        [HttpGet]
        public IActionResult SelectLang()
        {
            var topics = _topicService.GetTopics();

            tutorMenuModel = new TopicViewModel
            {
                TopicList = new SelectList(topics, "IdTopic", "NameTopic"),
            };

            return View(tutorMenuModel);
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var userLogin = this.HttpContext.User.Identity.Name;
            var result = _auditTutor.GetAccountViewModel(userLogin).Result;

            // во вьюху передаем созданный класс с данными
            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> AccountStatistic()
        {
            var userLogin = this.HttpContext.User.Identity.Name;
            var result = _auditTutor.GetAccountViewModel(userLogin).Result;

            // во вьюху передаем созданный класс с данными
            return View(result);
        }

        [HttpGet]
        public IActionResult OurContact()
        {
            return View();
        }
    }
}
using DBContext.Connect;
using LanguageTutorService;
using LanguageTutorService.Services;
using LanguageTutorService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        public IActionResult Main()
        {
            return View();
        }
        
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

            var history = _auditTutor.GetHistory(userLogin);

            var accountVM = new AccountVM
            {
                History = await history.ToListAsync(),
                Login = userLogin,
                LastVisit = _auditTutor.GetLastVisit(userLogin),
                CountAnswer = _auditTutor.GetCountAnswer(userLogin)
            };

            return View(accountVM);
        }

        [HttpGet]
        public IActionResult OurContact()
        {
            return View();
        }
    }
}
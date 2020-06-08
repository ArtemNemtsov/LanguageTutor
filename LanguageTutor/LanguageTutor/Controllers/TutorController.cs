using LanguageTutorService.Models;
using LanguageTutorService.Services;
using MediaStudioService.Core.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguageTutor.Controllers
{
    [Authorize]
    public class TutorController : Controller
    {
        private readonly TutorService _tutorService;

        public TutorController(TutorService tutorService)
        {
            _tutorService = tutorService;
        }

        [HttpPost]
        public IActionResult Start(InputLangOption langOption)
        {
            var playModel = _tutorService.NewGame(this.HttpContext, langOption);
            return View(playModel);
        }

        [HttpPost]
        public JsonResult Continue(InputUserAnswer userAnswer)
        {
            var continueModel = _tutorService.Continue(this.HttpContext, userAnswer);
            return Json(RespоnceManager.CreateSucces(continueModel));
        }
    }
}
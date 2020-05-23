using Microsoft.AspNetCore.Mvc;

namespace LanguageTutor.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public ActionResult Greeting()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

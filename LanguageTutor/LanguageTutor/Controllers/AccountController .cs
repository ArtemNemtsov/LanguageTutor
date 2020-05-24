using LanguageTutorService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguageTutor.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly TutorAuditService _auditTutor;

        public AccountController(TutorAuditService auditTutor)
        {
            _auditTutor = auditTutor;
        }
    }
}
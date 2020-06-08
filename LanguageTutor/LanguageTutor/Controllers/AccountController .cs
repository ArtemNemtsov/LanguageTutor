using DBContext.Models;
using LanguageTutorService;
using LanguageTutorService.Services;
using MediaStudioService.Core.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LanguageTutor.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly TutorAuditService _auditTutor;
        private readonly AccountService _accountService;

        public AccountController(TutorAuditService auditTutor, AccountService accountService)
        {
            _auditTutor = auditTutor;
            _accountService = accountService;

        }

        [HttpPost]
        public JsonResult ChangePassword(Account account)
        {
            try
            {
                // из запроса получаем логин
                var login = this.HttpContext.User.Identity.Name;

                // в полученный класс аккаунт добавляем полученный логин
                account.Login = login;

                 // проверяем что логин и пароль верны
                _accountService.CheckLoginPasswordCorrectly(account);

                // меняем пароль
                 var result =_accountService.ChangePassword(account);

                return Json(RespоnceManager.CreateSucces(result));
            }
            catch (Exception ex)
            {
                // в случае неудачи возвращаем причину 
                return Json(RespоnceManager.CreateError(ex));
            }
        }
    }
}
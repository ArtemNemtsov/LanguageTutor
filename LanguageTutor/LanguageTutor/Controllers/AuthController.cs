using DBContext.Models;
using LanguageTutorService;
using MediaStudioService.Core.Classes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LanguageTutor.Controllers
{
    public class AuthController : Controller
    {
        private readonly AccountService auth;
        private readonly LinkGenerator linkGenerator;

        public AuthController(AccountService authService, LinkGenerator link, IHttpContextAccessor _accessor)
        {
            auth = authService;
            linkGenerator = link;
        }

        [HttpPost]
        public async Task<JsonResult> SignInAsync(Account inputAccount)
        {
            try
            {
                // если такой аккаунт существует, то
                auth.CheckAccountExist(inputAccount);

               // аутентификация
               await Authenticate(inputAccount.Login);

                //создаем ссылку, и отправляем ее, чтобы потом зайти по ней

                string scheme = Url.ActionContext.HttpContext.Request.Scheme;
                var callbackLink =  Url.Action("Main", "TutorMenu", null, scheme);

                // возвращаем результат
                return Json(RespоnceManager.CreateSucces(callbackLink));
            }
            catch (Exception ex)
            {
                // в случае неудачи возвращаем причину 
                return Json(RespоnceManager.CreateError(ex));
            }
        }

        [HttpPost]
        public JsonResult SignUp(Account newAccount)
        {
            try
            {
                auth.AddAccount(newAccount);
                return Json(RespоnceManager.CreateSucces("Аккаунт успешно добавлен!"));
            }
            catch (Exception ex)
            {
                // в случае неудачи возвращаем причину 
                return Json(RespоnceManager.CreateError(ex));
            }
        }
        public async Task<IActionResult> SignOut()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.RedirectToAction("Greeting", "Home");
        }

        private async Task Authenticate(string login)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };

            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
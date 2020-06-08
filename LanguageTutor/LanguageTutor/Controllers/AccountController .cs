using DBContext.Models;
using Fingers10.ExcelExport.ActionResults;
using LanguageTutorService;
using LanguageTutorService.Services;
using MediaStudioService.Core.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> GetExcel()
        {
            // из запроса получаем логин
            var userLogin = this.HttpContext.User.Identity.Name;

            // из БД загружаем статистику для данного логина
            var history = _auditTutor.GetHistoryIEnumerable(userLogin);

            return new ExcelResult<TtutorAudit>(history, "Статистика", $"{userLogin} TutorLanguage");
        }

        public IActionResult LoadPhoto(IFormFile files)
        {
            try
            {
                // проверяем что пришел именно файл изображение
                CheckValidImageType(files);

                using (var memoryStream = new MemoryStream())
                {
                    using var img = Image.FromStream(memoryStream);
                    // TODO: ResizeImage(img, 100, 100);
                }

                return Json(RespоnceManager.CreateSucces("Фото успешно загружено!"));
            }
            catch (Exception ex)
            {
                return Json(RespоnceManager.CreateError(ex));
            }
        }

        // метод проверяет что дествительно пришел файл изображение
        private void CheckValidImageType(IFormFile postedFile)
        {
            // проверяем что размер не 0
            if (postedFile == null || postedFile.Length == 0)
                throw new InvalidOperationException($"Ошибка загрузки, выберете файл соовествующий графическому формату!");

            // проверяем что размер что тип сооветсвут изображения(а не просто переименовали расширение) 
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                            postedFile.ContentType.ToLower() != "image/jpeg" &&
                            postedFile.ContentType.ToLower() != "image/pjpeg" &&
                            postedFile.ContentType.ToLower() != "image/gif" &&
                            postedFile.ContentType.ToLower() != "image/x-png" &&
                            postedFile.ContentType.ToLower() != "image/png")
                {
                throw new InvalidOperationException($"Ошибка загрузки, выберете файл соовествующий графическому формату!");
            }

            // проверяем расширение файла
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                    && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                    && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                    && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
                {
                throw new InvalidOperationException($"Ошибка загрузки, выберете файл соовествующий графическому формату!");
                }
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
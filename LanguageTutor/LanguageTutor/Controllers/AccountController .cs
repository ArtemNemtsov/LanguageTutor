using DBContext.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using LanguageTutorService;
using LanguageTutorService.Services;
using MediaStudioService.Core.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

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

        public FileResult GetExcel()
        {
            // из запроса получаем логин
            var userLogin = this.HttpContext.User.Identity.Name;

            // из БД загружаем статистику для данного логина
            var history = _auditTutor.GetHistoryForExcel(userLogin);

            // Lets converts our object data to Datatable for a simplified logic.
            // Datatable is most easy way to deal with complex datatypes for easy reading and formatting. 
            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(history), (typeof(DataTable)));

            SpreadsheetDocument document;
            MemoryStream stream = new MemoryStream();
            using (document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "История ответов" };

                sheets.Append(sheet);

                Row headerRow = new Row();

                List<String> columns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);

                    Cell cell = new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(column.ColumnName)
                    };

                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in table.Rows)
                {
                    Row newRow = new Row();
                    foreach (String col in columns)
                    {
                        Cell cell = new Cell
                        {
                            DataType = CellValues.String,
                            CellValue = new CellValue(dsrow[col].ToString())
                        };
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                workbookPart.Workbook.Save();
                document.Close();
            }


            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"LanguageTutor {userLogin}.xlsx");
        }

        public IActionResult LoadPhoto(IFormFile files)
        {
            try
            {
                // из запроса получаем логин
                var userLogin = this.HttpContext.User.Identity.Name;

                // проверяем что пришел именно файл изображение
                CheckValidImageType(files);            

                // обновляем фото в БД
                var isSuccess = _accountService.UpdatePhoto(files, userLogin);
                if(!isSuccess)
                    throw new InvalidOperationException($"Ошибка загрузки файла в БД!");



                return Json(RespоnceManager.CreateSucces("Фото профиля успешно обновлено"));
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
                _accountService.CheckPasswordCorrect(account);

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
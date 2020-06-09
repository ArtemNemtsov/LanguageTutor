using DBContext.Connect;
using DBContext.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace LanguageTutorService
{
    public class AccountService
   {
        private readonly dc58kv94isevv4Context postgres;

        public AccountService(dc58kv94isevv4Context postgresDB)
        {
            postgres = postgresDB;
        }

        public  bool IsModelValidAsync(Account account)
        {
            return account.Login.Length > 3 && account.Password.Length > 3;
        }

        public void AddAccount(Account account)
        {
            // проверяем, что такого логина не существует
            CheckLoginExist(account);

            // Проверяем для именипароли и имени аккаунта
            CheckLengthAccount(account);

            // добавляем логин в БД и сохраняем
            postgres.Add(account);
            postgres.SaveChanges();
        }


        public void UpdatePhoto(IFormFile formFile, string login)
        {
            byte[] newPhoto = null; ;
            if (formFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    formFile.CopyTo(ms);
                    newPhoto = ms.ToArray();

                    var account = postgres.Account.Where(w => w.Login == login).FirstOrDefault();
                    account.Photo = newPhoto;
                    postgres.Update(account);
                    postgres.SaveChanges();
                }
            }
        }

        public void GetPhoto(Account account)
        {
            // проверяем, что такого логина не существует
            CheckLoginExist(account);

            // Проверяем для именипароли и имени аккаунта
            CheckLengthAccount(account);

            // добавляем логин в БД и сохраняем
            postgres.Add(account);
            postgres.SaveChanges();
        }
        public string ChangePassword(Account userAccount)
        {
            // по логину находи из БД учетку
            var postgresAccount = postgres.Account
                .Where(w => w.Login == userAccount.Login).FirstOrDefault();

            // меняем пароль
            postgresAccount.Password = userAccount.NewPassword;
            postgres.SaveChanges();

            return $"Пароль для логина {userAccount.Login} успешно изменен!";
        }

        private void CheckLoginExist(Account account)
        {
            // если такой логин уже существуетЖ то выдаем об этом ошибку
            if( postgres.Account.Any(a => a.Login == account.Login))
                throw new InvalidOperationException($"Данный пользователь уже существует");
        }

        private void CheckLengthAccount(Account account)
        {
            if (account.Login.Length >= 30)
                throw new InvalidOperationException($"Длина логина должна быть менее 30 символов");

            if (account.Password.Length >= 30)
                throw new InvalidOperationException($"Длина пароля должна быть менее 30 символов");

            if (account.Login.Length < 5)
                throw new InvalidOperationException($"Длина логина должна быть более 5 символов");

            if (account.Password.Length < 5)
                throw new InvalidOperationException($"Длина пароля должна быть более 5 символов");
        }

        public void CheckAccountExist(Account account)
        {
            // если такого аккаунта нет в БД, то вызываем ошибку
            if (!postgres.Account.Any(a => a.Login == account.Login && a.Password == account.Password))
                throw new InvalidOperationException($"Аккаунт с таким логином и паролем не существует!");
        }

        public void CheckPasswordCorrect(Account account)
        {
            // если такого аккаунта нет в БД, то вызываем ошибку
            if (!postgres.Account.Any(a => a.Login == account.Login && a.Password == account.Password))
                throw new InvalidOperationException($"Введите верный пароль!");
        }
    }
}

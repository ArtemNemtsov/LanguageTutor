using DBContext.Connect;
using DBContext.Models;
using System;
using System.Linq;

namespace LanguageTutorService
{
    public class AuthService
   {
        private readonly dc58kv94isevv4Context postgres;

        public AuthService(dc58kv94isevv4Context postgresDB)
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

            // добавляем логин в БД и сохраняем
            postgres.Add(account);
            postgres.SaveChanges();
        }

        private void CheckLoginExist(Account account)
        {
            // если такой логин уже существуетЖ то выдаем об этом ошибку
            if( postgres.Account.Any(a => a.Login == account.Login))
                throw new InvalidOperationException($"Данный пользователь уже существует");
        }

        public void CheckAccountExist(Account account)
        {
            // если такого аккаунта нет в БД, то вызываем ошибку
            if (!postgres.Account.Any(a => a.Login == account.Login && a.Password == account.Password))
                throw new InvalidOperationException($"Неверный логин или пароль");
        }
    }
}

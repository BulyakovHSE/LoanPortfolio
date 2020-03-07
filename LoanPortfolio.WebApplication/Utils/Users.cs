using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanPortfolio.WebApplication
{
    public class Users
    {
        static private bool ok;

        //Проверка при авторизации
        public static (List<string> errors, User user) CheckAuth(string login, string password, IEnumerable<User> users)
        {
            User user = new User();
            List<string> errors = new List<string>();

            (ok, user.Email) = Utils.CheckTextIsNotNull(login);
            if (!ok) errors.Add("Введите логин");

            (ok, user.Password) = Utils.CheckTextIsNotNull(password);
            if (!ok) errors.Add("Введите пароль");

            if (errors.Count != 0) return (errors, user);

            User user1 = users.SingleOrDefault(x => x.Email == user.Email);
            if (user1 == null)
            {
                errors.Add("Неверный логин");
            }
            else
            {
                if (user1.Password != user.Password) errors.Add("Неверный пароль");
            }

            return (errors, user);
        }

        //Проверка при регистрации
        public static (List<string> errors, User user) CheckRegister(string firstName, string lastName, string login, string password, IEnumerable<User> users)
        {
            User user = new User();
            List<string> errors = new List<string>();

            (ok, user.FirstName) = Utils.CheckTextIsNotNull(firstName);
            if (!ok) errors.Add("Введите Имя");

            (ok, user.LastName) = Utils.CheckTextIsNotNull(lastName);
            if (!ok) errors.Add("Введите Фамилию");

            (ok, user.Email) = Utils.CheckTextIsNotNull(login);
            if (!ok) errors.Add("Введите Email");

            (ok, user.Password) = Utils.CheckTextIsNotNull(password);
            if (!ok) errors.Add("Введите пароль");

            User user1 = users.SingleOrDefault(x => x.Email == user.Email);
            if (user1 != null)
            {
                errors.Add("Пользователь с таким Email уже существует");
            }

            return (errors, user);
        }


        //Проверка при изменении данных
        public static (List<string> errors, User user) CheckUser(int userId, string firstName, string lastName, string login, string password, IEnumerable<User> users)
        {
            User user = new User();
            List<string> errors = new List<string>();

            (ok, user.FirstName) = Utils.CheckTextIsNotNull(firstName);
            if (!ok) errors.Add("Введите Имя");

            (ok, user.LastName) = Utils.CheckTextIsNotNull(lastName);
            if (!ok) errors.Add("Введите Фамилию");

            (ok, user.Email) = Utils.CheckTextIsNotNull(login);
            if (!ok) errors.Add("Введите Email");

            (ok, user.Password) = Utils.CheckTextIsNotNull(password);
            if (!ok) errors.Add("Введите пароль");

            User user1 = users.SingleOrDefault(x => x.Email == user.Email && x.Id != userId);
            if (user1 != null)
            {
                errors.Add("Пользователь с таким Email уже существует");
            }

            return (errors, user);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;
using LoanPortfolio.WebApplication.Security;

namespace LoanPortfolio.WebApplication.Controllers
{
    public class HomeController : BaseController
    {
        public IUserService _userService;

        public HomeController(IUserService userService, IAuthService authService) : base(authService, userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Кредитный портфель";
            ViewBag.User = CurrentUser;

            return View();
        }

        public ActionResult Close()
        {
            ViewBag.Title = "Кредитный портфель";
            _authService.Logout();
            ViewBag.User = null;

            return View("Index");
        }

        public ActionResult RestorePassword()
        {
            ViewBag.Title = "Восстановление пароля";

            return View();
        }

        [HttpPost]
        public ActionResult RestorePassword(string login)
        {
            (List<string> errors, User user) = Users.CheckRestorePassword(login, _userService.GetAll());
            if (errors.Count == 0)
            {
                ViewBag.Title = "Кредитный портфель";

                return View("Index");
            }

            ViewBag.Errors = errors;
            ViewBag.Title = "Восстановление пароля";
            ViewBag.User = user;

            return View();
        }

        public ActionResult Auth()
        {
            ViewBag.Title = "Авторизация";

            return View();
        }

        [HttpPost]
        public ActionResult Auth(string login, string password)
        {
            (List<string> errors, User user) = Users.CheckAuth(login, password, _userService.GetAll());
            if (errors.Count == 0)
            {
                ViewBag.Title = "Кредитный портфель";
                _authService.Login(user.Email, user.Password, true);
                ViewBag.User = _userService.GetAll().SingleOrDefault(x => x.Email == user.Email);
                return View("Index");
            }

            ViewBag.Errors = errors;
            ViewBag.Title = "Авторизация";
            ViewBag.User = CurrentUser;

            return View();
        }

        public ActionResult Register()
        {
            ViewBag.Title = "Регистрация";
            ViewBag.User = new User();

            return View();
        }

        [HttpPost]
        public ActionResult Register(string firstName, string lastName, string login, string password)
        {
            (List<string> errors, User user) = Users.CheckRegister(firstName, lastName, login, password, _userService.GetAll());
            if (errors.Count == 0)
            {
                _userService.Add(user.Email, user.Password, user.FirstName, user.LastName);
                _authService.Login(user.Email, user.Password, true);
                ViewBag.Title = "Кредитный портфель";
                ViewBag.User = user;
                return View("Index");
            }

            ViewBag.Title = "Регистрация";
            ViewBag.Errors = errors;
            ViewBag.User = user;
            return View();
        }

        public ActionResult PC()
        {
            ViewBag.Title = "Личный кабинет";
            ViewBag.User = CurrentUser;

            return View();
        }

        [HttpPost]
        public ActionResult PC(int userId, string firstName, string lastName, string login, string password)
        {
            (List<string> errors, User user) = Users.CheckUser(userId, firstName, lastName, login, password, _userService.GetAll());
            User oldUser = _userService.GetById(userId);
            ViewBag.Title = "Личный кабинет";
            if (errors.Count == 0)
            {
                if (oldUser.Email != user.Email) _userService.ChangeEmail(oldUser, user.Email);
                if (oldUser.FirstName != user.FirstName) _userService.ChangeFirstName(oldUser, user.FirstName);
                if (oldUser.LastName != user.LastName) _userService.ChangeLastName(oldUser, user.LastName);
                if (oldUser.Password != user.Password) _userService.ChangePassword(oldUser, user.Password);
                _authService.Login(user.Email, user.Password, true);
                ViewBag.User = user;
                return View();
            }

            ViewBag.Errors = errors;
            ViewBag.User = CurrentUser;
            return View();
        }
    }
}
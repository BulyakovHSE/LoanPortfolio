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

        public HomeController(IUserService userService, IAuthService authService) : base(authService)
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
            ViewBag.User = CurrentUser;

            return View("Index");
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
                ViewBag.User = CurrentUser;
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
                ViewBag.User = CurrentUser;
                return View("Index");
            }

            ViewBag.Title = "Регистрация";
            ViewBag.User = user;
            return View();
        }
    }
}
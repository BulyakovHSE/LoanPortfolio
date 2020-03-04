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
        public HomeController(IUserService userService, IAuthService authService) : base(authService)
        {
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
            ViewBag.User = CurrentUser;

            return View();
        }

        [HttpPost]
        public ActionResult Auth(string login, string password)
        {
            ViewBag.Title = "Кредитный портфель";
            _authService.Login(login, password, true);
            ViewBag.User = CurrentUser;
            return View("Index");
        }

        public ActionResult Register()
        {
            ViewBag.Title = "Регистрация";
            ViewBag.User = CurrentUser;

            return View();
        }

        [HttpPost]
        public ActionResult Register(string firstName, string lastName, string login, string password)
        {
            ViewBag.Title = "Кредитный портфель";
            ViewBag.User = CurrentUser;

            return View("Index");
        }
    }
}
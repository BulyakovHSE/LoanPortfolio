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
    public class HomeController : Controller
    {
        private User _user;

        public HomeController(IUserService userService, IAuthService authService)
        {
            //if (authService.IsLoggedIn())
            //{
            //    _user = userService.GetAll().ToList()[0];
            //}
            if (userService.GetAll().Any())
            {
                _user = userService.GetAll().ToList()[0];
            }
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Кредитный портфель";
            ViewBag.User = _user;

            return View();
        }

        public ActionResult Close()
        {
            ViewBag.Title = "Кредитный портфель";
            ViewBag.User = _user;

            return View("Index");
        }

        public ActionResult Auth()
        {
            ViewBag.Title = "Авторизация";
            ViewBag.User = _user;

            return View();
        }

        [HttpPost]
        public ActionResult Auth(string firstName, string lastName, string login, string password)
        {
            ViewBag.Title = "Кредитный портфель";
            ViewBag.User = _user;

            return View("Index");
        }

        public ActionResult Register()
        {
            ViewBag.Title = "Регистрация";
            ViewBag.User = _user;

            return View();
        }

        [HttpPost]
        public ActionResult Register(string login, string password)
        {
            ViewBag.Title = "Кредитный портфель";
            ViewBag.User = _user;

            return View("Index");
        }
    }
}
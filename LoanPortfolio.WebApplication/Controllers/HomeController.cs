using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private User _user;
        private IExpenseService _expenseService;
        private IIncomeService _incomeService;

        public HomeController()
        {
            var userService = MvcApplication._container.GetInstance<IUserService>();
            var users = userService.GetAll().ToList();
            _user = users[0];
            _expenseService = MvcApplication._container.GetInstance<IExpenseService>();
            _incomeService = MvcApplication._container.GetInstance<IIncomeService>();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Кредитный портфель";

            return View();
        }
    }
}
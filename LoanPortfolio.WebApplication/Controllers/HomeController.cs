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

        public HomeController(IUserService userService, IExpenseService expenseService, IIncomeService incomeService)
        {
            if (userService.GetAll().Any())
            {
                _user = userService.GetAll().ToList()[0];
            }
            
            _expenseService = expenseService;
            _incomeService = incomeService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Кредитный портфель";

            return View();
        }
    }
}
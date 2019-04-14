using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.WebApplication.Controllers
{
    public class ResultController : Controller
    {
        private User _user;
        private IExpenseService _expenseService;
        private IIncomeService _incomeService;

        public ResultController()
        {
            var userService = MvcApplication._container.GetInstance<IUserService>();
            var users = userService.GetAll().ToList();
            _user = users[0];
            _expenseService = MvcApplication._container.GetInstance<IExpenseService>();
            _incomeService = MvcApplication._container.GetInstance<IIncomeService>();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Оборотно-сальдовая ведомость";

            var incomes = _incomeService.GetAll(_user).ToList();
            float sum = 0;
            foreach (Income income in incomes)
            {
                if (income is RegularIncome r)
                {
                    sum += r.Sum;
                }
                else if(income is PeriodicIncome p)
                {
                    sum += p.Sum;
                }
            }

            ViewBag.Income = sum.ToString();
            var balance = sum;
            var expenses = _expenseService.GetAll(_user).ToList();
            sum = 0;
            foreach (Expense expense in expenses)
            {
                sum += expense.Sum;
            }

            balance -= sum;
            ViewBag.Expense = sum.ToString();
            ViewBag.Balance = balance.ToString();
            return View();
        }
    }
}
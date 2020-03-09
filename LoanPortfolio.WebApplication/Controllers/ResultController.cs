using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;
using LoanPortfolio.WebApplication.Security;

namespace LoanPortfolio.WebApplication.Controllers
{
    public class ResultController : BaseController
    {
        private User _user;
        private IExpenseService _expenseService;
        private IIncomeService _incomeService;

        public ResultController(IUserService userService, IExpenseService expenseService, IIncomeService incomeService, IAuthService authService) : base(authService)
        {
            ViewBag.User = CurrentUser;
            _user = CurrentUser;
            _expenseService = expenseService;
            _incomeService = incomeService;
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
                else if (income is PeriodicIncome p)
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
                if (expense is HCSExpense hcsExpense)
                {
                    sum += hcsExpense.Sum;
                }
                else if (expense is PersonalExpense personalExpense)
                {
                    sum += personalExpense.Sum;
                }
                else if (expense is LoanPayment loanPayment)
                {
                    sum += loanPayment.Loan.AmountDie / loanPayment.Loan.RepaymentPeriod;
                }
            }

            balance -= sum;
            ViewBag.Expense = sum.ToString();
            ViewBag.Balance = balance.ToString();
            return View();
        }
    }
}
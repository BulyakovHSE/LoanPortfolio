using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.WebApplication.Controllers
{
    public class CreditHistoryController : Controller
    {
        private User _user;
        private IExpenseService _expenseService;

        public CreditHistoryController(IUserService userService, IExpenseService expenseService)
        {
            if (userService.GetAll().Any())
            {
                _user = userService.GetAll().ToList()[0];
            }

            _expenseService = expenseService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Кредитная история";
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(Loan));
            return View();
        }


        #region Add
        public ActionResult AddCredit()
        {
            ViewBag.Title = "Новый кредит";

            return View();
        }

        [HttpPost]
        public ActionResult AddCredit(string Date, string LoanSum, string AmountDie, string RepaymentPeriod, string CreditInstitutionName, string BankAddress)
        {
            if (float.TryParse(LoanSum, out var loanSum))
            {
                if (float.TryParse(AmountDie, out var amountDie))
                {
                    if (int.TryParse(RepaymentPeriod, out var repaymentPeriod))
                    {
                        if (DateTime.TryParse(Date, out var date))
                        {
                            _expenseService.AddLoan(_user, date, loanSum, amountDie, repaymentPeriod, CreditInstitutionName, BankAddress);

                            ViewBag.Title = "Кредитная история";
                            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(Loan));

                            return View("Index");
                        }
                        else
                        {
                            ViewBag.Error = "Введите даты в формате ДД.ММ.ГГГГ";
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Введите челое значение месяцев";
                    }
                }
                else
                {
                    ViewBag.Error = "Введите сумму погашения";
                }
            }
            else
            {
                ViewBag.Error = "Введите сумму кредита";
            }

            ViewBag.Title = "Новый кредит";

            return View();
        }
        #endregion

        #region Change
        [HttpGet]
        public ActionResult ChangeCredit(int id)
        {
            var loan = _expenseService.GetById(id);
            ViewBag.Title = "Кредит";

            ViewBag.Loan = loan;

            return View();
        }

        [HttpPost]
        public ActionResult ChangeCredit(int expenseid, string Date, string LoanSum, string AmountDie, string RepaymentPeriod, string CreditInstitutionName, string BankAddress)
        {
            if (float.TryParse(LoanSum, out var loanSum))
            {
                if (float.TryParse(AmountDie, out var amountDie))
                {
                    if (int.TryParse(RepaymentPeriod, out var repaymentPeriod))
                    {
                        if (DateTime.TryParse(Date, out var date))
                        {
                            var expense = (Loan)_expenseService.GetById(expenseid);

                            expense.DatePayment = date;
                            expense.LoanSum = loanSum;
                            expense.AmountDie = amountDie;
                            expense.RepaymentPeriod = repaymentPeriod;
                            expense.CreditInstitutionName = CreditInstitutionName;
                            expense.BankAddress = BankAddress;

                            _expenseService.UpdateExpense(expense);

                            ViewBag.Title = "Кредитная история";
                            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(Loan));

                            return View("Index");
                        }
                        else
                        {
                            ViewBag.Error = "Введите даты в формате ДД.ММ.ГГГГ";
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Введите челое значение месяцев";
                    }
                }
                else
                {
                    ViewBag.Error = "Введите сумму погашения";
                }
            }
            else
            {
                ViewBag.Error = "Введите сумму кредита";
            }

            var loan = _expenseService.GetById(expenseid);
            ViewBag.Title = "Кредит";

            ViewBag.Loan = loan;

            return View();
        }
        #endregion
    }
}
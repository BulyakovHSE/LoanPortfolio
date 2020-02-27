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
        private ILoanService _loanService;

        public CreditHistoryController(IUserService userService, ILoanService loanService)
        {
            if (userService.GetAll().Any())
            {
                _user = userService.GetAll().ToList()[0];
            }

            _loanService = loanService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Кредитная история";
            ViewBag.Loan = _loanService.GetAll(_user);
            return View();
        }


        #region Add
        public ActionResult AddCredit()
        {
            ViewBag.Title = "Новый кредит";

            return View();
        }

        [HttpPost]
        public ActionResult AddCredit(DateTime date, string loanSum, string amountDie, string repaymentPeriod, string creditInstitutionName, string bankAddress)
        {
            float loanSumValue, amountDieValue;
            int repaymentPeriodValue;

            if (!float.TryParse(loanSum, out loanSumValue))
            {
                ViewBag.Error = "Введите сумму кредита";
                ViewBag.Title = "Новый кредит";
                return View();
            }
            else
            {
                if (loanSumValue <= 0)
                {
                    ViewBag.Error = "Cумма кредита должна быть больше 0";
                    ViewBag.Title = "Новый кредит";
                    return View();
                }
            }

            if (!float.TryParse(amountDie, out amountDieValue))
            {
                ViewBag.Error = "Введите сумму погашения";
                ViewBag.Title = "Новый кредит";
                return View();
            }
            else
            {
                if (amountDieValue <= 0)
                {
                    ViewBag.Error = "Cумма погашения должна быть больше 0";
                    ViewBag.Title = "Новый кредит";
                    return View();
                }
            }

            if (!int.TryParse(repaymentPeriod, out repaymentPeriodValue))
            {
                ViewBag.Error = "Введите целое значение месяцев";
                ViewBag.Title = "Новый кредит";
                return View();
            }
            else
            {
                if (repaymentPeriodValue <= 0)
                {
                    ViewBag.Error = "Количество месяцев должно быть больше 0";
                    ViewBag.Title = "Новый кредит";
                    return View();
                }
            }

            if (date < DateTime.MinValue || date > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату";
                ViewBag.Title = "Новый кредит";
                return View();
            }

            _loanService.AddLoan(_user, loanSumValue, date, amountDieValue, repaymentPeriodValue, creditInstitutionName, bankAddress);

            ViewBag.Title = "Кредитная история";
            ViewBag.Loan = _loanService.GetAll(_user);

            return View("Index");
        }
        #endregion

        #region Change
        [HttpGet]
        public ActionResult ChangeCredit(int id)
        {
            var loan = _loanService.GetById(id);
            ViewBag.Title = "Кредит";

            ViewBag.Loan = loan;

            return View();
        }

        [HttpPost]
        public ActionResult ChangeCredit(int expenseid, DateTime date, string loanSum, string amountDie, string repaymentPeriod, string creditInstitutionName, string bankAddress)
        {
            var credit = _loanService.GetById(expenseid);
            float loanSumValue, amountDieValue;
            int repaymentPeriodValue;

            if (!float.TryParse(loanSum, out loanSumValue))
            {
                ViewBag.Error = "Введите сумму кредита";
                ViewBag.Title = "Кредит";
                ViewBag.Loan = credit;
                return View();
            }
            else
            {
                if (loanSumValue <= 0)
                {
                    ViewBag.Error = "Cумма кредита должна быть больше 0";
                    ViewBag.Title = "Кредит";
                    ViewBag.Loan = credit;
                    return View();
                }
            }

            if (!float.TryParse(amountDie, out amountDieValue))
            {
                ViewBag.Error = "Введите сумму погашения";
                ViewBag.Title = "Кредит";
                ViewBag.Loan = credit;
                return View();
            }
            else
            {
                if (amountDieValue <= 0)
                {
                    ViewBag.Error = "Cумма погашения должна быть больше 0";
                    ViewBag.Title = "Кредит";
                    ViewBag.Loan = credit;
                    return View();
                }
            }

            if (!int.TryParse(repaymentPeriod, out repaymentPeriodValue))
            {
                ViewBag.Error = "Введите целое значение месяцев";
                ViewBag.Title = "Кредит";
                ViewBag.Loan = credit;
                return View();
            }
            else
            {
                if (repaymentPeriodValue <= 0)
                {
                    ViewBag.Error = "Количество месяцев должно быть больше 0";
                    ViewBag.Title = "Кредит";
                    ViewBag.Loan = credit;
                    return View();
                }
            }

            if (date < DateTime.MinValue || date > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату";
                ViewBag.Title = "Кредит";
                ViewBag.Loan = credit;
                return View();
            }

            credit.ClearanceDate = date;
            credit.LoanSum = loanSumValue;
            credit.AmountDie = amountDieValue;
            credit.RepaymentPeriod = repaymentPeriodValue;
            credit.CreditInstitutionName = creditInstitutionName;
            credit.BankAddress = bankAddress;

            _loanService.UpdateLoan(credit);

            ViewBag.Title = "Кредитная история";
            ViewBag.Loan = _loanService.GetAll(_user);

            return View("Index");
        }
        #endregion
    }
}
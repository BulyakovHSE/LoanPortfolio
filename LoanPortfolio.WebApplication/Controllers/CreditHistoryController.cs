﻿using System;
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
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));
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
            if (!float.TryParse(amountDie, out amountDieValue))
            {
                ViewBag.Error = "Введите сумму погашения";
                ViewBag.Title = "Новый кредит";
                return View();
            }
            if (!int.TryParse(repaymentPeriod, out repaymentPeriodValue))
            {
                ViewBag.Error = "Введите целое значение месяцев";
                ViewBag.Title = "Новый кредит";
                return View();
            }
            if (date < DateTime.MinValue || date > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату";
                ViewBag.Title = "Новый кредит";
                return View();
            }
            _expenseService.AddLoan(_user, date, loanSumValue, amountDieValue, repaymentPeriodValue, creditInstitutionName, bankAddress);

            ViewBag.Title = "Кредитная история";
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));

            return View("Index");
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
        public ActionResult ChangeCredit(int expenseid, DateTime date, string loanSum, string amountDie, string repaymentPeriod, string creditInstitutionName, string bankAddress)
        {
            var credit = (LoanPayment)_expenseService.GetById(expenseid);
            float loanSumValue, amountDieValue;
            int repaymentPeriodValue;

            if (!float.TryParse(loanSum, out loanSumValue))
            {
                ViewBag.Error = "Введите сумму кредита";
                ViewBag.Title = "Кредит";
                ViewBag.Loan = credit;
                return View();
            }
            if (!float.TryParse(amountDie, out amountDieValue))
            {
                ViewBag.Error = "Введите сумму погашения";
                ViewBag.Title = "Кредит";
                ViewBag.Loan = credit;
                return View();
            }
            if (!int.TryParse(repaymentPeriod, out repaymentPeriodValue))
            {
                ViewBag.Error = "Введите целое значение месяцев";
                ViewBag.Title = "Кредит";
                ViewBag.Loan = credit;
                return View();
            }
            if (date < DateTime.MinValue || date > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату";
                ViewBag.Title = "Кредит";
                ViewBag.Loan = credit;
                return View();
            }


            credit.DatePayment = date;
            credit.LoanSum = loanSumValue;
            credit.AmountDie = amountDieValue;
            credit.RepaymentPeriod = repaymentPeriodValue;
            credit.CreditInstitutionName = creditInstitutionName;
            credit.BankAddress = bankAddress;

            _expenseService.UpdateExpense(credit);

            ViewBag.Title = "Кредитная история";
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));

            return View("Index");
        }
        #endregion
    }
}
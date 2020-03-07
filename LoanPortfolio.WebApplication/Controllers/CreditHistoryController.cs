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
    public class CreditHistoryController : BaseController
    {
        private User _user;
        private ILoanService _loanService;

        public CreditHistoryController(IUserService userService, ILoanService loanService, IAuthService authService) : base(authService)
        {
            _user = CurrentUser;
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
            Loan loan = new Loan();
            loan.ClearanceDate = DateTime.Now;
            ViewBag.Loan = loan;
            return View();
        }

        [HttpPost]
        public ActionResult AddCredit(DateTime date, string loanSum, string amountDie, string repaymentPeriod, string creditInstitutionName, string bankAddress)
        {
            (List<string> errors, Loan loan) = Loans.CheckLoan(date, loanSum, amountDie, repaymentPeriod);

            if (errors.Count == 0)
            {
                _loanService.AddLoan(_user, loan.LoanSum, loan.ClearanceDate, loan.AmountDie, loan.RepaymentPeriod, creditInstitutionName, bankAddress);

                ViewBag.Title = "Кредитная история";
                ViewBag.Loan = _loanService.GetAll(_user);

                return View("Index");
            }

            ViewBag.Errors = errors;
            ViewBag.Loan = loan;
            ViewBag.Title = "Новый кредит";
            return View();
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
            (List<string> errors, Loan loan) = Loans.CheckLoan(date, loanSum, amountDie, repaymentPeriod);

            if (errors.Count == 0)
            {
                loan.CreditInstitutionName = creditInstitutionName;
                loan.BankAddress = bankAddress;
                _loanService.UpdateLoan(loan);

                ViewBag.Title = "Кредитная история";
                ViewBag.Loan = _loanService.GetAll(_user);

                return View("Index");
            }

            var credit = _loanService.GetById(expenseid);
            ViewBag.Errors = errors;
            ViewBag.Loan = credit;
            ViewBag.Title = "Кредит";
            return View();
        }
        #endregion
    }
}
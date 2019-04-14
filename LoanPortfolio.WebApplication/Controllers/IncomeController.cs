using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.WebApplication.Controllers
{
    public class IncomeController : Controller
    {
        private User _user;
        private IIncomeService _incomeService;

        public IncomeController()
        {
            var userService = MvcApplication._container.GetInstance<IUserService>();
            var users = userService.GetAll().ToList();
            _user = users[0];
            _incomeService = MvcApplication._container.GetInstance<IIncomeService>();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Доходы";

            ViewBag.IncomesRegular = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(RegularIncome));
            ViewBag.IncomesPeriod = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(PeriodicIncome));

            return View();
        }

        #region Add

        public ActionResult AddRegular()
        {
            ViewBag.Title = "Новый доход";

            return View();
        }

        [HttpPost]
        public RedirectResult AddRegular(string Source, string PrepaidExpanse, string DatePrepaidExpanse, string Salary, string DateSalary)
        {
            if (!string.IsNullOrWhiteSpace(Source) && float.TryParse(PrepaidExpanse, out var prepaidExpanse) && DateTime.TryParse(DatePrepaidExpanse, out var datePrepaidExpanse)
                                                                       && float.TryParse(Salary, out var salary) && DateTime.TryParse(DateSalary, out var dateSalary))
            {
                _incomeService.AddRegularIncome(_user, Source, datePrepaidExpanse, prepaidExpanse, dateSalary, salary);

                return Redirect("~/Income/Index");
            }

            return Redirect("~/Income/AddRegular");
        }

        public ActionResult AddPeriod()
        {
            ViewBag.Title = "Новый доход";

            return View();
        }

        [HttpPost]
        public RedirectResult AddPeriod(string Source, string Sum)
        {
            if (!string.IsNullOrWhiteSpace(Source) && float.TryParse(Sum, out var value))
            {
                _incomeService.AddPeriodicIncome(_user, Source, value, DateTime.Now);

                return Redirect("~/Income/Index");
            }

            return Redirect("~/Income/AddPeriod");
        }

        #endregion

        #region Change

        [HttpGet]
        public ActionResult ChangeRegular(int id)
        {
            var income = _incomeService.GetById(id);
            ViewBag.Title = income.IncomeSource;

            ViewBag.Income = income;

            return View();
        }

        [HttpPost]
        public RedirectResult ChangeRegular(int incomeid, string Source, string PrepaidExpanse, string DatePrepaidExpanse, string Salary, string DateSalary)
        {
            if (!string.IsNullOrWhiteSpace(Source) && float.TryParse(PrepaidExpanse, out var prepaidExpanse) && DateTime.TryParse(DatePrepaidExpanse, out var datePrepaidExpanse)
                                                                       && float.TryParse(Salary, out var salary) && DateTime.TryParse(DateSalary, out var dateSalary))
            {
                var income = (RegularIncome)_incomeService.GetById(incomeid);

                _incomeService.ChangeIncomeSource(income, Source);
                _incomeService.ChangePrepaidExpanse(income, prepaidExpanse);
                _incomeService.ChangeDatePrepaidExpanse(income, datePrepaidExpanse);
                _incomeService.ChangeSalary(income, salary);
                _incomeService.ChangeDateSalary(income, dateSalary);

                return Redirect("~/Income/Index");
            }

            return Redirect("~/Income/ChangeRegular/" + incomeid);
        }

        [HttpGet]
        public ActionResult ChangePeriod(int id)
        {
            var income = _incomeService.GetById(id);
            ViewBag.Title = income.IncomeSource;

            ViewBag.Income = income;

            return View();
        }

        [HttpPost]
        public RedirectResult ChangePeriod(int incomeid, string Source, string Sum)
        {
            if (!string.IsNullOrWhiteSpace(Source) && float.TryParse(Sum, out var value))
            {
                var income = (PeriodicIncome)_incomeService.GetById(incomeid);

                _incomeService.ChangeIncomeSource(income, Source);
                _incomeService.ChangeSum(income, value);

                return Redirect("~/Income/Index");
            }

            return Redirect("~/Income/ChangePeriod/" + incomeid);
        }

        #endregion

        [HttpGet]
        public RedirectResult Delete(int id)
        {
            _incomeService.Remove(_incomeService.GetById(id));

            return Redirect("~/Income/Index");
        }
    }
}
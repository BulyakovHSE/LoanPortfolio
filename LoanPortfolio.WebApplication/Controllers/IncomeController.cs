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
    public class IncomeController : BaseController
    {
        private User _user;
        private IIncomeService _incomeService;

        public IncomeController(IUserService userService, IIncomeService incomeService, IAuthService authService) : base(authService)
        {
            _user = CurrentUser;
            _incomeService = incomeService;

            ViewBag.User = CurrentUser;
        }

        private List<RegularIncome> GetRegularIncomes(DateTime time)
        {
            int mm = time.Month;
            int yy = time.Year;

            var regular = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(RegularIncome));
            List<RegularIncome> regularIncomes = new List<RegularIncome>();
            foreach (RegularIncome regularIncome in regular)
            {
                if ((regularIncome.DatePrepaidExpanse.Month == mm && regularIncome.DatePrepaidExpanse.Year == yy) ||
                    (regularIncome.DateSalary.Month == mm && regularIncome.DateSalary.Year == yy))
                {
                    regularIncomes.Add(regularIncome);
                }
            }
            return regularIncomes;
        }

        private List<PeriodicIncome> GetPeriodicIncomes(DateTime time)
        {
            int mm = time.Month;
            int yy = time.Year;

            var periodics = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(PeriodicIncome));
            List<PeriodicIncome> periodicIncomes = new List<PeriodicIncome>();
            foreach (PeriodicIncome periodicIncome in periodics)
            {
                if (periodicIncome.DateIncome.Month == mm && periodicIncome.DateIncome.Year == yy)
                {
                    periodicIncomes.Add(periodicIncome);
                }
            }

            return periodicIncomes;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Доходы";
            ViewBag.IncomesRegular = GetRegularIncomes(DateTime.Now);
            ViewBag.IncomesPeriod = GetPeriodicIncomes(DateTime.Now);
            ViewBag.Time = DateTime.Now;

            return View();
        }

        [HttpPost]
        public ActionResult Index(DateTime time)
        {
            ViewBag.Title = "Доходы";
            ViewBag.IncomesRegular = GetRegularIncomes(time);
            ViewBag.IncomesPeriod = GetPeriodicIncomes(time);
            ViewBag.Time = time;

            return View("Index");
        }

        #region Add

        public ActionResult AddRegular()
        {
            ViewBag.Title = "Новый доход";

            RegularIncome regularIncome = new RegularIncome();
            regularIncome.DatePrepaidExpanse = DateTime.Now;
            regularIncome.DateSalary = DateTime.Now;
            ViewBag.Income = regularIncome;

            return View();
        }

        [HttpPost]
        public ActionResult AddRegular(string source, string prepaidExpanse, DateTime datePrepaidExpanse, string salary, DateTime dateSalary)
        {
            (List<string> errors, RegularIncome regularIncome) = Incomes.CheckRegularIncome(source, prepaidExpanse, datePrepaidExpanse, salary, dateSalary);

            if (errors.Count == 0)
            {
                _incomeService.AddRegularIncome(_user, regularIncome.IncomeSource, regularIncome.DatePrepaidExpanse,
                    regularIncome.PrepaidExpanse, regularIncome.DateSalary, regularIncome.Salary);

                ViewBag.Title = "Доходы";

                ViewBag.IncomesRegular = GetRegularIncomes(DateTime.Now);
                ViewBag.IncomesPeriod = GetPeriodicIncomes(DateTime.Now);
                ViewBag.Time = DateTime.Now;

                return View("Index");
            }

            ViewBag.Errors = errors;
            ViewBag.Income = regularIncome;
            ViewBag.Title = "Новый доход";

            return View();
        }

        public ActionResult AddPeriod()
        {
            ViewBag.Title = "Новый доход";
            PeriodicIncome periodicIncome = new PeriodicIncome();
            periodicIncome.DateIncome = DateTime.Now;
            ViewBag.Income = periodicIncome;

            return View();
        }

        [HttpPost]
        public ActionResult AddPeriod(string source, string sum, DateTime date)
        {
            (List<string> errors, PeriodicIncome periodicIncome) = Incomes.CheckPeriodIncome(source, sum, date);

            if (errors.Count == 0)
            {
                _incomeService.AddPeriodicIncome(_user, periodicIncome.IncomeSource, periodicIncome.Sum, periodicIncome.DateIncome);

                ViewBag.Title = "Доходы";

                ViewBag.IncomesRegular = GetRegularIncomes(DateTime.Now);
                ViewBag.IncomesPeriod = GetPeriodicIncomes(DateTime.Now);
                ViewBag.Time = DateTime.Now;

                return View("Index");
            }

            ViewBag.Errors = errors;
            ViewBag.Income = periodicIncome;
            ViewBag.Title = "Новый доход";

            return View();
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
        public ActionResult ChangeRegular(int incomeid, string source, string prepaidExpanse, DateTime datePrepaidExpanse, string salary, DateTime dateSalary)
        {
            (List<string> errors, RegularIncome regularIncome) = Incomes.CheckRegularIncome(source, prepaidExpanse, datePrepaidExpanse, salary, dateSalary);

            if (errors.Count == 0)
            {
                _incomeService.UpdateIncome(regularIncome);

                ViewBag.Title = "Доходы";

                ViewBag.IncomesRegular = GetRegularIncomes(DateTime.Now);
                ViewBag.IncomesPeriod = GetPeriodicIncomes(DateTime.Now);
                ViewBag.Time = DateTime.Now;

                return View("Index");
            }

            var income = (RegularIncome)_incomeService.GetById(incomeid);
            ViewBag.Errors = errors;
            ViewBag.Income = income;
            ViewBag.Title = income.IncomeSource;

            return View();
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
        public ActionResult ChangePeriod(int incomeid, string source, string sum, DateTime date)
        {
            (List<string> errors, PeriodicIncome periodicIncome) = Incomes.CheckPeriodIncome(source, sum, date);

            if (errors.Count == 0)
            {
                _incomeService.UpdateIncome(periodicIncome);

                ViewBag.Title = "Доходы";

                ViewBag.IncomesRegular = GetRegularIncomes(DateTime.Now);
                ViewBag.IncomesPeriod = GetPeriodicIncomes(DateTime.Now);
                ViewBag.Time = DateTime.Now;

                return View("Index");
            }

            var income = (PeriodicIncome)_incomeService.GetById(incomeid);
            ViewBag.Errors = errors;
            ViewBag.Title = income.IncomeSource;
            ViewBag.Income = income;

            return View();
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
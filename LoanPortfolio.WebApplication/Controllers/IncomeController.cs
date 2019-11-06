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

        public IncomeController(IUserService userService, IIncomeService incomeService)
        {
            if (userService.GetAll().Any())
            {
                _user = userService.GetAll().ToList()[0];
            }

            _incomeService = incomeService;
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
        public ActionResult AddRegular(string source, string prepaidExpanse, DateTime datePrepaidExpanse, string salary, DateTime dateSalary)
        {
            float valuePrepaidExpanse, valueSalary;

            if (string.IsNullOrWhiteSpace(source))
            {
                ViewBag.Error = "Введите название";
                return View("AddRegular");
            }

            if (!float.TryParse(prepaidExpanse, out valuePrepaidExpanse))
            {
                ViewBag.Error = "Введите сумму аванса";
                return View("AddRegular");
            }

            if (datePrepaidExpanse < DateTime.MinValue || datePrepaidExpanse > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату аванса";
                return View("AddRegular");
            }

            if (!float.TryParse(salary, out valueSalary))
            {
                ViewBag.Error = "Введите сумму окончаловки";
                return View("AddRegular");
            }

            if (dateSalary < DateTime.MinValue || dateSalary > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату окончаловки";
                return View("AddRegular");
            }

            _incomeService.AddRegularIncome(_user, source, datePrepaidExpanse, valuePrepaidExpanse, dateSalary, valueSalary);

            ViewBag.Title = "Доходы";

            ViewBag.IncomesRegular = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(RegularIncome));
            ViewBag.IncomesPeriod = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(PeriodicIncome));

            return View("Index");
        }

        public ActionResult AddPeriod()
        {
            ViewBag.Title = "Новый доход";

            return View();
        }

        [HttpPost]
        public ActionResult AddPeriod(string Source, string Sum)
        {
            if (!string.IsNullOrWhiteSpace(Source))
            {
                if (float.TryParse(Sum, out var value))
                {
                    _incomeService.AddPeriodicIncome(_user, Source, value, DateTime.Now);

                    ViewBag.Title = "Доходы";

                    ViewBag.IncomesRegular = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(RegularIncome));
                    ViewBag.IncomesPeriod = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(PeriodicIncome));

                    return View("Index");
                }
                else
                {
                    ViewBag.Error = "Введите сумму";
                }
            }
            else
            {
                ViewBag.Error = "Введите название";
            }

            ViewBag.Title = "Новый доход";

            return View("AddPeriod");
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
        public ActionResult ChangeRegular(int incomeid, string Source, string PrepaidExpanse, string DatePrepaidExpanse, string Salary, string DateSalary)
        {
            if (!string.IsNullOrWhiteSpace(Source))
            {
                if (float.TryParse(PrepaidExpanse, out var prepaidExpanse))
                {
                    if (DateTime.TryParse(DatePrepaidExpanse, out var datePrepaidExpanse))
                    {
                        if (float.TryParse(Salary, out var salary))
                        {
                            if (DateTime.TryParse(DateSalary, out var dateSalary))
                            {
                                var income = (RegularIncome)_incomeService.GetById(incomeid);
                                income.IncomeSource = Source;
                                income.PrepaidExpanse = prepaidExpanse;
                                income.DatePrepaidExpanse = datePrepaidExpanse;
                                income.Salary = salary;
                                income.DateSalary = dateSalary;

                                _incomeService.UpdateIncome(income);

                                ViewBag.Title = "Доходы";

                                ViewBag.IncomesRegular = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(RegularIncome));
                                ViewBag.IncomesPeriod = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(PeriodicIncome));

                                return View("Index");
                            }
                            else
                            {
                                ViewBag.Error = "Введите дату окончаловки в формате ДД.ММ.ГГГГ";
                            }
                        }
                        else
                        {
                            ViewBag.Error = "Введите сумму окончаловки";
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Введите дату аванса в формате ДД.ММ.ГГГГ";
                    }
                }
                else
                {
                    ViewBag.Error = "Введите сумму аванса";
                }
            }
            else
            {
                ViewBag.Error = "Введите название";
            }

            var income1 = _incomeService.GetById(incomeid);
            ViewBag.Title = income1.IncomeSource;

            ViewBag.Income = income1;

            return View("ChangeRegular");
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
        public ActionResult ChangePeriod(int incomeid, string Source, string Sum)
        {
            if (!string.IsNullOrWhiteSpace(Source))
            {
                if (float.TryParse(Sum, out var value))
                {
                    var income = (PeriodicIncome)_incomeService.GetById(incomeid);

                    income.IncomeSource = Source;
                    income.Sum = value;

                    _incomeService.UpdateIncome(income);

                    ViewBag.Title = "Доходы";

                    ViewBag.IncomesRegular = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(RegularIncome));
                    ViewBag.IncomesPeriod = _incomeService.GetAll(_user).Where(x => x.GetType() == typeof(PeriodicIncome));

                    return View("Index");
                }
                else
                {
                    ViewBag.Error = "Введите сумму";
                }
            }
            else
            {
                ViewBag.Error = "Введите название";
            }

            var income1 = _incomeService.GetById(incomeid);
            ViewBag.Title = income1.IncomeSource;

            ViewBag.Income = income1;

            return View("ChangePeriod");
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
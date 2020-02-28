﻿using System;
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

        public ActionResult ChangeTime(DateTime time)
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
            return View();
        }

        [HttpPost]
        public ActionResult AddRegular(string source, string prepaidExpanse, DateTime datePrepaidExpanse, string salary, DateTime dateSalary)
        {
            float prepaidExpanseValue, salaryValue;

            if (string.IsNullOrWhiteSpace(source))
            {
                ViewBag.Error = "Введите название";
                ViewBag.Title = "Новый доход";
                return View();
            }

            if (!float.TryParse(prepaidExpanse, out prepaidExpanseValue))
            {
                ViewBag.Error = "Введите сумму аванса";
                ViewBag.Title = "Новый доход";
                return View();
            }
            else
            {
                if (prepaidExpanseValue <= 0)
                {
                    ViewBag.Error = "Сумма аванса должна быть больше 0";
                    ViewBag.Title = "Новый доход";
                    return View();
                }
            }

            if (datePrepaidExpanse < DateTime.MinValue || datePrepaidExpanse > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату аванса";
                ViewBag.Title = "Новый доход";
                return View();
            }

            if (!float.TryParse(salary, out salaryValue))
            {
                ViewBag.Error = "Введите сумму окончаловки";
                ViewBag.Title = "Новый доход";
                return View();
            }
            else
            {
                if (salaryValue <= 0)
                {
                    ViewBag.Error = "Сумма окончаловки должна быть больше 0";
                    ViewBag.Title = "Новый доход";
                    return View();
                }
            }

            if (dateSalary < DateTime.MinValue || dateSalary > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату окончаловки";
                ViewBag.Title = "Новый доход";
                return View();
            }
            else
            {
                if (dateSalary <= datePrepaidExpanse)
                {
                    ViewBag.Error = "Дата окончаловки должна быть больше даты аванса";
                    ViewBag.Title = "Новый доход";
                    return View();
                }
            }

            _incomeService.AddRegularIncome(_user, source, datePrepaidExpanse, prepaidExpanseValue, dateSalary, salaryValue);

            ViewBag.Title = "Доходы";

            ViewBag.IncomesRegular = GetRegularIncomes(DateTime.Now);
            ViewBag.IncomesPeriod = GetPeriodicIncomes(DateTime.Now);
            ViewBag.Time = DateTime.Now;

            return View("Index");
        }

        public ActionResult AddPeriod()
        {
            ViewBag.Title = "Новый доход";

            return View();
        }

        [HttpPost]
        public ActionResult AddPeriod(string source, string sum)
        {
            float value;

            if (string.IsNullOrWhiteSpace(source))
            {
                ViewBag.Error = "Введите название";
                ViewBag.Title = "Новый доход";
                return View();
            }

            if (!float.TryParse(sum, out value))
            {
                ViewBag.Error = "Введите сумму";
                ViewBag.Title = "Новый доход";
                return View();
            }
            else
            {
                if (value <= 0)
                {
                    ViewBag.Error = "Сумма должна быть больше 0";
                    ViewBag.Title = "Новый доход";
                    return View();
                }
            }

            _incomeService.AddPeriodicIncome(_user, source, value, DateTime.Now);

            ViewBag.Title = "Доходы";

            ViewBag.IncomesRegular = GetRegularIncomes(DateTime.Now);
            ViewBag.IncomesPeriod = GetPeriodicIncomes(DateTime.Now);
            ViewBag.Time = DateTime.Now;

            return View("Index");
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
            float prepaidExpanseValue, salaryValue;
            var income = (RegularIncome)_incomeService.GetById(incomeid);

            if (string.IsNullOrWhiteSpace(source))
            {
                ViewBag.Error = "Введите название";
                ViewBag.Title = income.IncomeSource;
                ViewBag.Income = income;

                return View();
            }

            if (!float.TryParse(prepaidExpanse, out prepaidExpanseValue))
            {
                ViewBag.Error = "Введите сумму аванса";
                ViewBag.Title = income.IncomeSource;
                ViewBag.Income = income;

                return View();
            }
            else
            {
                if (prepaidExpanseValue <= 0)
                {
                    ViewBag.Error = "Сумма аванса должна быть больше 0";
                    ViewBag.Title = income.IncomeSource;
                    ViewBag.Income = income;
                    return View();
                }
            }

            if (datePrepaidExpanse < DateTime.MinValue || datePrepaidExpanse > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату аванса";
                ViewBag.Title = income.IncomeSource;
                ViewBag.Income = income;

                return View();
            }

            if (!float.TryParse(salary, out salaryValue))
            {
                ViewBag.Error = "Введите сумму окончаловки";
                ViewBag.Title = income.IncomeSource;
                ViewBag.Income = income;

                return View();
            }
            else
            {
                if (salaryValue <= 0)
                {
                    ViewBag.Error = "Сумма окончаловки должна быть больше 0";
                    ViewBag.Title = income.IncomeSource;
                    ViewBag.Income = income;
                    return View();
                }
            }

            if (dateSalary < DateTime.MinValue || dateSalary > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату окончаловки";
                ViewBag.Title = income.IncomeSource;
                ViewBag.Income = income;

                return View();
            }
            else
            {
                if (dateSalary <= datePrepaidExpanse)
                {
                    ViewBag.Error = "Дата окончаловки должна быть больше даты аванса";
                    ViewBag.Title = income.IncomeSource;
                    ViewBag.Income = income;
                    return View();
                }
            }

            income.IncomeSource = source;
            income.PrepaidExpanse = prepaidExpanseValue;
            income.DatePrepaidExpanse = datePrepaidExpanse;
            income.Salary = salaryValue;
            income.DateSalary = dateSalary;

            _incomeService.UpdateIncome(income);

            ViewBag.Title = "Доходы";

            ViewBag.IncomesRegular = GetRegularIncomes(DateTime.Now);
            ViewBag.IncomesPeriod = GetPeriodicIncomes(DateTime.Now);
            ViewBag.Time = DateTime.Now;

            return View("Index");
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
        public ActionResult ChangePeriod(int incomeid, string source, string sum)
        {
            float value;
            var income = (PeriodicIncome)_incomeService.GetById(incomeid);

            if (string.IsNullOrWhiteSpace(source))
            {
                ViewBag.Error = "Введите название";
                ViewBag.Title = income.IncomeSource;
                ViewBag.Income = income;

                return View();
            }

            if (!float.TryParse(sum, out value))
            {
                ViewBag.Error = "Введите сумму";
                ViewBag.Title = income.IncomeSource;
                ViewBag.Income = income;

                return View();
            }
            else
            {
                if (value <= 0)
                {
                    ViewBag.Error = "Сумма должна быть больше 0";
                    ViewBag.Title = income.IncomeSource;
                    ViewBag.Income = income;
                    return View();
                }
            }

            income.IncomeSource = source;
            income.Sum = value;

            _incomeService.UpdateIncome(income);

            ViewBag.Title = "Доходы";

            ViewBag.IncomesRegular = GetRegularIncomes(DateTime.Now);
            ViewBag.IncomesPeriod = GetPeriodicIncomes(DateTime.Now);
            ViewBag.Time = DateTime.Now;

            return View("Index");
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
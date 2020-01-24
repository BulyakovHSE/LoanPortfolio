using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.WebApplication.Controllers
{
    public class ExpenseController : Controller
    {
        private User _user;
        private IExpenseService _expenseService;

        public ExpenseController(IUserService userService, IExpenseService expenseService)
        {
            if (userService.GetAll().Any())
            {
                _user = userService.GetAll().ToList()[0];
            }

            _expenseService = expenseService;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Расходы";

            ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
            ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));

            return View();
        }


        #region Add

        public ActionResult AddPersonal()
        {
            ViewBag.Title = "Новый расход";

            return View();
        }

        [HttpPost]
        public ActionResult AddPersonal(string category, string sum)
        {
            ExpenseCategory categoryEnum;
            float value;

            if (!Enum.TryParse(category, out categoryEnum))
            {
                ViewBag.Error = "Выберите категорию";
                ViewBag.Title = "Новый расход";
                return View();
            }

            if (!float.TryParse(sum, out value))
            {
                ViewBag.Error = "Введите сумму";
                ViewBag.Title = "Новый расход";
                return View();
            }
            _expenseService.AddPersonalExpense(_user, DateTime.Now, value, categoryEnum);

            ViewBag.Title = "Расходы";

            ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
            ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));

            return View("Index");
        }

        public ActionResult AddHCS()
        {
            ViewBag.Title = "Новый расход ЖКХ";

            return View();
        }

        [HttpPost]
        public ActionResult AddHCS(DateTime date, string sum, string comment)
        {
            float value;

            if (date < DateTime.MinValue || date > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату";
                ViewBag.Title = "ЖКХ";
                return View();
            }

            if (!float.TryParse(sum, out value))
            {
                ViewBag.Error = "Введите значение сумму";
                ViewBag.Title = "ЖКХ";
                return View();
            }

            _expenseService.AddHCSExpense(_user, date, value, comment);

            ViewBag.Title = "Расходы";

            ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
            ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));

            return View("Index");
        }
        #endregion

        #region Change

        [HttpGet]
        public ActionResult ChangeHCS(int id)
        {
            var expense = _expenseService.GetById(id);
            ViewBag.Title = "ЖКХ";

            ViewBag.Expense = expense;

            return View();
        }

        [HttpPost]
        public ActionResult ChangeHCS(int expenseid, DateTime date, string sum, string comment)
        {
            var expense = (HCSExpense)_expenseService.GetById(expenseid);
            float value;

            if (date < DateTime.MinValue || date > DateTime.MaxValue)
            {
                ViewBag.Error = "Введите дату";
                ViewBag.Title = "ЖКХ";
                ViewBag.Expense = expense;
                return View();
            }

            if (!float.TryParse(sum, out value))
            {
                ViewBag.Error = "Введите значение сумму";
                ViewBag.Title = "ЖКХ";
                ViewBag.Expense = expense;
                return View();
            }



            expense.Sum = value;
            expense.DatePayment = date;
            expense.Comment = comment;

            _expenseService.UpdateExpense(expense);

            ViewBag.Title = "Расходы";

            ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
            ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));

            return View("Index");
        }

        [HttpGet]
        public ActionResult ChangePersonal(int id)
        {
            var expense = (PersonalExpense)_expenseService.GetById(id);
            ViewBag.Title = expense.ExpenseCategory;

            ViewBag.Expense = expense;

            return View();
        }

        [HttpPost]
        public ActionResult ChangePersonal(int expenseid, string category, string sum)
        {
            var expense = (PersonalExpense)_expenseService.GetById(expenseid);
            ExpenseCategory categoryEnum;
            float value;

            if (!Enum.TryParse(category, out categoryEnum))
            {
                ViewBag.Error = "Выберите категорию";
                ViewBag.Title = expense.ExpenseCategory;
                ViewBag.Expense = expense;
                return View();
            }

            if (!float.TryParse(sum, out value))
            {
                ViewBag.Error = "Введите сумму";
                ViewBag.Title = expense.ExpenseCategory;
                ViewBag.Expense = expense;
                return View();
            }

            expense.ExpenseCategory = categoryEnum;
            expense.Sum = value;
            _expenseService.UpdateExpense(expense);

            ViewBag.Title = "Расходы";

            ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
            ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));

            return View("Index");
        }

        #endregion

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _expenseService.Remove(_expenseService.GetById(id));

            ViewBag.Title = "Расходы";

            ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
            ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));

            return View("Index");
        }
    }
}
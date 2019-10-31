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
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(Loan));

            return View();
        }


        #region Add

        public ActionResult AddPersonal()
        {
            ViewBag.Title = "Новый расход";

            return View();
        }

        [HttpPost]
        public ActionResult AddPersonal(string Category, string Sum)
        {
            ExpenseCategory category;
            if (Enum.TryParse(Category, out category))
            {
                if (float.TryParse(Sum, out var value))
                {
                    _expenseService.AddPersonalExpense(_user, DateTime.Now, value, category);

                    ViewBag.Title = "Расходы";

                    ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
                    ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
                    ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(Loan));

                    return View("Index");
                }
                else
                {
                    ViewBag.Error = "Введите сумму";
                }
            }
            else
            {
                ViewBag.Error = "Выберите категорию";
            }
            ViewBag.Title = "Новый расход";
            return View();
        }

        public ActionResult AddHCS()
        {
            ViewBag.Title = "Новый расход ЖКХ";

            return View();
        }

        [HttpPost]
        public ActionResult AddHCS(string Date, string Sum, string Comment)
        {
            if (DateTime.TryParse(Date, out var date))
            {
                if (float.TryParse(Sum, out var value))
                {
                    _expenseService.AddHCSExpense(_user,date,value,Comment);

                    ViewBag.Title = "Расходы";

                    ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
                    ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
                    ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(Loan));

                    return View("Index");
                }
                else
                {
                    ViewBag.Error = "Введите значение сумму";
                }
            }
            else
            {
                ViewBag.Error = "Введите дату в формате ДД.ММ.ГГГГ";
            }
            ViewBag.Title = "Новый расход";
            return View();
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
        public ActionResult ChangeHCS(int expenseid, string Date, string Sum, string Comment)
        {
            if (float.TryParse(Sum, out var sum))
            {
                if (DateTime.TryParse(Date, out var date))
                {
                    var expense = (HCSExpense) _expenseService.GetById(expenseid);

                    expense.Sum = sum;
                    expense.DatePayment = date;
                    expense.Comment = Comment;

                    _expenseService.UpdateExpense(expense);

                    ViewBag.Title = "Расходы";

                    ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
                    ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
                    ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(Loan));

                    return View("Index");
                }
                else
                {
                    ViewBag.Error = "Введите дату в формате ДД.ММ.ГГГГ";
                }
            }
            else
            {
                ViewBag.Error = "Необходимо значение суммы";
            }

            var expense1 = _expenseService.GetById(expenseid);
            ViewBag.Title = "ЖКХ";

            ViewBag.Expense = expense1;
            return View("ChangeHCS");
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
        public ActionResult ChangePersonal(int expenseid, string Category, string Sum)
        {
            ExpenseCategory category;
            if (Enum.TryParse(Category, out category))
            {
                if (float.TryParse(Sum, out var value))
                {
                    var expense = (PersonalExpense)_expenseService.GetById(expenseid);
                    expense.ExpenseCategory = category;
                    expense.Sum = value;
                    _expenseService.UpdateExpense(expense);

                    ViewBag.Title = "Расходы";

                    ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
                    ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
                    ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(Loan));

                    return View("Index");
                }
                else
                {
                    ViewBag.Error = "Введите значение сумму";
                }
            }
            else
            {
                ViewBag.Error = "Выберите категорию";
            }

            var expense1 = (PersonalExpense)_expenseService.GetById(expenseid);
            ViewBag.Title = expense1.ExpenseCategory;
            ViewBag.Expense = expense1;

            return View("ChangePersonal");
        }

        #endregion

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _expenseService.Remove(_expenseService.GetById(id));

            ViewBag.Title = "Расходы";

            ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
            ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
            ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(Loan));

            return View("Index");
        }
    }
}
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

            return View();
        }


        #region Add

        public ActionResult AddPersonal()
        {
            ViewBag.Title = "Новый расход";

            return View();
        }

        [HttpPost]
        public RedirectResult AddPersonal(string Category, string Sum)
        {
            ExpenseCategory category;
            if (!Enum.TryParse(Category, out category) && float.TryParse(Sum, out var value))
            {
                _expenseService.AddPersonalExpense(_user, DateTime.Now, value, category);

                return Redirect("~/Expense/Index");
            }
            return Redirect("~/Expense/AddPersonal");
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
        public RedirectResult ChangeHCS(int expenseid, string Date, string Sum, string Comment)
        {
            if (float.TryParse(Sum, out var sum) && DateTime.TryParse(Date, out var date))
            {
                var expense = (HCSExpense)_expenseService.GetById(expenseid);

                expense.Sum = sum;
                expense.DatePayment = date;
                expense.Comment = Comment;

                _expenseService.UpdateExpense(expense);

                return Redirect("~/Expense/Index");
            }

            return Redirect("~/Expense/ChangeHCS/" + expenseid);
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
        public RedirectResult ChangePersonal(int expenseid, string Category, string Sum)
        {
            ExpenseCategory category;
            if (!Enum.TryParse(Category, out category) && float.TryParse(Sum, out var value))
            {
                var expense = (PersonalExpense)_expenseService.GetById(expenseid);
                expense.ExpenseCategory = category;
                expense.Sum = value;
                _expenseService.UpdateExpense(expense);

                return Redirect("~/Expense/Index");
            }

            return Redirect("~/Expense/ChangePersonal/" + expenseid);
        }

        #endregion

        [HttpGet]
        public RedirectResult Delete(int id)
        {
            _expenseService.Remove(_expenseService.GetById(id));

            return Redirect("~/Expense/Index");
        }
    }
}
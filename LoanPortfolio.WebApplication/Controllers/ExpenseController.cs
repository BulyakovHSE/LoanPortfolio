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

        public ExpenseController()
        {
            var userService = MvcApplication._container.GetInstance<IUserService>();
            var users = userService.GetAll().ToList();
            _user = users[0];
            _expenseService = MvcApplication._container.GetInstance<IExpenseService>();
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
            if (!string.IsNullOrWhiteSpace(Category) && float.TryParse(Sum, out var value))
            {
                _expenseService.AddPersonalExpense(_user, DateTime.Now, value, Category);

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

                _expenseService.ChangeDatePayment(expense,date);
                _expenseService.ChangeSum(expense, sum);
                _expenseService.ChangeComment(expense,Comment);
                
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
            if (!string.IsNullOrWhiteSpace(Category) && float.TryParse(Sum, out var value))
            {
                var expense = (PersonalExpense)_expenseService.GetById(expenseid);

                _expenseService.ChangeExpanseCategory(expense, Category);
                _expenseService.ChangeSum(expense, value);

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
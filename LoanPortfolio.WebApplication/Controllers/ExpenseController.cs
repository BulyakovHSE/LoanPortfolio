using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Services.Interfaces;
using LoanPortfolio.WebApplication.Security;

namespace LoanPortfolio.WebApplication.Controllers
{
    public class ExpenseController : BaseController
    {
        private User _user;
        private IExpenseService _expenseService;
        private IRepository<Category> _category;
        private ILoanService _loanService;

        public ExpenseController(IUserService userService, IExpenseService expenseService, IRepository<Category> category, ILoanService loanService, IAuthService authService) : base(authService, userService)
        {
            ViewBag.User = CurrentUser;
            _user = CurrentUser;
            _expenseService = expenseService;
            _category = category;
            _loanService = loanService;
        }

        private List<PersonalExpense> GetPersonalExpense(DateTime time)
        {
            int mm = time.Month;
            int yy = time.Year;

            var personals = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
            List<PersonalExpense> personalExpenses = new List<PersonalExpense>();
            foreach (PersonalExpense personalExpense in personals)
            {
                if (personalExpense.DatePayment.Month == mm && personalExpense.DatePayment.Year == yy)
                {
                    personalExpenses.Add(personalExpense);
                }
            }
            return personalExpenses;
        }

        private List<HCSExpense> GetHCSExpense(DateTime time)
        {
            int mm = time.Month;
            int yy = time.Year;

            var hcs = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
            List<HCSExpense> hcsExpenses = new List<HCSExpense>();
            foreach (var expense in hcs)
            {
                var hcsExpense = (HCSExpense) expense;
                if (hcsExpense.DatePayment.Month == mm && hcsExpense.DatePayment.Year == yy)
                {
                    hcsExpenses.Add(hcsExpense);
                }
            }

            return hcsExpenses;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Расходы";

            ViewBag.HCS = GetHCSExpense(DateTime.Now);
            ViewBag.Personal = GetPersonalExpense(DateTime.Now);
            ViewBag.Loan = _loanService.GetAll(_user);
            ViewBag.Time = DateTime.Now;

            return View();
        }

        [HttpPost]
        public ActionResult Index(DateTime time)
        {
            ViewBag.Title = "Расходы";
            ViewBag.HCS = GetHCSExpense(time);
            ViewBag.Personal = GetPersonalExpense(time);
            ViewBag.Loan = _loanService.GetAll(_user);
            ViewBag.Time = time;

            return View("Index");
        }

        #region Add

        public ActionResult AddPersonal()
        {
            ViewBag.Title = "Новый расход";
            ViewBag.Categories = _user.Categories;
            PersonalExpense personalExpense = new PersonalExpense();
            personalExpense.DatePayment = DateTime.Now;
            ViewBag.Expense = personalExpense;
            return View();
        }

        [HttpPost]
        public ActionResult AddPersonal(string categoryId, string sum, DateTime date)
        {
            (List<string> errors, PersonalExpense personalExpense, int id) = Expenses.CheckPersonalExpense(categoryId, sum, date);

            if (errors.Count == 0)
            {
                Category category = _category.All().Where(x => x.Id == id).First();
                _expenseService.AddPersonalExpense(_user, personalExpense.DatePayment, personalExpense.Sum, category);

                ViewBag.Title = "Расходы";

                ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
                ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
                ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));
                ViewBag.Time = DateTime.Now;
                return View("Index");
            }

            ViewBag.Errors = errors;
            ViewBag.Title = "Новый расход";
            ViewBag.Categories = _user.Categories;
            return View();
        }

        public ActionResult AddHCS()
        {
            HCSExpense hcsExpense = new HCSExpense();
            hcsExpense.DatePayment = DateTime.Now;
            ViewBag.Expense = hcsExpense;
            ViewBag.Title = "Новый расход ЖКХ";

            return View();
        }

        [HttpPost]
        public ActionResult AddHCS(DateTime date, string sum, string comment)
        {
            (List<string> errors, HCSExpense hcsExpense) = Expenses.CheckHCSExpense(date, sum);

            if (errors.Count == 0)
            {
                _expenseService.AddHCSExpense(_user, hcsExpense.DatePayment, hcsExpense.Sum, comment);

                ViewBag.Title = "Расходы";

                ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
                ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
                ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));
                ViewBag.Time = DateTime.Now;
                return View("Index");
            }

            ViewBag.Errors = errors;
            ViewBag.Expense = hcsExpense;
            ViewBag.Title = "Новый расход ЖКХ";
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
        public ActionResult ChangeHCS(int expenseid, DateTime date, string sum, string comment)
        {
            (List<string> errors, HCSExpense hcsExpense) = Expenses.CheckHCSExpense(date, sum);

            if (errors.Count == 0)
            {
                hcsExpense.Comment = comment;

                _expenseService.UpdateExpense(hcsExpense);

                ViewBag.Title = "Расходы";

                ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
                ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
                ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));
                ViewBag.Time = DateTime.Now;
                return View("Index");
            }

            var expense = (HCSExpense)_expenseService.GetById(expenseid);
            ViewBag.Errors = errors;
            ViewBag.Expense = expense;
            ViewBag.Title = "ЖКХ";
            return View();
        }

        [HttpGet]
        public ActionResult ChangePersonal(int id)
        {
            var expense = (PersonalExpense)_expenseService.GetById(id);
            ViewBag.Title = expense.ExpenseCategory;
            ViewBag.Categories = _user.Categories;
            ViewBag.Expense = expense;

            return View();
        }

        [HttpPost]
        public ActionResult ChangePersonal(int expenseid, string categoryId, string sum, DateTime date)
        {
            (List<string> errors, PersonalExpense personalExpense, int id) = Expenses.CheckPersonalExpense(categoryId, sum, date);

            if (errors.Count == 0)
            {
                Category category = _user.Categories.Where(x => x.Id == id).First();
                personalExpense.ExpenseCategory = category;
                _expenseService.UpdateExpense(personalExpense);

                ViewBag.Title = "Расходы";

                ViewBag.HCS = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(HCSExpense));
                ViewBag.Personal = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(PersonalExpense));
                ViewBag.Loan = _expenseService.GetAll(_user).Where(x => x.GetType() == typeof(LoanPayment));
                ViewBag.Time = DateTime.Now;
                return View("Index");
            }

            PersonalExpense expense = (PersonalExpense)_expenseService.GetById(expenseid);
            ViewBag.Errors = errors;
            ViewBag.Title = expense.ExpenseCategory;
            ViewBag.Expense = expense;
            ViewBag.Categories = _user.Categories;
            return View();
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
            ViewBag.Time = DateTime.Now;
            return View("Index");
        }
    }
}
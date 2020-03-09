using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Services.Interfaces;
using LoanPortfolio.WebApplication.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanPortfolio.WebApplication.Controllers
{
    public class CategoriesController : BaseController
    {
        private User _user;
        private IRepository<Category> _category;

        public CategoriesController(IUserService userService, IRepository<Category> category, IAuthService authService) : base(authService)
        {
            ViewBag.User = CurrentUser;
            _user = CurrentUser;
            _category = category;
        }


        public ActionResult Index()
        {
            ViewBag.Title = "Категории";
            ViewBag.Categories = _user.Categories;
            return View();
        }

        public ActionResult AddCategories()
        {
            ViewBag.Title = "Новая категория";
            ViewBag.Category = new Category();
            return View();
        }

        [HttpPost]
        public ActionResult AddCategories(string name)
        {
            (List<string> errors, Category category) = Categories.CheckCategory(name);

            if (_user.Categories.Where(x => x.Name == category.Name).ToList().Count > 0)
            {
                errors.Add("Такая категория уже существует");
            }

            if (errors.Count == 0)
            {
                _category.Add(category);

                ViewBag.Title = "Категории";
                ViewBag.Categories = _user.Categories;
                return View("Index");
            }

            ViewBag.Title = "Новая категория";
            ViewBag.Errors = errors;
            return View();
        }

        [HttpGet]
        public ActionResult ChangeCategories(int id)
        {
            Category category = _user.Categories.Where(x => x.Id == id).First();
            ViewBag.Title = category.Name;
            ViewBag.Category = category;
            return View();
        }

        [HttpPost]
        public ActionResult ChangeCategories(int id, string name)
        {
            (List<string> errors, Category newCategory) = Categories.CheckCategory(name);
            Category category = _user.Categories.Where(x => x.Id == id).First();

            if (_user.Categories.Where(x => x.Name == newCategory.Name).ToList().Count > 0)
            {
                errors.Add("Такая категория уже существует");
            }

            if (errors.Count == 0)
            {
                category.Name = newCategory.Name;
                _category.Update(category);

                ViewBag.Title = "Категории";
                ViewBag.Categories = _user.Categories;
                return View("Index");
            }

            ViewBag.Title = category.Name;
            ViewBag.Errors = errors;
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _category.Remove(_user.Categories.Where(x => x.Id == id).First());

            ViewBag.Title = "Категории";
            ViewBag.Categories = _user.Categories;

            return View("Index");
        }
    }
}
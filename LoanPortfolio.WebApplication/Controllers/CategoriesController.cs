using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanPortfolio.WebApplication.Controllers
{
    public class CategoriesController : Controller
    {
        private IRepository<Category> _category;

        public CategoriesController(IRepository<Category> category)
        {
            _category = category;
        }


        public ActionResult Index(string name)
        {
            if (name == null)
            {
                ViewBag.Title = "Новая категория";
                return View();
            }

            if (name.Length <= 0)
            {
                ViewBag.Title = "Новая категория";
                ViewBag.Error = "Введите название";
                return View();
            }

            var categories = _category.All().Where(x => x.Name == name).ToList();
            if (categories.Count > 0)
            {
                ViewBag.Title = "Новая категория";
                ViewBag.Error = "Такая категория уже существует";
                return View();
            }
            Category category = new Category();
            category.Name = name;
            _category.Add(category);

            ViewBag.Title = "Новый расход";
            ViewBag.Category = _category.All().ToList();
            return View("~/Views/Expense/AddPersonal.cshtml");
        }
    }
}
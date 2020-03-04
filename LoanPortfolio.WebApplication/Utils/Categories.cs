using LoanPortfolio.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanPortfolio.WebApplication
{
    public class Categories
    {
        static private bool ok;

        //Добавление и изменение категорий расходов
        public static (List<string> errors, Category category) CheckCategory(string name)
        {
            Category category = new Category();
            List<string> errors = new List<string>();

            (ok, category.Name) = Utils.CheckTextIsNotNull(name);
            if (!ok) errors.Add("Введите название");

            return (errors, category);
        }
    }
}
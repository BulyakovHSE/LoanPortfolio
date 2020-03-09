using LoanPortfolio.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanPortfolio.WebApplication
{
    public class Expenses
    {
        static private bool ok;

        //Добавление и изменение персональных расходов
        public static (List<string> errors, PersonalExpense personalExpense, int id) CheckPersonalExpense(string categoryId, string sum, DateTime date)
        {
            PersonalExpense personalExpense = new PersonalExpense();
            List<string> errors = new List<string>();
            int id;

            if (!Int32.TryParse(categoryId, out id))
            {
                errors.Add("Выберите категорию");
            }

            float value;
            (ok, value) = Utils.CheckNumberIsNotNull(sum);
            if (!ok)
            {
                errors.Add("Введите сумму");
            }
            else
            {
                (ok, personalExpense.Sum) = Utils.CheckNumberIsPositive(value);
                if (!ok) errors.Add("Сумма должна быть больше 0");
            }

            (ok, personalExpense.DatePayment) = Utils.CheckDate(date);
            if (!ok) errors.Add("Слишком маленькая дата");

            return (errors, personalExpense, id);
        }

        //Добавление и изменение коммунальных услуг
        public static (List<string> errors, HCSExpense hcsExpense) CheckHCSExpense(DateTime date, string sum)
        {
            HCSExpense hcsExpense = new HCSExpense();
            List<string> errors = new List<string>();

            (ok, hcsExpense.DatePayment) = Utils.CheckDate(date);
            if (!ok) errors.Add("Слишком маленькая дата");

            float value;
            (ok, value) = Utils.CheckNumberIsNotNull(sum);
            if (!ok)
            {
                errors.Add("Введите сумму");
            }
            else
            {
                (ok, hcsExpense.Sum) = Utils.CheckNumberIsPositive(value);
                if (!ok) errors.Add("Сумма должна быть больше 0");
            }

            return (errors, hcsExpense);
        }
    }
}
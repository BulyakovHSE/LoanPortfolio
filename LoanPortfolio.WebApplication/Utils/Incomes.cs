using LoanPortfolio.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanPortfolio.WebApplication
{
    public class Incomes
    {
        static private bool ok;

        //Добавление и изменение дополнительного дохода
        public static (List<string> errors, PeriodicIncome periodicIncome) CheckPeriodIncome(string source, string sum)
        {
            PeriodicIncome periodicIncome = new PeriodicIncome();
            List<string> errors = new List<string>();

            (ok, periodicIncome.IncomeSource) = Utils.CheckTextIsNotNull(source);
            if (!ok) errors.Add("Введите название");

            float value;
            (ok, value) = Utils.CheckNumberIsNotNull(sum);
            if (!ok)
            {
                errors.Add("Введите сумму");
            }
            else
            {
                (ok, periodicIncome.Sum) = Utils.CheckNumberIsPositive(value);
                if (!ok) errors.Add("Сумма должна быть больше 0");
            }

            return (errors, periodicIncome);
        }
    }
}
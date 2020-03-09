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
        public static (List<string> errors, PeriodicIncome periodicIncome) CheckPeriodIncome(string source, string sum, DateTime date)
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
                if (!ok) errors.Add("Сумма дохода должна быть больше 0");
            }

            (ok, periodicIncome.DateIncome) = Utils.CheckDate(date);
            if (!ok) errors.Add("Слишком маленькая дата");

            return (errors, periodicIncome);
        }

        //Добавление и изменение основного дохода
        public static (List<string> errors, RegularIncome regularIncome) CheckRegularIncome(string source, string prepaidExpanse, DateTime datePrepaidExpanse, string salary, DateTime dateSalary)
        {
            RegularIncome regularIncome = new RegularIncome();
            List<string> errors = new List<string>();

            (ok, regularIncome.IncomeSource) = Utils.CheckTextIsNotNull(source);
            if (!ok) errors.Add("Введите название");

            float prepaidExpanseValue;
            (ok, prepaidExpanseValue) = Utils.CheckNumberIsNotNull(prepaidExpanse);
            if (!ok)
            {
                errors.Add("Введите сумму аванса");
            }
            else
            {
                (ok, regularIncome.PrepaidExpanse) = Utils.CheckNumberIsPositive(prepaidExpanseValue);
                if (!ok) errors.Add("Сумма аванса должна быть больше 0");
            }

            (ok, regularIncome.DatePrepaidExpanse) = Utils.CheckDate(datePrepaidExpanse);
            if (!ok) errors.Add("Слишком маленькая дата аванса");

            float salaryValue;
            (ok, salaryValue) = Utils.CheckNumberIsNotNull(salary);
            if (!ok)
            {
                errors.Add("Введите сумму доплаты");
            }
            else
            {
                (ok, regularIncome.Salary) = Utils.CheckNumberIsPositive(salaryValue);
                if (!ok) errors.Add("Сумма доплаты должна быть больше 0");
            }

            (ok, regularIncome.DateSalary) = Utils.CheckDate(dateSalary);
            if (!ok) errors.Add("Слишком маленькая дата доплаты");

            if (dateSalary <= datePrepaidExpanse)
            {
                errors.Add("Дата доплаты должна быть больше даты аванса");
            }

            regularIncome.DatePrepaidExpanse = datePrepaidExpanse;
            regularIncome.DateSalary = dateSalary;

            return (errors, regularIncome);
        }
    }
}
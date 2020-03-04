using LoanPortfolio.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanPortfolio.WebApplication
{
    public class Utils
    {
        //Введена ли строка
        public static (bool ok, string result) CheckTextIsNotNull(string text)
        {
            string result = text.Trim();
            return (!string.IsNullOrWhiteSpace(result), result);
        }

        //Проверка на число
        public static (bool ok, float result) CheckNumberIsNotNull(string text)
        {
            float result;
            if (float.TryParse(text, out result))
            {
                return (true, result);
            }

            return (false, 0);
        }

        //Проверка на положительное число
        public static (bool ok, float result) CheckNumberIsPositive(float value)
        {
            if (value > 0)
            {
                return (true, value);
            }

            return (false, 0);
        }

        //Проверка на минимальную дату
        public static (bool ok, DateTime result) CheckDate(DateTime date)
        {
            if (date >= new DateTime(2000, 1, 1))
            {
                return (true, date);
            }
            return (false, DateTime.Now);
        }
    }
}
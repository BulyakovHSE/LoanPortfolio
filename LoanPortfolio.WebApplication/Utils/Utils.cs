using LoanPortfolio.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanPortfolio.WebApplication
{
    public class Utils
    {
        //Проверка количества месяцев
        public bool checkMounthNumber(int value)
        {
            return value > 0;
        }
    }
}
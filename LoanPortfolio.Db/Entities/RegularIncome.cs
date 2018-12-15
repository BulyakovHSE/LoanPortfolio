using System;

namespace LoanPortfolio.Db.Entities
{
    public class RegularIncome : Income
    {
        public DateTime DatePrepaidExpanse { get; set; }

        public float PrepaidExpanse { get; set; }

        public DateTime DateSalary { get; set; }

        public float Salary { get; set; }
    }
}
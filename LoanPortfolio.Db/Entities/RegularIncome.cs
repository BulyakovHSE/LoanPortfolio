using System;
using System.ComponentModel.DataAnnotations;

namespace LoanPortfolio.Db.Entities
{
    public class RegularIncome : Income
    {
        [Required]
        public DateTime DatePrepaidExpanse { get; set; }

        [Required]
        public float PrepaidExpanse { get; set; }

        [Required]
        public DateTime DateSalary { get; set; }

        [Required]
        public float Salary { get; set; }

        [Required]
        public override float Sum {
            set => value = 0;
            get => PrepaidExpanse + Salary; }
        }
}
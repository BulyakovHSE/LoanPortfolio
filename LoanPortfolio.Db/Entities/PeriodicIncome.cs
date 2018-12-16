using System;
using System.ComponentModel.DataAnnotations;

namespace LoanPortfolio.Db.Entities
{
    public class PeriodicIncome : Income
    {
        [Required]
        public DateTime DateIncome { get; set; }

        [Required]
        public override float Sum { get; set; }
    }
}
using LoanPortfolio.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanPortfolio.WebApplication
{
    public class Loans
    {
        static private bool ok;

        //Добавление и изменение кредитов
        public static (List<string> errors, Loan loan) CheckLoan(DateTime date, string loanSum, string amountDie, string repaymentPeriod, string notification)
        {
            Loan loan = new Loan();
            List<string> errors = new List<string>();

            float loanSumValue;
            (ok, loanSumValue) = Utils.CheckNumberIsNotNull(loanSum);
            if (!ok)
            {
                errors.Add("Введите сумму кредита");
            }
            else
            {
                (ok, loan.LoanSum) = Utils.CheckNumberIsPositive(loanSumValue);
                if (!ok) errors.Add("Cумма кредита должна быть больше 0");
            }

            float amountDieValue;
            (ok, amountDieValue) = Utils.CheckNumberIsNotNull(amountDie);
            if (!ok)
            {
                errors.Add("Введите сумму погашения");
            }
            else
            {
                (ok, loan.AmountDie) = Utils.CheckNumberIsPositive(amountDieValue);
                if (!ok)
                {
                    errors.Add("Cумма погашения должна быть больше 0");
                }
                else
                {
                    if (loan.AmountDie <= loan.LoanSum) errors.Add("Cумма погашения должна быть больше суммы кредита");
                }
            }

            int repaymentPeriodValue;
            if (!int.TryParse(repaymentPeriod, out repaymentPeriodValue))
            {
                errors.Add("Введите целое значение месяцев");
            }
            else
            {
                if (repaymentPeriodValue <= 0)
                {
                    errors.Add("Количество месяцев должно быть больше 0");
                }
                else
                {
                    loan.RepaymentPeriod = repaymentPeriodValue;
                }
            }

            (ok, loan.ClearanceDate) = Utils.CheckDate(date);
            if (!ok) errors.Add("Слишком маленькая дата");


            string resultNotification = notification.Trim();
            if (string.IsNullOrWhiteSpace(resultNotification))
            {
                loan.AdditionalNotificationRequired = false;
            }
            else
            {
                double result;
                if (Double.TryParse(resultNotification, out result))
                {
                    if (result == 0 || result == 1)
                    {
                        loan.AdditionalNotificationRequired = false;
                    }
                    else
                    {
                        loan.AdditionalNotificationTimeSpan = TimeSpan.FromDays(result);
                    }
                }
                else
                {
                    errors.Add("Кол-во дней для напоминания должно быть целым");
                }
            }

            return (errors, loan);
        }
    }
}
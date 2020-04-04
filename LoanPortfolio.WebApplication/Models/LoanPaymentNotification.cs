using System;
using LoanPortfolio.Db.Entities;

namespace LoanPortfolio.WebApplication.Models
{
    public class LoanPaymentNotification : Notification
    {
        private LoanPayment _loan;
        private bool _isLastPayment;

        public override DateTime ExecuteDateTime { get; }

        public override string Subject => "Уведомление об оплате кредита";

        public override string Text =>
            $"<h2>Напоминание об оплате кредита в {_loan.CreditInstitutionName} {_loan.DatePayment.ToShortDateString()}</h2>" +
            $"<p>Уважаемый(-ая) {User.FirstName} {User.LastName}!</p>" +
            $"<p>Напоминаем вам о том, что {_loan.DatePayment.ToShortDateString()} необходимо оплатить кредит на сумму {_loan.Sum} ₽ в {_loan.CreditInstitutionName}</p>" +
            $"{(string.IsNullOrWhiteSpace(_loan.BankAddress) ? "" : $"<p>Адрес банка (банкомата) для внесения платежа: {_loan.BankAddress}</p>")}" +
            $"{(_isLastPayment ? "<p>Данный платеж является последним. Пожалуйста, обратитель в банк для официального закрытия кредита!</p>" : "")}";

        public LoanPaymentNotification(LoanPayment expense, User user, bool isLastPayment, DateTime executeDateTime = default(DateTime)) : base(expense, user)
        {
            ExecuteDateTime = executeDateTime == default(DateTime) ? expense.DatePayment.AddDays(-1).SetTime(23, 25) : executeDateTime;
            _loan = expense;
            _isLastPayment = isLastPayment;
        }
    }
}
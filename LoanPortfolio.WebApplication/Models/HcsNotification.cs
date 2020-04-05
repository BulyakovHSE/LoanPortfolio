using System;
using LoanPortfolio.Db.Entities;

namespace LoanPortfolio.WebApplication.Models
{
    public class HcsNotification : Notification
    {
        private HCSExpense _hcs;

        public override DateTime ExecuteDateTime => _hcs.DatePayment.AddDays(-3).SetTime(10, 00);

        public override string Subject => "Уведомление об оплате услуг ЖКХ";

        public override string Text =>
            $"<h2>Напоминание об оплате услуг ЖКХ " +
            $"{(string.IsNullOrWhiteSpace(_hcs.Comment) ? _hcs.DatePayment.ToShortDateString() : $"{_hcs.Comment} {_hcs.DatePayment.ToShortDateString()}")}</h2>" +
            $"<p>Уважаемый(-ая) {User.FirstName} {User.LastName}!</p>" +
            $"<p>Напоминаем вам о том, что {_hcs.DatePayment.ToShortDateString()} необходимо оплатить услуги ЖКХ на сумму {_hcs.Sum} ₽</p>" +
            $"{(string.IsNullOrWhiteSpace(_hcs.Comment) ? "" : $"<p>Ваш комментарий к платежу: {_hcs.Comment}</p>")}";

        public HcsNotification(HCSExpense expense, User user) : base(expense, user)
        {
            _hcs = expense;
        }
    }
}
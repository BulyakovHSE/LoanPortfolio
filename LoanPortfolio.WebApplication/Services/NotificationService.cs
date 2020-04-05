using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Db.Repositories;
using LoanPortfolio.WebApplication.Models;

namespace LoanPortfolio.WebApplication.Services
{
    public class NotificationService
    {
        private IRepository<User> _userRepository;
        private IRepository<Expense> _expenseRepository;
        private IRepository<Loan> _loanRepository;
        private SmtpClient _smtp;
        private Timer _timer;

        private List<Notification> _notifications;

        public NotificationService()
        {
            var context = new LoanContext();
            _userRepository = new EntityFrameworkRepository<User>(context);
            _expenseRepository = new EntityFrameworkRepository<Expense>(context);
            _loanRepository = new EntityFrameworkRepository<Loan>(context);
            _smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailLogin"],
                    ConfigurationManager.AppSettings["EmailPassword"]),
                EnableSsl = true
            };
            UpdateNotificationsList();

            _timer = new Timer(Execute, null, 0, 60000);
        }

        private void Execute(object o)
        {
            var datetime = DateTime.Now;
            if (datetime.Hour == 0 && datetime.Minute == 0)
                UpdateNotificationsList();
            var notifications = _notifications.Where(x =>
                x.ExecuteDateTime.Hour == datetime.Hour && x.ExecuteDateTime.Minute == datetime.Minute);

            foreach (var notification in notifications)
            {
                var from = new MailAddress("loanportfoliohse@gmail.com", "Кредитный портфель");
                var to = new MailAddress(notification.User.Email);
                var message = new MailMessage(from, to)
                {
                    Subject = notification.Subject, Body = notification.Text, IsBodyHtml = true
                };
                _smtp.Send(message);
            }
        }

        public void UpdateNotificationsList()
        {
            _notifications = new List<Notification>();
            var list = new List<Notification>();
            var hcs = _expenseRepository.All().Select(x => x as HCSExpense).Where(x => x != null);
            foreach (var expense in hcs)
            {
                expense.User = _userRepository.All().FirstOrDefault(x => x.Id == expense.UserId);
                list.Add(new HcsNotification(expense, expense.User));
            }

            var loans = _expenseRepository.All().Select(x => x as LoanPayment).Where(x => x != null);
            foreach (var payment in loans)
            {
                var loan = _loanRepository.All().FirstOrDefault(x => x.Id == payment.LoanId);
                if(loan == null) return;
                payment.Loan = loan;
                payment.User = _userRepository.All().FirstOrDefault(x => x.Id == loan.UserId);
                var isLastPayment = payment.DatePayment == loan.ClearanceDate.AddMonths(loan.RepaymentPeriod);
                list.Add(new LoanPaymentNotification(payment, payment.User, isLastPayment));
                if (loan.AdditionalNotificationRequired)
                {
                    list.Add(new LoanPaymentNotification(payment, payment.User, isLastPayment,
                        payment.DatePayment.AddDays(loan.AdditionalNotificationTimeSpan.Days).SetTime(
                        loan.AdditionalNotificationTimeSpan.Hours,
                        loan.AdditionalNotificationTimeSpan.Minutes)));
                }
            }

            var dateNow = DateTime.Now;
            _notifications.AddRange(list.Where(x => x.ExecuteDateTime.Day == dateNow.Day &&
                                                    x.ExecuteDateTime.Month == dateNow.Month &&
                                                    x.ExecuteDateTime.Year == dateNow.Year));
        }

        public void UpdateNotificationsList(int userId)
        {
            _notifications.RemoveAll(x => x.User.Id == userId);

            var list = new List<Notification>();
            var user = _userRepository.All().FirstOrDefault(x => x.Id == userId);
            if (user == null) return;
            var hcs = user.Expenses.Select(x => x as HCSExpense).Where(x => x != null);
            foreach (var expense in hcs)
            {
                expense.User = _userRepository.All().FirstOrDefault(x => x.Id == expense.UserId);
                list.Add(new HcsNotification(expense, expense.User));
            }

            var loans = user.Expenses.Select(x => x as LoanPayment).Where(x => x != null);

            foreach (var payment in loans)
            {
                var loan = _loanRepository.All().FirstOrDefault(x => x.Id == payment.LoanId);
                if (loan == null) return;
                payment.Loan = loan;
                payment.User = _userRepository.All().FirstOrDefault(x => x.Id == loan.UserId);
                var isLastPayment = payment.DatePayment == loan.PaymentsSchedule.Keys.Last();
                list.Add(new LoanPaymentNotification(payment, user, isLastPayment));
                if (loan.AdditionalNotificationRequired)
                {
                    list.Add(new LoanPaymentNotification(payment, user, isLastPayment,
                        payment.DatePayment.AddDays(loan.AdditionalNotificationTimeSpan.Days).SetTime(
                            loan.AdditionalNotificationTimeSpan.Hours,
                            loan.AdditionalNotificationTimeSpan.Minutes)));
                }
            }

            var dateNow = DateTime.Now;
            _notifications.AddRange(list.Where(x => x.ExecuteDateTime.Day == dateNow.Day &&
                                                    x.ExecuteDateTime.Month == dateNow.Month &&
                                                    x.ExecuteDateTime.Year == dateNow.Year));
        }
    }
}
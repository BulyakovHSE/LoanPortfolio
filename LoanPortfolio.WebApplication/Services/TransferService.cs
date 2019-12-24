using System;
using System.Linq;
using System.Threading;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Db.Repositories;
using LoanPortfolio.Services;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.WebApplication.Services
{
    /// <summary>
    /// Сервис переноса финансовых показетелей пользователя на новый месяц
    /// </summary>
    public class TransferService
    {
        private IRepository<User> _userRepository;
        private IRepository<Income> _incomeRepository;
        private IRepository<Expense> _expenseRepository;
        private DateTime _nextMonthDate;
        private Timer _timer;

        public TransferService()
        {
            _nextMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
            _timer = new Timer(Execute, null, 0, 60000);
        }

        /// <summary>
        /// Выполнение метода TransferUserFinances для каждого пользователя если наступил первый день следующего месяца
        /// </summary>
        private void Execute(object o)
        {
            if (DateTime.Now.Date == _nextMonthDate)
            {
                var context = new LoanContext();
                _incomeRepository = new EntityFrameworkRepository<Income>(context);
                _expenseRepository = new EntityFrameworkRepository<Expense>(context);
                _userRepository = new EntityFrameworkRepository<User>(context);
                _nextMonthDate = _nextMonthDate.AddMonths(1);
                foreach (var user in _userRepository.All().ToList())
                    TransferUserFinances(user);
            }
        }

        /// <summary>
        /// Перенос доходов и расходов пользователя на следующий месяц
        /// </summary>
        /// <param name="user">Пользователь, чьи фин. показатели будут перенесены</param>
        private void TransferUserFinances(User user)
        {
            var incomes = _incomeRepository.All().Where(x => x.UserId == user.Id).ToList();
            var expenses = _expenseRepository.All().Where(x => x.UserId == user.Id).ToList();
            foreach (var income in incomes)
            {
                if (income is RegularIncome regular)
                {
                    var regularNew = new RegularIncome
                    {
                        DatePrepaidExpanse = regular.DatePrepaidExpanse.AddMonths(1),
                        DateSalary = regular.DateSalary.AddMonths(1),
                        IncomeSource = regular.IncomeSource,
                        PrepaidExpanse = regular.PrepaidExpanse,
                        Salary = regular.Salary,
                        User = user,
                        UserId = user.Id
                    };
                    _incomeRepository.Add(regularNew);
                }
                else if (income is PeriodicIncome periodic)
                {
                    var periodicNew = new PeriodicIncome
                    { DateIncome = periodic.DateIncome.AddMonths(1), IncomeSource = periodic.IncomeSource, Sum = 0f, User = user, UserId = user.Id };
                    _incomeRepository.Add(periodicNew);
                }
            }

            foreach (var expense in expenses)
            {
                if (expense is HCSExpense hcs)
                {
                    var hcsNew = new HCSExpense { Comment = hcs.Comment, DatePayment = hcs.DatePayment.AddMonths(1), Sum = 0f, User = user, UserId = user.Id };
                    _expenseRepository.Add(hcsNew);
                }
                else if (expense is Loan load)
                {
                    //todo: add Loan dates transfer after adding Loan payment schedule
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Moq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Db.Repositories;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services;
using LoanPortfolio.Services.Interfaces;
using LoanPortfolio.WebApplication.Services;
using System.Data.Entity;

namespace CreditPortfolioUnitTests
{
    [TestClass]
    public class TransferServiceTest
    {
        private LoanContext loanContext;
        private IRepository<User> _userRepository;
        private IRepository<Income> _incomeRepository;
        private IRepository<Expense> _expenseRepository;
        private IRepository<Loan> _loanRepository;

        private UserService userService;
        private IncomeService incomeService;
        private ExpenseService expenseService;
        private LoanService loanService;
        private TransferService transferService;

        private User _user;
        private Loan _loan;
        private PeriodicIncome _periodicIncome;
        private RegularIncome _regularIncome;
        private HCSExpense _HCSExpense;
        private PersonalExpense _personalExpense;
        private List<LoanPayment> _loanPaymentExpenseList;

        [TestInitialize]
        public void TestInitialize()
        {
            loanContext = new LoanContext();
            loanContext.Database.CreateIfNotExists();
            _incomeRepository = new EntityFrameworkRepository<Income>(loanContext);
            _expenseRepository = new EntityFrameworkRepository<Expense>(loanContext);
            _userRepository = new EntityFrameworkRepository<User>(loanContext);
            _loanRepository = new EntityFrameworkRepository<Loan>(loanContext);

            userService = new UserService(_userRepository);
            incomeService = new IncomeService(_incomeRepository);
            expenseService = new ExpenseService(_expenseRepository);
            loanService = new LoanService(_loanRepository,_expenseRepository);
            
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            loanContext.Database.Delete();
        }

        [TestMethod]
        public void TransferTest()
        {
            _user = userService.Add("222@mail.ru", "123456789", "Test", "Yestovich");
            _regularIncome = incomeService.AddRegularIncome(_user, "Отас", new DateTime(2020, 03, 02), 600, new DateTime(2020, 03, 24), 6000);
            _periodicIncome = incomeService.AddPeriodicIncome(_user, "Подработка", 235, new DateTime(2020, 03, 20));
            _HCSExpense = expenseService.AddHCSExpense(_user, new DateTime(2020, 03, 01), 6000, "Ltd");
            _personalExpense = expenseService.AddPersonalExpense(_user, new DateTime(2020, 03, 06), 800, new Category { Name = "Развлечения", User = _user });
            _loan = loanService.AddLoan(_user, 20000, new DateTime(2020, 03, 13), 20000, 1, "Ад 666", "666 хыв");

            _regularIncome.DatePrepaidExpanse.AddMonths(1);
            _regularIncome.DateSalary.AddMonths(1);
            _periodicIncome.DateIncome.AddMonths(1);
            _HCSExpense.DatePayment.AddMonths(1);
            _personalExpense.DatePayment.AddMonths(1);
            _loan.ClearanceDate.AddMonths(1);

            Transfer(_user);

            PeriodicIncome actualPeriodicIncome = (PeriodicIncome)incomeService.GetById(2);
            RegularIncome actualRegularIncome = (RegularIncome) incomeService.GetById(1);
            HCSExpense actualHCSExpense = (HCSExpense) expenseService.GetById(1);
            PersonalExpense actualPersonalExpense = (PersonalExpense)expenseService.GetById(2);
            LoanPayment actualLoanPaymentExpense = (LoanPayment)expenseService.GetById(3);

            Assert.AreEqual(_regularIncome.DatePrepaidExpanse, actualRegularIncome.DatePrepaidExpanse);
            Assert.AreEqual(_regularIncome.DateSalary, actualRegularIncome.DateSalary);
            Assert.AreEqual(_periodicIncome.DateIncome, actualPeriodicIncome.DateIncome);
            Assert.AreEqual(_personalExpense.DatePayment, actualPersonalExpense.DatePayment);
            Assert.AreEqual(_HCSExpense.DatePayment, actualHCSExpense.DatePayment);
            //Assert.AreEqual(_HCSExpense.DatePayment, actualLoanPaymentExpense.DatePayment);
        }

        [TestMethod]
        public void TransferfullTest()
        {
            _user = userService.Add("222@mail.ru", "123456789", "Test", "Yestovich");
            _regularIncome = incomeService.AddRegularIncome(_user, "Отас", new DateTime(2020, 02, 02), 600, new DateTime(2020, 02, 24), 6000);
            _periodicIncome = incomeService.AddPeriodicIncome(_user, "Подработка", 235, new DateTime(2020, 02, 20));
            _HCSExpense = expenseService.AddHCSExpense(_user, new DateTime(2020, 02, 01), 6000, "Ltd");
            _personalExpense = expenseService.AddPersonalExpense(_user, new DateTime(2020, 02, 06), 800, new Category { Name = "Развлечения", User = _user });
            _loan = loanService.AddLoan(_user, 20000, new DateTime(2020, 02, 13), 20000, 1, "Ад 666", "666 хыв");

            TransferService transferService = new TransferService();

            //RegularIncome actualRegularIncome = (RegularIncome)incomeService.GetById(1);
            //PeriodicIncome actualPeriodicIncome = (PeriodicIncome)incomeService.GetById(2);
            //HCSExpense actualHCSExpense = (HCSExpense)expenseService.GetById(1);
            //PersonalExpense actualPersonalExpense = (PersonalExpense)expenseService.GetById(2);
            //LoanPayment actualLoanPaymentExpense = (LoanPayment)expenseService.GetById(3);

            RegularIncome actualRegularIncome = (RegularIncome)incomeService.GetById(3);
            PeriodicIncome actualPeriodicIncome = (PeriodicIncome)incomeService.GetById(4);
            HCSExpense actualHCSExpense = (HCSExpense)expenseService.GetById(4);
            PersonalExpense actualPersonalExpense = (PersonalExpense)expenseService.GetById(5);
            LoanPayment actualLoanPaymentExpense = (LoanPayment)expenseService.GetById(6);

            Assert.AreEqual(_regularIncome.DatePrepaidExpanse.AddMonths(2), actualRegularIncome.DatePrepaidExpanse);
            Assert.AreEqual(_regularIncome.DateSalary.AddMonths(2), actualRegularIncome.DateSalary);
            Assert.AreEqual(_periodicIncome.DateIncome.AddMonths(2), actualPeriodicIncome.DateIncome);
            Assert.AreEqual(_personalExpense.DatePayment.AddMonths(2), actualPersonalExpense.DatePayment);
            Assert.AreEqual(_HCSExpense.DatePayment.AddMonths(2), actualHCSExpense.DatePayment);
        }

        //copy paste of TransferUserFinances function from TransferService
        private void Transfer(User user)
        {
            var incomes = _incomeRepository.All().Where(x => x.UserId == user.Id).ToList();
            var expenses = _expenseRepository.All().Where(x => x.UserId == user.Id).ToList();
            foreach (var income in incomes)
            {
                if (income is RegularIncome regular && regular.DateSalary.Year == DateTime.Now.Year && regular.DateSalary.Month == DateTime.Now.Month)
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
                else if (income is PeriodicIncome periodic && periodic.DateIncome.Year == DateTime.Now.Year && periodic.DateIncome.Month == DateTime.Now.Month)
                {
                    var periodicNew = new PeriodicIncome
                    { DateIncome = periodic.DateIncome.AddMonths(1), IncomeSource = periodic.IncomeSource, Sum = 0f, User = user, UserId = user.Id };
                    _incomeRepository.Add(periodicNew);
                }
            }

            foreach (var expense in expenses)
            {
                if (expense is HCSExpense hcs && hcs.DatePayment.Year == DateTime.Now.Year && hcs.DatePayment.Month == DateTime.Now.Month)
                {
                    var hcsNew = new HCSExpense { Comment = hcs.Comment, DatePayment = hcs.DatePayment.AddMonths(1), Sum = 0f, User = user, UserId = user.Id };
                    _expenseRepository.Add(hcsNew);
                }
                else if (expense is LoanPayment loan && loan.Loan != null && loan.DatePayment.Year == DateTime.Now.Year && loan.DatePayment.Month == DateTime.Now.Month)
                {
                    var payment = loan.Loan.PaymentsSchedule.FirstOrDefault(x =>
                        x.Key.Year == DateTime.Now.Year && x.Key.Month == DateTime.Now.Month);
                    if (!payment.Equals(default(KeyValuePair<DateTime, float>)))
                    {
                        var loanPayment = new LoanPayment()
                        {
                            BankAddress = loan.BankAddress,
                            CreditInstitutionName = loan.CreditInstitutionName,
                            User = loan.User,
                            Loan = loan.Loan,
                            LoanId = loan.LoanId,
                            Sum = payment.Value,
                            UserId = loan.UserId,
                            DatePayment = payment.Key
                        };
                        _expenseRepository.Add(loanPayment);
                    }
                }
            }
        }
    }
}

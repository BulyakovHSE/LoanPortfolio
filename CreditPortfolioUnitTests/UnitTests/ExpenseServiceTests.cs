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
using System.Data.Entity;

namespace CreditPortfolioUnitTests
{
    [TestClass]
    public class ExpenseServiceTests
    {
        private ExpenseService expenseService;
        //private string DbConnect;

        private User _user;
        private DateTime _datePayment;
        private DateTime _datePayment2;
        private float _sum;
        private float _sum2;
        private Category _category;
        private string _comment;

        [TestInitialize]
        public void TestInitialize()
        {
            _user = new User
            {
                Id = 0,
                Email = "222@mail.ru",
                Password = "12345",
                FirstName = "Test",
                LastName = "Yestovish",
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };
            _datePayment = new DateTime(2020, 04, 03);
            _datePayment2 = new DateTime(2020, 04, 10);
            _sum = 600;
            _sum2 = 500;
            _category = new Category() { Name = "dsg" };
            _comment = "324";

            //expenseService = new ExpenseService(new EntityFrameworkRepository<Expense>(new DbContext(DbConnect)));
        }

        /// <summary>
        /// Тестирование добавления персональных затрат
        /// </summary>
        [TestMethod]
        public void AddPersonalExpenseTest()
        {
            PersonalExpense expected = new PersonalExpense
            {
                Id = 0,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                ExpenseCategory = _category
            };

            var mockRepository = new Mock<IRepository<Expense>>();
            mockRepository.Setup(rep => rep.Add(It.IsAny<PersonalExpense>())).Returns(expected);
            expenseService = new ExpenseService(mockRepository.Object);

            PersonalExpense actual = expenseService.AddPersonalExpense(_user, _datePayment, _sum, _category);

            Assert.AreEqual(expected,actual);
        }

        /// <summary>
        /// Тестирование добавление капитальных рассходов
        /// </summary>
        [TestMethod]
        public void AddHCSExpenseTest()
        {
            HCSExpense expected = new HCSExpense
            {
                UserId = _user.Id,
                DatePayment = _datePayment2,
                Sum = _sum2,
                Comment = _comment
            };

            var mockRepository = new Mock<IRepository<Expense>>();
            mockRepository.Setup(rep => rep.Add(It.IsAny<HCSExpense>())).Returns(expected);
            expenseService = new ExpenseService(mockRepository.Object);

            HCSExpense actual = expenseService.AddHCSExpense(_user, _datePayment2,_sum2,_comment);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            PersonalExpense expected = new PersonalExpense
            {
                Id = 0,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                ExpenseCategory = _category
            };

            var mockRepository = new Mock<IRepository<Expense>>();
            mockRepository.Setup(rep => rep.Add(It.IsAny<PersonalExpense>())).Returns(expected);
            mockRepository.Setup(rep => rep.All().SingleOrDefault(x=> x.Id == 0)).Returns(expected);
            expenseService = new ExpenseService(mockRepository.Object);

            PersonalExpense personalExpense = expenseService.AddPersonalExpense(_user, _datePayment, _sum, _category);
            //HCSExpense hcsExpense = expenseService.AddHCSExpense(user, datePayment2, sum2, "Bonus");

            PersonalExpense actual = (PersonalExpense)expenseService.GetById(0);

            Assert.AreEqual(personalExpense, actual);
        }

        [TestMethod]
        public void UpdatePersonalExpenceTest()
        {
            PersonalExpense personalExpense = new PersonalExpense
            {
                Id = 0,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                ExpenseCategory = _category
            };

            //PersonalExpense personalExpense = expenseService.AddPersonalExpense(_user, _datePayment, _sum, _category);
            //HCSExpense hcsExpense = expenseService.AddHCSExpense(_user, _datePayment2, _sum2, _comment);

            var mockRepository = new Mock<IRepository<Expense>>();
            mockRepository.Setup(rep => rep.Update(It.IsAny<Expense>()));
            mockRepository.Setup(rep => rep.All().SingleOrDefault(x => x.Id == 0)).Returns(personalExpense);
            expenseService = new ExpenseService(mockRepository.Object);

            personalExpense.DatePayment = new DateTime(2020,04,01);
            personalExpense.Sum = 9000;

            expenseService.UpdateExpense(personalExpense);

            PersonalExpense actual = (PersonalExpense)expenseService.GetById(0);

            Assert.AreEqual(personalExpense, actual);
        }


        //[TestMethod]
        //public void UpdateHCSExpenceTest()
        //{
        //    User user = new User
        //    {
        //        Id = 1,
        //        Email = "222@mail.ru",
        //        Password = "12345",
        //        FirstName = "Test",
        //        LastName = "Yestovish",
        //        Incomes = new List<Income>(),
        //        Expenses = new List<Expense>(),
        //        Loans = new List<Loan>()
        //    };
        //    DateTime datePayment = new DateTime(2020, 04, 03);
        //    float sum = 600;

        //    DateTime datePayment2 = new DateTime(2020, 04, 10);
        //    float sum2 = 500;

        //    PersonalExpense personalExpense = expenseService.AddPersonalExpense(user, datePayment, sum, new Category() { Name = "dsg" });
        //    HCSExpense hcsExpense = expenseService.AddHCSExpense(user, datePayment2, sum2, "Bonus");

        //    hcsExpense.DatePayment = new DateTime(2020,04,13);
        //    hcsExpense.Sum = 1;
        //    hcsExpense.Comment = "134";

        //    expenseService.UpdateExpense(hcsExpense);

        //    HCSExpense actual = (HCSExpense)expenseService.GetById(1);

        //    Assert.AreEqual(hcsExpense, actual);
        //}

        [TestMethod]
        public void GetAllTest()
        {
            PersonalExpense personalExpense = new PersonalExpense{
                Id = 0,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                ExpenseCategory = _category
            };
            HCSExpense hcsExpense = new HCSExpense
            {
                Id = 1,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                Comment = _comment
            };

            List<Expense> expected = new List<Expense> { personalExpense, hcsExpense };

            var mockRepository = new Mock<IRepository<Expense>>();
            mockRepository.Setup(rep => rep.All().AsQueryable<Expense>().Where(x => x.UserId == It.IsAny<int>())).Returns(expected.AsQueryable<Expense>());
            expenseService = new ExpenseService(mockRepository.Object);

            IEnumerable<Expense> actual = expenseService.GetAll(_user);

            CollectionAssert.AreEquivalent(expected, actual.ToList());
        }

        /// <summary>
        /// ТЕстирование удаления трат пользователя
        /// </summary>
        [TestMethod]
        public void RemovePersonalExpenseTest()
        {
            PersonalExpense personalExpense = new PersonalExpense
            {
                Id = 0,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                ExpenseCategory = _category
            };
            HCSExpense hcsExpense = new HCSExpense
            {
                Id = 1,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                Comment = _comment
            };

            List<Expense> expected = new List<Expense> { hcsExpense };
            IEnumerable<Expense> en = expected;

            var mockRepository = new Mock<IRepository<Expense>>();
            mockRepository.Setup(rep => rep.Remove(It.IsAny<Expense>()));
            mockRepository.Setup(rep => rep.All().Where(x => x.UserId == 0)).Returns(en.AsQueryable<Expense>());
            expenseService = new ExpenseService(mockRepository.Object);

            expenseService.Remove(personalExpense);

            IEnumerable<Expense> expenseList = expenseService.GetAll(_user);

            CollectionAssert.DoesNotContain(expenseList.ToList() ,personalExpense);
        }

        //[TestMethod]
        //public void RemoveHCSExpenseTest()
        //{
        //    User user = new User
        //    {
        //        Id = 1,
        //        Email = "222@mail.ru",
        //        Password = "12345",
        //        FirstName = "Test",
        //        LastName = "Yestovish",
        //        Incomes = new List<Income>(),
        //        Expenses = new List<Expense>(),
        //        Loans = new List<Loan>()
        //    };
        //    DateTime datePayment = new DateTime(2020, 04, 03);
        //    float sum = 600;

        //    DateTime datePayment2 = new DateTime(2020, 04, 10);
        //    float sum2 = 500;

        //    PersonalExpense personalExpense = expenseService.AddPersonalExpense(user, datePayment, sum, new Category() { Name = "dsg" });
        //    HCSExpense hcsExpense = expenseService.AddHCSExpense(user, datePayment2, sum2, "Bonus");

        //    expenseService.Remove(hcsExpense);

        //    IEnumerable<Expense> expenseList = expenseService.GetAll(user);

        //    CollectionAssert.DoesNotContain(expenseList.ToList(), hcsExpense);
        //}
    
        [TestMethod]
        public void CheckUser1()
        {
            PersonalExpense expected = new PersonalExpense
            {
                Id = 0,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                ExpenseCategory = _category
            };
            Assert.AreEqual(_user, expected.User);
        }

        [TestMethod]
        public void CheckUserId1()
        {
            PersonalExpense expected = new PersonalExpense
            {
                Id = 0,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                ExpenseCategory = _category
            };
            Assert.AreEqual(_user.Id, expected.UserId);
        }

        [TestMethod]
        public void CheckDatePayment1()
        {
            PersonalExpense expected = new PersonalExpense
            {
                Id = 0,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                ExpenseCategory = _category
            };
            Assert.AreEqual(_datePayment, expected.DatePayment);
        }

        [TestMethod]
        public void CheckSum1()
        {
            PersonalExpense expected = new PersonalExpense
            {
                Id = 0,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                ExpenseCategory = _category
            };
            Assert.AreEqual(_sum, expected.Sum);
        }

        [TestMethod]
        public void CheckExpanseCategory()
        {
            PersonalExpense expected = new PersonalExpense
            {
                Id = 0,
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                ExpenseCategory = _category
            };
            Assert.AreEqual(_category, expected.ExpenseCategory);
        }

        [TestMethod]
        public void CheckUser2()
        {
            HCSExpense expected = new HCSExpense
            {
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                Comment = _comment
            };
            Assert.AreEqual(_user, expected.User);
        }

        [TestMethod]
        public void CheckUserId2()
        {
            HCSExpense expected = new HCSExpense
            {
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                Comment = _comment
            };
            Assert.AreEqual(_user.Id, expected.UserId);
        }

        [TestMethod]
        public void CheckDatePayment2()
        {
            HCSExpense expected = new HCSExpense
            {
                UserId = _user.Id,
                DatePayment = _datePayment2,
                Sum = _sum,
                Comment = _comment
            };
            Assert.AreEqual(_datePayment2, expected.DatePayment);
        }

        [TestMethod]
        public void CheckSum2()
        {
            HCSExpense expected = new HCSExpense
            {
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum2,
                Comment = _comment
            };
            Assert.AreEqual(_sum2, expected.Sum);
        }

        [TestMethod]
        public void CheckComment()
        {
            HCSExpense expected = new HCSExpense
            {
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                Comment = _comment
            };
            Assert.AreEqual(_comment, expected.Comment);
        }
    }
}

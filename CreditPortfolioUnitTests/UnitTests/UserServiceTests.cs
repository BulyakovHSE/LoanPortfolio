using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class UserServiceTests
    {
        private UserService userService;
        //private string DbConnect;

        string _email;
        string _password;
        string _firstName;
        string _lastName;

        /// <summary>
        /// Инициализация перед тестирования
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _email = "222@mail.ru";
            _password = "12345";
            _firstName = "Test";
            _lastName = "Testovich";

            var mockRepository = new Mock<IRepository<User>>();
            userService = new UserService(mockRepository.Object);
            //userService = new UserService(new EntityFrameworkRepository<User>(new DbContext(DbConnect)));

        }

        /// <summary>
        /// Тестирование добавление пользователя
        /// </summary>
        [TestMethod]
        public void AddUserTest()
        {
            User expected = new User
            { 
                Email = _email,
                Password = _password,
                FirstName = _firstName,
                LastName = _lastName,
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };

            var mockRepository = new Mock<IRepository<User>>();
            mockRepository.Setup(rep => rep.Add(It.IsAny<User>())).Returns(expected);
            userService = new UserService(mockRepository.Object);

            User actual = userService.Add(_email, _password, _firstName, _lastName);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Тестирование получение пользователя по Id
        /// </summary>
        [TestMethod]
        public void GetByIdTest()
        {
            User expected = new User
            {
                Email = _email,
                Password = _password,
                FirstName = _firstName,
                LastName = _lastName,
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };

            var mockRepository = new Mock<IRepository<User>>();
            mockRepository.Setup(rep => rep.All().First(u => u.Id == 0)).Returns(expected);
            userService = new UserService(mockRepository.Object);

            User actual = userService.GetById(0);

            Assert.AreEqual(expected,actual);
        }

        /// <summary>
        /// Тестирование изменение фамилии пользователя
        /// </summary>
        [TestMethod]
        public void ChangeLastNameTest()
        {
            User user = new User
            {
                Email = _email,
                Password = _password,
                FirstName = _firstName,
                LastName = _lastName,
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };

            string newLastName = "Erroro";
            user.LastName = newLastName;

            var mockRepository = new Mock<IRepository<User>>();
            mockRepository.Setup(rep => rep.Update(user));
            mockRepository.Setup(rep => rep.All().First(u => u.Id == 0)).Returns(user);
            userService = new UserService(mockRepository.Object);


            userService.ChangeLastName(user, newLastName);
            User actual = userService.GetById(0);

            Assert.AreEqual(user,actual);
        }

        /// <summary>
        /// Тестирование изменение имени пользователя
        /// </summary>
        [TestMethod]
        public void ChangeFirstName()
        {
            User user = new User
            {
                Email = _email,
                Password = _password,
                FirstName = _firstName,
                LastName = _lastName,
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };

            string newFirstName = "Err";
            user.FirstName = newFirstName;

            var mockRepository = new Mock<IRepository<User>>();
            mockRepository.Setup(rep => rep.Update(user));
            mockRepository.Setup(rep => rep.All().First(u => u.Id == 0)).Returns(user);
            userService = new UserService(mockRepository.Object);

            userService.ChangeLastName(user, newFirstName);
            User actual = userService.GetById(0);

            Assert.AreEqual(user, actual);
        }

        /// <summary>
        /// Тестирование изменение пароля пользователя
        /// </summary>
        [TestMethod]
        public void ChangePasswordTest()
        {
            User user = new User
            {
                Email = _email,
                Password = _password,
                FirstName = _firstName,
                LastName = _lastName,
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };

            string newPassword = "98765";
            user.Password = newPassword;

            var mockRepository = new Mock<IRepository<User>>();
            mockRepository.Setup(rep => rep.Update(user));
            mockRepository.Setup(rep => rep.All().First(u => u.Id == 0)).Returns(user);
            userService = new UserService(mockRepository.Object);

            userService.ChangePassword(user, newPassword);
            User actual = userService.GetById(0);

            Assert.AreEqual(user, actual);
        }

        /// <summary>
        /// Тестирование изменения почты пользователя
        /// </summary>
        [TestMethod]
        public void ChangeEmail()
        {
            User user = new User
            {
                Email = _email,
                Password = _password,
                FirstName = _firstName,
                LastName = _lastName,
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };

            string newEmail = "111@mail.com";
            user.Email = newEmail;

            var mockRepository = new Mock<IRepository<User>>();
            mockRepository.Setup(rep => rep.Update(It.IsAny<User>()));
            mockRepository.Setup(rep => rep.All().First(u => u.Id == 0)).Returns(user);
            userService = new UserService(mockRepository.Object);

            userService.ChangeEmail(user, newEmail);
            User actual = userService.GetById(0);

            Assert.AreEqual(user, actual);
        }

        /// <summary>
        /// Тестрование получение всех пользователей 
        /// </summary>
        [TestMethod]
        public void GetAllTest()
        {
            User user = new User
            {
                Email = _email,
                Password = _password,
                FirstName = _firstName,
                LastName = _lastName,
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };
            User user2 = new User{
                Email = "111@mail.ru", 
                Password = "123456", 
                FirstName = "Test2", 
                LastName ="Testovichi",
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };
            User user3 = new User { 
                Email ="333@mail.ru", 
                Password = "123457", 
                FirstName ="Test3", 
                LastName = "Testovichii",
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };

            List<User> expectedUserList = new List<User>() { user, user2, user3};

            var mockRepository = new Mock<IRepository<User>>();
            mockRepository.Setup(rep => rep.All()).Returns(expectedUserList.AsQueryable<User>);
            userService = new UserService(mockRepository.Object);

            IEnumerable<User> actualUserList = userService.GetAll();

            CollectionAssert.AreEquivalent(expectedUserList,actualUserList.ToList());
        }

        /// <summary>
        /// Тестирование удаления пользователя
        /// </summary>
        [TestMethod]
        public void RemoveTest()
        {
            User user = new User
            {
                Email = _email,
                Password = _password,
                FirstName = _firstName,
                LastName = _lastName,
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };
            User user2 = new User
            {
                Email = "111@mail.ru",
                Password = "123456",
                FirstName = "Test2",
                LastName = "Testovichi",
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };
            User user3 = new User
            {
                Email = "333@mail.ru",
                Password = "123457",
                FirstName = "Test3",
                LastName = "Testovichii",
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };

            List<User> expectedUserList = new List<User>() { user2, user3 };

            var mockRepository = new Mock<IRepository<User>>();
            mockRepository.Setup(rep => rep.Remove(user));
            mockRepository.Setup(rep => rep.All()).Returns(expectedUserList.AsQueryable<User>);
            userService = new UserService(mockRepository.Object);

            userService.Remove(user);
            IEnumerable<User> userData = userService.GetAll();

            CollectionAssert.DoesNotContain(userData.ToList(), user);
        }
    }
}

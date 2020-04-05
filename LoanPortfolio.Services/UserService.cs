using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User Add(string email, string pass, string firstName, string lastName)
        {
            var user = new User {Email = email, Password = pass, FirstName = firstName, LastName = lastName};
            var categories = new List<Category>();
            var cats = new[] {"Еда", "Здоровье", "Развлечения"};
            foreach (var cat in cats)
                categories.Add(new Category{Name = cat, User = user});
            user.Categories = categories;
            _userRepository.Add(user);
            return _userRepository.All().Single(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _userRepository.All().First(u => u.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.All();
        }

        public void ChangeLastName(User user, string lastName)
        {
            user.LastName = lastName;
            _userRepository.Update(user);
        }

        public void ChangeFirstName(User user, string firstName)
        {
            user.FirstName = firstName;
            _userRepository.Update(user);
        }

        public void ChangePassword(User user, string password)
        {
            user.Password = password;
            _userRepository.Update(user);
        }

        public string SetTemporaryPassword(User user)
        {
            string pass = "";
            var r = new Random();
            while (pass.Length < 8)
            {
                Char c = (char)r.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    pass += c;
            }

            user.Password = pass;
            _userRepository.Update(user);

            MailAddress from = new MailAddress("loanportfoliohse@gmail.com", "Кредитный портфель");
            MailAddress to = new MailAddress(user.Email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Восстановления пароля - Кредитный портфель";
            m.Body = $"<p>Ваш новый пароль для кредитного портфеля: {pass}</p>";
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailLogin"],
                ConfigurationManager.AppSettings["EmailPassword"]);
            smtp.EnableSsl = true;
            smtp.Send(m);

            return pass;
        }

        public void ChangeEmail(User user, string email)
        {
            if (_userRepository.All().Any(x => x.Email == email))
                throw new ArgumentException("User with this email already exists!");

            user.Email = email;
            _userRepository.Update(user);
        }

        public void Remove(User user)
        {
            _userRepository.Remove(user);
        }
    }
}
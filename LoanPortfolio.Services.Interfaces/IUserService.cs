using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using LoanPortfolio.Db.Entities;

namespace LoanPortfolio.Services.Interfaces
{
    public interface IUserService
    {
        User Add(string email, string pass, string firstName, string lastName);

        void Remove(User user);

        User GetById(int id);

        IEnumerable<User> GetAll();

        void ChangeEmail(User user, string email);

        void ChangePassword(User user, string password);

        void ChangeFirstName(User user, string firstName);

        void ChangeLastName(User user, string lastName);

        string SetTemporaryPassword(User user);
    }
}
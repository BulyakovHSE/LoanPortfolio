using LoanPortfolio.Db.Entities;

namespace LoanPortfolio.Services.Interfaces
{
    public interface IAuthService
    {
        bool IsLoggedIn();

        User Login(string username, string password, bool isPersistent);

        void Logout();
    }
}
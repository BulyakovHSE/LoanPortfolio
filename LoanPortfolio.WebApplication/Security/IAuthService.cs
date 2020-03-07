using System.Security.Principal;
using System.Web;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.WebApplication.Security
{
    public interface IAuthService
    {
        HttpContext HttpContext { get; set; }

        IUserService _userService { get; set; }

        User Login(string username, string password, bool isPersistent);

        void Logout();

        IPrincipal CurrentUser { get; }
    }
}
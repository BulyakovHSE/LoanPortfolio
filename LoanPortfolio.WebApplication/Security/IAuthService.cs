using System.Security.Principal;
using System.Web;
using LoanPortfolio.Db.Entities;

namespace LoanPortfolio.WebApplication.Security
{
    public interface IAuthService
    {
        HttpContext HttpContext { get; set; }

        User Login(string username, string password, bool isPersistent);

        void Logout();

        IPrincipal CurrentUser { get; }
    }
}
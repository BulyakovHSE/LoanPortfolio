using LoanPortfolio.Db.Entities;

namespace LoanPortfolio.WebApplication.Security
{
    public interface IUserProvider
    {
        User User { get; set; }
    }
}
using LoanPortfolio.Db.Interfaces;

namespace LoanPortfolio.Db.Entities
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}
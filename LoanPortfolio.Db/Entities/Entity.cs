using System.ComponentModel.DataAnnotations;
using LoanPortfolio.Db.Interfaces;

namespace LoanPortfolio.Db.Entities
{
    public abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
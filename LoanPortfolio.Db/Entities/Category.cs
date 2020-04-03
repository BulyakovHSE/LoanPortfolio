using System.ComponentModel.DataAnnotations;

namespace LoanPortfolio.Db.Entities
{
    /// <summary>
    /// Категория личного расхода
    /// </summary>
    public class Category : Entity
    {
        public User User { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
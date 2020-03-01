namespace LoanPortfolio.Db.Entities
{
    /// <summary>
    /// Категория личного расхода
    /// </summary>
    public class Category : Entity
    {
        public User User { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }
    }
}
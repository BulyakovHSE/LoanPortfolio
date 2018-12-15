namespace LoanPortfolio.Db.Entities
{
    public abstract class Income : Entity
    {
        public User User { get; set; }

        public int UserId => User?.Id ?? -1;

        public string IncomeSource { get; set; }

        public float Sum { get; set; }
    }
}
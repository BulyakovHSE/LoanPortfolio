using System.Linq;

namespace LoanPortfolio.Db.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        int Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        IQueryable<TEntity> All();
    }
}
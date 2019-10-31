using System.Linq;

namespace LoanPortfolio.Db.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        IQueryable<TEntity> All();
    }
}
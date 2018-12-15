using System.Data.Entity;
using System.Linq;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;

namespace LoanPortfolio.Db.Repositories
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _set;

        public void Add(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<TEntity> All()
        {
            throw new System.NotImplementedException();
        }
    }
}
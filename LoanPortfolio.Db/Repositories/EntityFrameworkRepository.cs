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

        public EntityFrameworkRepository(DbContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _set.Add(entity);
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            _set.Remove(entity);
            _context.SaveChanges();
        }

        public IQueryable<TEntity> All()
        {
            return _set;
        }
    }
}
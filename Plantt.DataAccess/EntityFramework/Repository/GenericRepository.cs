using Microsoft.EntityFrameworkCore;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Repository;

namespace Plantt.DataAccess.EntityFramework.Repository
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly PlanttDBContext _context;

        public GenericRepository(PlanttDBContext context)
        {
            _context = context;
        }

        public virtual TEntity? GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public virtual IEnumerable<TEntity> Getall()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public virtual void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual void Delete(int id)
        {
            TEntity? entity = GetById(id);
            if (entity is not null)
            {
                Delete(entity);
            }
            else
            {
                throw new NullReferenceException($"The entity with {id} was not found, and could therefore not be deleted");
            }
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public virtual IEnumerable<TEntity> Where(Func<TEntity, bool> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }
    }
}

using Demo.Domain;
using Demo.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Persistence
{
    public class GenericRepository<TEntity,TKey>: IGenericRepository<TEntity,TKey>, IDisposable 
        where TEntity : DomainEntity<Guid>
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddRange(List<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public void Dispose()
        {
             _context?.Dispose();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity>items =_context.Set<TEntity>();
            if(includeProperties != null)
            {
                foreach(var includeProperty in includeProperties)
                {
                    items=items.Include(includeProperty);
                }
            }
            if(predicate != null)
            {
                items=items.Where(predicate);
            }
            return items;
        }

        public async Task<TEntity?> FindByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await FindAll(s => s.Id.Equals(id), includeProperties).FirstOrDefaultAsync();
        }

        public async Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await FindAll(null,includeProperties).AsTracking().FirstOrDefaultAsync(predicate);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity); 
        }

        public void RemoveMultiple(List<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain
{
    public interface IGenericRepository<TEntity, Tkey> where TEntity : class
    {
        Task<TEntity?> FindByIdAsync(Tkey id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null,
            params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> FindAll(Expression<Func<TEntity,bool>>? predicate=null,
            params Expression<Func<TEntity, object>>[] includeProperties);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void AddRange(List<TEntity> entities);
        void Update(TEntity entity);
        void RemoveMultiple(List<TEntity> entities);

    }
}

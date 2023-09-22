using Desafio.Domain.Models;
using System.Linq.Expressions;

namespace Desafio.Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Add(TEntity entity);
        Task<TEntity> GetById(int id);
        Task<List<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
        void Update(TEntity entity);
        void Remove(int id);
        Task<List<TEntity>> Search(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? pageSize = null,
            int? index = null);
        Task<bool> Commit();
    }
}

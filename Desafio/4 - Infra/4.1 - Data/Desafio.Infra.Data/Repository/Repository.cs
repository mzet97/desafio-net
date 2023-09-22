using Desafio.Domain.Interfaces;
using Desafio.Domain.Models;
using Desafio.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Desafio.Infra.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly DataIdentityDbContext Db;
        protected readonly DbSet<TEntity> DbSet;
        protected int Count;

        protected Repository(DataIdentityDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
            Count = DbSet.AsQueryable().Count();
        }

        public async Task Add(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void Dispose()
        {
            Db?.Dispose();
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual void Remove(int id)
        {
            var entity = DbSet.Find(id);
            DbSet.Remove(entity);
        }

        public virtual async Task<bool> Commit()
        {
            var result = await Db.SaveChangesAsync();
            return await Task.FromResult(result > 0);
        }

        public virtual async Task<List<TEntity>> Search(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? pageSize = null,
            int? pageIndex = null)
        {
            var query = DbSet.AsQueryable();
            Count = query.Count();
            int pages = 0;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (pageSize != null && pageSize.HasValue && pageSize > 0)
            {
                pages = Count / pageSize.Value;

                if (pageIndex != null && pageIndex.HasValue && pageIndex.Value > 0)
                {
                    if (pageIndex.Value > pages)
                    {
                        query = query.Skip(pageSize.Value * pages).Take(pageSize.Value);
                    }
                    else
                    {
                        query = query.Skip(pageSize.Value * pageIndex.Value).Take(pageSize.Value);
                    }

                }
                else
                {
                    query = query.Skip(pageSize.Value);
                }

            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }
    }
}

using Desafio.Domain.Interfaces.Repository;
using Desafio.Domain.Models;
using Desafio.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Desafio.Infra.Data.Repository
{
    public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(DataIdentityDbContext db) : base(db)
        {
        }

        public override async Task<List<Pessoa>> Search(
            Expression<Func<Pessoa, bool>> predicate = null,
            Func<IQueryable<Pessoa>, IOrderedQueryable<Pessoa>> orderBy = null,
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
                        query = query.OrderBy(x => x.Id).Skip(pageSize.Value * pages).Take(pageSize.Value);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Id).Skip(pageSize.Value * pageIndex.Value).Take(pageSize.Value);
                    }

                }
                else
                {
                    query = query.OrderBy(x => x.Id).Skip(pageSize.Value);
                }

            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }
    }
}

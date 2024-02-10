using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Sidkenu.Dominio.Entidades.Base;
using System.Linq.Expressions;

namespace Sidkenu.Dominio.Repositorio
{
    public class RepositoryGenericLectura<T> : IRepositoryGenericLectura<T> where T : EntidadBase
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _entities;

        public RepositoryGenericLectura(DbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        // ================================================================================== //
        // =========================          Consultas            ========================== //
        // ================================================================================== //

        public virtual T GetById(Guid id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<T> query = _entities;

            if (enableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            return query.FirstOrDefault(x => x.Id == id);
        }

        public virtual IEnumerable<T> GetByIds(List<Guid> ids,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<T> query = _entities;

            if (enableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            query = query.Where(x => ids == null || ids.Contains(x.Id));

            return query.ToList();
        }

        public virtual IEnumerable<T> GetByFilter(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<T> query = _entities;

            if (enableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null
            ? orderBy(query).ToList()
            : query.ToList();
        }

        public virtual IEnumerable<T> GetByFilterTake(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true, 
            int take = 1000)
        {
            IQueryable<T> query = _entities;

            if (enableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = query.Take(take);

            return orderBy != null
            ? orderBy(query).ToList()
            : query.ToList();
        }

        public virtual IEnumerable<T> GetByFilterIgnoreQueryFilter(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<T> query = _entities;

            query = query.IgnoreQueryFilters();

            if (enableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null
                ? orderBy(query).ToList()
                : query.ToList();
        }

        public virtual IEnumerable<T> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _entities;

            query = query.AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            return query.ToList();
        }
    }
}

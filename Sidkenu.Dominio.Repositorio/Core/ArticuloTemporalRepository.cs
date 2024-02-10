using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using System.Linq.Expressions;

namespace Sidkenu.Dominio.Repositorio.Core
{
    public class ArticuloTemporalRepository : IArticuloTemporalRepository
    {
        protected readonly DbContext _context;
        public ArticuloTemporalRepository(DbContext context)
        {
            _context = context;
        }

        // ================================================================================== //
        // =========================         Persistencia          ========================== //
        // ================================================================================== //

        public virtual void Add(ArticuloTemporal entity)
        {
            _context.Set<ArticuloBase>().Add(entity);
        }

        public virtual void Update(ArticuloTemporal entity)
        {
            _context.Set<ArticuloBase>().Update(entity);
        }

        

        public virtual void DeleteFisico(Guid id, string userLogin)
        {
            var entity = GetById(id);

            _context.Set<ArticuloBase>().Remove(entity);
        }
        
        public virtual void Delete(Guid id, string userLogin)
        {
            var entity = GetById(id);

            entity.EstaEliminado = !entity.EstaEliminado;
            entity.User = userLogin;

            Update(entity);
        }

        public virtual ArticuloTemporal GetById(Guid id,
            Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ArticuloTemporal> query = _context.Set<ArticuloBase>().OfType<ArticuloTemporal>();

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

        public virtual IEnumerable<ArticuloTemporal> GetByIds(List<Guid> ids,
            Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ArticuloTemporal> query = _context.Set<ArticuloBase>().OfType<ArticuloTemporal>();

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

        public virtual IEnumerable<ArticuloTemporal> GetByFilter(Expression<Func<ArticuloTemporal, bool>> predicate = null,
            Func<IQueryable<ArticuloTemporal>, IOrderedQueryable<ArticuloTemporal>> orderBy = null,
            Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ArticuloTemporal> query = _context.Set<ArticuloBase>().OfType<ArticuloTemporal>();

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

        public virtual IEnumerable<ArticuloTemporal> GetByFilterTake(Expression<Func<ArticuloTemporal, bool>> predicate = null,
            Func<IQueryable<ArticuloTemporal>, IOrderedQueryable<ArticuloTemporal>> orderBy = null,
            Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null,
            bool enableTracking = true,
            int take = 1000)
        {
            IQueryable<ArticuloTemporal> query = _context.Set<ArticuloBase>().OfType<ArticuloTemporal>();

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

        public virtual IEnumerable<ArticuloTemporal> GetByFilterIgnoreQueryFilter(Expression<Func<ArticuloTemporal, bool>> predicate = null,
            Func<IQueryable<ArticuloTemporal>, IOrderedQueryable<ArticuloTemporal>> orderBy = null,
            Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ArticuloTemporal> query = _context.Set<ArticuloBase>().OfType<ArticuloTemporal>();

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

        public virtual IEnumerable<ArticuloTemporal> GetAll(Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null)
        {
            IQueryable<ArticuloTemporal> query = _context.Set<ArticuloBase>().OfType<ArticuloTemporal>();

            query = query.AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            return query.ToList();
        }
    }
}

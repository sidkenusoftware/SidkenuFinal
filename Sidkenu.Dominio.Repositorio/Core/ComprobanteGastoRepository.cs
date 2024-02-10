using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using System.Linq.Expressions;

namespace Sidkenu.Dominio.Repositorio.Core
{
    public class ComprobanteGastoRepository : IComprobanteGastoRepository
    {
        protected readonly DbContext _context;

        public ComprobanteGastoRepository(DbContext context)
        {
            _context = context;
        }

        // ================================================================================== //
        // =========================         Persistencia          ========================== //
        // ================================================================================== //

        public virtual void Add(ComprobanteGasto entity)
        {
            _context.Set<Comprobante>().Add(entity);
        }

        public virtual void Update(ComprobanteGasto entity)
        {
            _context.Set<Comprobante>().Update(entity);
        }

        public virtual void DeleteFisico(Guid id, string userLogin)
        {
            var entity = GetById(id);

            _context.Set<Comprobante>().Remove(entity);
        }

        public virtual void Delete(Guid id, string userLogin)
        {
            var entity = GetById(id);

            entity.EstaEliminado = !entity.EstaEliminado;
            entity.User = userLogin;

            Update(entity);
        }

        public virtual ComprobanteGasto GetById(Guid id,
            Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteGasto> query = _context.Set<Comprobante>().OfType<ComprobanteGasto>();

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

        public virtual IEnumerable<ComprobanteGasto> GetByIds(List<Guid> ids,
            Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteGasto> query = _context.Set<Comprobante>().OfType<ComprobanteGasto>();

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

        public virtual IEnumerable<ComprobanteGasto> GetByFilter(Expression<Func<ComprobanteGasto, bool>> predicate = null,
            Func<IQueryable<ComprobanteGasto>, IOrderedQueryable<ComprobanteGasto>> orderBy = null,
            Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteGasto> query = _context.Set<Comprobante>().OfType<ComprobanteGasto>();

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

        public virtual IEnumerable<ComprobanteGasto> GetByFilterTake(Expression<Func<ComprobanteGasto, bool>> predicate = null,
            Func<IQueryable<ComprobanteGasto>, IOrderedQueryable<ComprobanteGasto>> orderBy = null,
            Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null,
            bool enableTracking = true,
            int take = 1000)
        {
            IQueryable<ComprobanteGasto> query = _context.Set<Comprobante>().OfType<ComprobanteGasto>();

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

        public virtual IEnumerable<ComprobanteGasto> GetByFilterIgnoreQueryFilter(Expression<Func<ComprobanteGasto, bool>> predicate = null,
            Func<IQueryable<ComprobanteGasto>, IOrderedQueryable<ComprobanteGasto>> orderBy = null,
            Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteGasto> query = _context.Set<Comprobante>().OfType<ComprobanteGasto>();

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

        public virtual IEnumerable<ComprobanteGasto> GetAll(Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null)
        {
            IQueryable<ComprobanteGasto> query = _context.Set<Comprobante>().OfType<ComprobanteGasto>();

            query = query.AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            return query.ToList();
        }        
    }
}

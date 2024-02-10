using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using System.Linq.Expressions;

namespace Sidkenu.Dominio.Repositorio.Core
{
    public class ComprobanteVentaRepository : IComprobanteVentaRepository
    {
        protected readonly DbContext _context;

        public ComprobanteVentaRepository(DbContext context)
        {
            _context = context;
        }

        // ================================================================================== //
        // =========================         Persistencia          ========================== //
        // ================================================================================== //

        public virtual void Add(ComprobanteVenta entity)
        {
            _context.Set<Comprobante>().Add(entity);
        }

        public virtual void Update(ComprobanteVenta entity)
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

        public virtual ComprobanteVenta GetById(Guid id,
            Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteVenta> query = _context.Set<Comprobante>().OfType<ComprobanteVenta>();

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

        public virtual IEnumerable<ComprobanteVenta> GetByIds(List<Guid> ids,
            Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteVenta> query = _context.Set<Comprobante>().OfType<ComprobanteVenta>();

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

        public virtual IEnumerable<ComprobanteVenta> GetByFilter(Expression<Func<ComprobanteVenta, bool>> predicate = null,
            Func<IQueryable<ComprobanteVenta>, IOrderedQueryable<ComprobanteVenta>> orderBy = null,
            Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteVenta> query = _context.Set<Comprobante>().OfType<ComprobanteVenta>();

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

        public virtual IEnumerable<ComprobanteVenta> GetByFilterTake(Expression<Func<ComprobanteVenta, bool>> predicate = null,
            Func<IQueryable<ComprobanteVenta>, IOrderedQueryable<ComprobanteVenta>> orderBy = null,
            Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null,
            bool enableTracking = true,
            int take = 1000)
        {
            IQueryable<ComprobanteVenta> query = _context.Set<Comprobante>().OfType<ComprobanteVenta>();

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

        public virtual IEnumerable<ComprobanteVenta> GetByFilterIgnoreQueryFilter(Expression<Func<ComprobanteVenta, bool>> predicate = null,
            Func<IQueryable<ComprobanteVenta>, IOrderedQueryable<ComprobanteVenta>> orderBy = null,
            Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteVenta> query = _context.Set<Comprobante>().OfType<ComprobanteVenta>();

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

        public virtual IEnumerable<ComprobanteVenta> GetAll(Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null)
        {
            IQueryable<ComprobanteVenta> query = _context.Set<Comprobante>().OfType<ComprobanteVenta>();

            query = query.AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            return query.ToList();
        }
    }
}

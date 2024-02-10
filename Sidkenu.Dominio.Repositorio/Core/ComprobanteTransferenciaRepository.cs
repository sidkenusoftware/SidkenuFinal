using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sidkenu.Dominio.Repositorio.Core
{
    public class ComprobanteTransferenciaRepository : IComprobanteTransferenciaRepository
    {
        protected readonly DbContext _context;

        public ComprobanteTransferenciaRepository(DbContext context)
        {
            _context = context;
        }

        // ================================================================================== //
        // =========================         Persistencia          ========================== //
        // ================================================================================== //

        public virtual void Add(ComprobanteTransferencia entity)
        {
            _context.Set<Comprobante>().Add(entity);
        }

        public virtual void Update(ComprobanteTransferencia entity)
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

        public virtual ComprobanteTransferencia GetById(Guid id,
            Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteTransferencia> query = _context.Set<Comprobante>().OfType<ComprobanteTransferencia>();

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

        public virtual IEnumerable<ComprobanteTransferencia> GetByIds(List<Guid> ids,
            Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteTransferencia> query = _context.Set<Comprobante>().OfType<ComprobanteTransferencia>();

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

        public virtual IEnumerable<ComprobanteTransferencia> GetByFilter(Expression<Func<ComprobanteTransferencia, bool>> predicate = null,
            Func<IQueryable<ComprobanteTransferencia>, IOrderedQueryable<ComprobanteTransferencia>> orderBy = null,
            Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteTransferencia> query = _context.Set<Comprobante>().OfType<ComprobanteTransferencia>();

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

        public virtual IEnumerable<ComprobanteTransferencia> GetByFilterTake(Expression<Func<ComprobanteTransferencia, bool>> predicate = null,
            Func<IQueryable<ComprobanteTransferencia>, IOrderedQueryable<ComprobanteTransferencia>> orderBy = null,
            Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null,
            bool enableTracking = true,
            int take = 1000)
        {
            IQueryable<ComprobanteTransferencia> query = _context.Set<Comprobante>().OfType<ComprobanteTransferencia>();

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

        public virtual IEnumerable<ComprobanteTransferencia> GetByFilterIgnoreQueryFilter(Expression<Func<ComprobanteTransferencia, bool>> predicate = null,
            Func<IQueryable<ComprobanteTransferencia>, IOrderedQueryable<ComprobanteTransferencia>> orderBy = null,
            Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<ComprobanteTransferencia> query = _context.Set<Comprobante>().OfType<ComprobanteTransferencia>();

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

        public virtual IEnumerable<ComprobanteTransferencia> GetAll(Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null)
        {
            IQueryable<ComprobanteTransferencia> query = _context.Set<Comprobante>().OfType<ComprobanteTransferencia>();

            query = query.AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            return query.ToList();
        }
    }
}

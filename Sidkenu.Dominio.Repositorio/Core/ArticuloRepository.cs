using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using System.Linq.Expressions;

namespace Sidkenu.Dominio.Repositorio.Core
{
    public class ArticuloRepository : IArticuloRepository
    {
        protected readonly DbContext _context;
        public ArticuloRepository(DbContext context)
        {
            _context = context;
        }

        // ================================================================================== //
        // =========================         Persistencia          ========================== //
        // ================================================================================== //

        public virtual void Add(Articulo entity)
        {
            _context.Set<ArticuloBase>().Add(entity);
        }

        public virtual void Update(Articulo entity)
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

        public virtual Articulo GetById(Guid id,
            Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<Articulo> query = _context.Set<ArticuloBase>().OfType<Articulo>();

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

        public virtual IEnumerable<Articulo> GetByIds(List<Guid> ids,
            Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<Articulo> query = _context.Set<ArticuloBase>().OfType<Articulo>();

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

        public virtual IEnumerable<Articulo> GetByFilter(Expression<Func<Articulo, bool>> predicate = null,
            Func<IQueryable<Articulo>, IOrderedQueryable<Articulo>> orderBy = null,
            Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<Articulo> query = _context.Set<ArticuloBase>().OfType<Articulo>();

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

        public virtual IEnumerable<Articulo> GetByFilterTake(Expression<Func<Articulo, bool>> predicate = null,
            Func<IQueryable<Articulo>, IOrderedQueryable<Articulo>> orderBy = null,
            Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null,
            bool enableTracking = true,
            int take = 1000)
        {
            IQueryable<Articulo> query = _context.Set<ArticuloBase>().OfType<Articulo>();

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

        public virtual IEnumerable<Articulo> GetByFilterIgnoreQueryFilter(Expression<Func<Articulo, bool>> predicate = null,
            Func<IQueryable<Articulo>, IOrderedQueryable<Articulo>> orderBy = null,
            Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<Articulo> query = _context.Set<ArticuloBase>().OfType<Articulo>();

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

        public virtual IEnumerable<Articulo> GetAll(Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null)
        {
            IQueryable<Articulo> query = _context.Set<ArticuloBase>().OfType<Articulo>();

            query = query.AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            return query.ToList();
        }
    }
}

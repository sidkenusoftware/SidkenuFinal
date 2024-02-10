using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Sidkenu.Dominio.Entidades.Base;
using System.Linq.Expressions;

namespace Sidkenu.Dominio.Repositorio
{
    public class RepositoryGeneric<T> : IRepositoryGeneric<T> where T : EntidadBase
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _entities;

        public RepositoryGeneric(DbContext context)
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

            return _entities.Local.Any(x => x.Id == id)
                ? _entities.Local.FirstOrDefault(x => x.Id == id)
                : query.FirstOrDefault(x => x.Id == id);
        }

        public virtual IEnumerable<T> GetByIds(List<Guid> ids,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true)
        {

            List<T> values = new();

            foreach (var id in ids) 
            {
                var entity = GetById(id, include, enableTracking);

                values.Add(entity);
            }            

            return values.ToList();

            //IQueryable<T> query = _entities;

            //if (enableTracking)
            //{
            //    query = query.AsNoTracking();
            //}

            //if (include != null)
            //{
            //    query = include(query);
            //}

            //query = query.Where(x => ids == null || ids.Contains(x.Id));

            //return query.ToList();
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

        // ================================================================================== //
        // =========================         Persistencia          ========================== //
        // ================================================================================== //

        public virtual void Add(T entity)
        {
            _entities.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _entities.Update(entity);
        }

        public virtual void Delete(Guid id, string userLogin)
        {
            var entity = GetById(id);

            entity.EstaEliminado = !entity.EstaEliminado;
            entity.User = userLogin;

            _entities.Update(entity);
        }

        public virtual void DeleteFisico(Guid id, string userLogin)
        {
            var entity = GetById(id);
            _entities.Remove(entity);
        }
    }
}

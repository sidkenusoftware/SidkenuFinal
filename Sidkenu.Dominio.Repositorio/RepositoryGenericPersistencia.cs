using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Repositorio
{
    public class RepositoryGenericPersistencia<T> : IRepositoryGenericPersistencia<T> where T : EntidadBase
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _entities;

        public RepositoryGenericPersistencia(DbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
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

        private T GetById(Guid id,
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

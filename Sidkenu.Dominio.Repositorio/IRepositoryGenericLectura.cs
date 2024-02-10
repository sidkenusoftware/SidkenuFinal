using Microsoft.EntityFrameworkCore.Query;
using Sidkenu.Dominio.Entidades.Base;
using System.Linq.Expressions;

namespace Sidkenu.Dominio.Repositorio
{
    public interface IRepositoryGenericLectura<T> where T : EntidadBase
    {
        // Consultas
        T GetById(Guid id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true);

        IEnumerable<T> GetByIds(List<Guid> ids,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true);

        IEnumerable<T> GetByFilter(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true);

        IEnumerable<T> GetByFilterTake(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true,
            int take = 1000);

        IEnumerable<T> GetByFilterIgnoreQueryFilter(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true);

        IEnumerable<T> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    }
}

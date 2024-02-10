using Microsoft.EntityFrameworkCore.Query;
using Sidkenu.Dominio.Entidades.Core;
using System.Linq.Expressions;

namespace Sidkenu.Dominio.Repositorio.Core
{
    public interface IArticuloRepository
    {
        void Add(Articulo entity);

        void Update(Articulo entity);

        void DeleteFisico(Guid id, string userLogin);

        void Delete(Guid id, string userLogin);

        Articulo GetById(Guid id,
            Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null,
            bool enableTracking = true);

        IEnumerable<Articulo> GetByIds(List<Guid> ids,
            Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null,
            bool enableTracking = true);

        IEnumerable<Articulo> GetByFilter(Expression<Func<Articulo, bool>> predicate = null,
            Func<IQueryable<Articulo>, IOrderedQueryable<Articulo>> orderBy = null,
            Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null,
            bool enableTracking = true);

        IEnumerable<Articulo> GetByFilterTake(Expression<Func<Articulo, bool>> predicate = null,
            Func<IQueryable<Articulo>, IOrderedQueryable<Articulo>> orderBy = null,
            Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null,
            bool enableTracking = true,
            int take = 1000);

        IEnumerable<Articulo> GetByFilterIgnoreQueryFilter(Expression<Func<Articulo, bool>> predicate = null,
            Func<IQueryable<Articulo>, IOrderedQueryable<Articulo>> orderBy = null,
            Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null,
            bool enableTracking = true);

        IEnumerable<Articulo> GetAll(Func<IQueryable<Articulo>, IIncludableQueryable<Articulo, object>> include = null);
    }
}

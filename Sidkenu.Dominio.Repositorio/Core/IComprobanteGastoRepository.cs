using Microsoft.EntityFrameworkCore.Query;
using Sidkenu.Dominio.Entidades.Core;
using System.Linq.Expressions;

namespace Sidkenu.Dominio.Repositorio.Core
{
    public interface IComprobanteGastoRepository
    {
        void Add(ComprobanteGasto entity);

        void Update(ComprobanteGasto entity);

        void DeleteFisico(Guid id, string userLogin);

        void Delete(Guid id, string userLogin);

        ComprobanteGasto GetById(Guid id,
            Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteGasto> GetByIds(List<Guid> ids,
            Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteGasto> GetByFilter(Expression<Func<ComprobanteGasto, bool>> predicate = null,
            Func<IQueryable<ComprobanteGasto>, IOrderedQueryable<ComprobanteGasto>> orderBy = null,
            Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteGasto> GetByFilterTake(Expression<Func<ComprobanteGasto, bool>> predicate = null,
            Func<IQueryable<ComprobanteGasto>, IOrderedQueryable<ComprobanteGasto>> orderBy = null,
            Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null,
            bool enableTracking = true,
            int take = 1000);

        IEnumerable<ComprobanteGasto> GetByFilterIgnoreQueryFilter(Expression<Func<ComprobanteGasto, bool>> predicate = null,
            Func<IQueryable<ComprobanteGasto>, IOrderedQueryable<ComprobanteGasto>> orderBy = null,
            Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteGasto> GetAll(Func<IQueryable<ComprobanteGasto>, IIncludableQueryable<ComprobanteGasto, object>> include = null);
    }
}
using Microsoft.EntityFrameworkCore.Query;
using Sidkenu.Dominio.Entidades.Core;
using System.Linq.Expressions;

namespace Sidkenu.Dominio.Repositorio.Core
{
    public interface IComprobanteVentaRepository
    {
        void Add(ComprobanteVenta entity);

        void Update(ComprobanteVenta entity);

        void DeleteFisico(Guid id, string userLogin);

        void Delete(Guid id, string userLogin);

        ComprobanteVenta GetById(Guid id,
            Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteVenta> GetByIds(List<Guid> ids,
            Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteVenta> GetByFilter(Expression<Func<ComprobanteVenta, bool>> predicate = null,
            Func<IQueryable<ComprobanteVenta>, IOrderedQueryable<ComprobanteVenta>> orderBy = null,
            Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteVenta> GetByFilterTake(Expression<Func<ComprobanteVenta, bool>> predicate = null,
            Func<IQueryable<ComprobanteVenta>, IOrderedQueryable<ComprobanteVenta>> orderBy = null,
            Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null,
            bool enableTracking = true,
            int take = 1000);

        IEnumerable<ComprobanteVenta> GetByFilterIgnoreQueryFilter(Expression<Func<ComprobanteVenta, bool>> predicate = null,
            Func<IQueryable<ComprobanteVenta>, IOrderedQueryable<ComprobanteVenta>> orderBy = null,
            Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteVenta> GetAll(Func<IQueryable<ComprobanteVenta>, IIncludableQueryable<ComprobanteVenta, object>> include = null);
    }
}
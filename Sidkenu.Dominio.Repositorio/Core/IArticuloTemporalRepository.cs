using Microsoft.EntityFrameworkCore.Query;
using Sidkenu.Dominio.Entidades.Core;
using System.Linq.Expressions;

namespace Sidkenu.Dominio.Repositorio.Core
{
    public interface IArticuloTemporalRepository
    {
        void Add(ArticuloTemporal entity);

        void Update(ArticuloTemporal entity);

        void DeleteFisico(Guid id, string userLogin);

        void Delete(Guid id, string userLogin);

        ArticuloTemporal GetById(Guid id,
            Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ArticuloTemporal> GetByIds(List<Guid> ids,
            Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ArticuloTemporal> GetByFilter(Expression<Func<ArticuloTemporal, bool>> predicate = null,
            Func<IQueryable<ArticuloTemporal>, IOrderedQueryable<ArticuloTemporal>> orderBy = null,
            Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ArticuloTemporal> GetByFilterTake(Expression<Func<ArticuloTemporal, bool>> predicate = null,
            Func<IQueryable<ArticuloTemporal>, IOrderedQueryable<ArticuloTemporal>> orderBy = null,
            Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null,
            bool enableTracking = true,
            int take = 1000);

        IEnumerable<ArticuloTemporal> GetByFilterIgnoreQueryFilter(Expression<Func<ArticuloTemporal, bool>> predicate = null,
            Func<IQueryable<ArticuloTemporal>, IOrderedQueryable<ArticuloTemporal>> orderBy = null,
            Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ArticuloTemporal> GetAll(Func<IQueryable<ArticuloTemporal>, IIncludableQueryable<ArticuloTemporal, object>> include = null);
    }
}

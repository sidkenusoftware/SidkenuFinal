using Microsoft.EntityFrameworkCore.Query;
using Sidkenu.Dominio.Entidades.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sidkenu.Dominio.Repositorio.Core
{
    public interface IComprobanteTransferenciaRepository
    {
        void Add(ComprobanteTransferencia entity);

        void Update(ComprobanteTransferencia entity);

        void DeleteFisico(Guid id, string userLogin);

        void Delete(Guid id, string userLogin);

        ComprobanteTransferencia GetById(Guid id,
            Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteTransferencia> GetByIds(List<Guid> ids,
            Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteTransferencia> GetByFilter(Expression<Func<ComprobanteTransferencia, bool>> predicate = null,
            Func<IQueryable<ComprobanteTransferencia>, IOrderedQueryable<ComprobanteTransferencia>> orderBy = null,
            Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteTransferencia> GetByFilterTake(Expression<Func<ComprobanteTransferencia, bool>> predicate = null,
            Func<IQueryable<ComprobanteTransferencia>, IOrderedQueryable<ComprobanteTransferencia>> orderBy = null,
            Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null,
            bool enableTracking = true,
            int take = 1000);

        IEnumerable<ComprobanteTransferencia> GetByFilterIgnoreQueryFilter(Expression<Func<ComprobanteTransferencia, bool>> predicate = null,
            Func<IQueryable<ComprobanteTransferencia>, IOrderedQueryable<ComprobanteTransferencia>> orderBy = null,
            Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null,
            bool enableTracking = true);

        IEnumerable<ComprobanteTransferencia> GetAll(Func<IQueryable<ComprobanteTransferencia>, IIncludableQueryable<ComprobanteTransferencia, object>> include = null);
    }
}

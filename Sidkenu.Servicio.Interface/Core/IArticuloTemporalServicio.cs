using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Articulo;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IArticuloTemporalServicio
    {
        ResultDTO Add(ArticuloTemporalPersistenciaDTO entidad, string user);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetByFilter(ArticuloFilterDTO filter);
        ResultDTO GetByFilterLookUp(ArticuloFilterDTO filter);
        ResultDTO GetById(Guid id, Guid empresaId);
    }
}

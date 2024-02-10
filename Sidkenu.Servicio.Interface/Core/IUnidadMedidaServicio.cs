using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.UnidadMedida;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IUnidadMedidaServicio
    {
        ResultDTO Add(UnidadMedidaPersistenciaDTO entidad, string user);
        ResultDTO Update(UnidadMedidaPersistenciaDTO entidad, string user);
        ResultDTO Delete(UnidadMedidaDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(UnidadMedidaFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();
    }
}

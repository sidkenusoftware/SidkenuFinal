using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.OrdenFabricacion;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IOrdenFabricacionServicio
    {
        ResultDTO GetByFilter(OrdenFabricacionFilterDTO filter);

        ResultDTO Add(OrdenFabricacionPersistenciaDTO entidad, string user);

        ResultDTO CambiarEstadoEnProceso(Guid OrdenFabricacionId, string user);

        ResultDTO CancelarOrdenFabricacion(Guid OrdenFabricacionId, string user);

        ResultDTO FinalizarOrdenFabricacion(Guid OrdenFabricacionId, string user);
    }
}

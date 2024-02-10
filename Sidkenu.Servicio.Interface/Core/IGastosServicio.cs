using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Gasto;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IGastosServicio
    {
        ResultDTO Add(GastosPersistenciaDTO entidad, string user);
    }
}

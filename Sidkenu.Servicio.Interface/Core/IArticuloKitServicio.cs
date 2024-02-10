using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ArticuloKit;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IArticuloKitServicio
    {
        ResultDTO Add(ArticuloKitPersistenciaDTO articuloKit, string user);
    }
}

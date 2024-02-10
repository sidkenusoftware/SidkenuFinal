using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ArticuloPrecio;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IArticuloPrecioServicio
    {
        ResultDTO AddOrUpdate(ArticuloPrecioPersistenciaDTO articuloPrecioDTO, string user);

        ResultDTO AddOrUpdate(ArticuloPrecioPersistenciaDTO articuloPrecioDTO, Articulo articuloNuevo, string user);
    }
}

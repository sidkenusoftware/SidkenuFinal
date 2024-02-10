using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Pedido;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IPedidoServicio
    {
        ResultDTO Add(PedidoPersistenciaDTO entidad, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(PedidoFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();
    }
}

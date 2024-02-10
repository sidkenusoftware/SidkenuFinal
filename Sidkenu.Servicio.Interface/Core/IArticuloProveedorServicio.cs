using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IArticuloProveedorServicio
    {
        ResultDTO GetAll(Guid? articuloId);

        ResultDTO GetArticulosSugeridos(Guid? proveedorId, Guid empresaId);
    }
}

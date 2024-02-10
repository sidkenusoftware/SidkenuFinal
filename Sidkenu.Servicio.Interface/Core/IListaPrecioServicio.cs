using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ListaPrecio;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IListaPrecioServicio
    {
        ResultDTO Add(ListaPrecioPersistenciaDTO entidad, string user);
        ResultDTO Update(ListaPrecioPersistenciaDTO entidad, string user);
        ResultDTO Delete(ListaPrecioDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(ListaPrecioFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();
    }
}

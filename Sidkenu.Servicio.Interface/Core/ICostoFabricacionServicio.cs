using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.CostoFabricacion;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface ICostoFabricacionServicio
    {
        ResultDTO Add(CostoFabricacionPersistenciaDTO entidad, string user);
        ResultDTO Update(CostoFabricacionPersistenciaDTO entidad, bool actualizarPrecio, string user);
        ResultDTO Delete(CostoFabricacionDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(CostoFabricacionFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();
    }
}

using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.TipoGasto;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface ITipoGastoServicio
    {
        ResultDTO Add(TipoGastoPersistenciaDTO entidad, string user);
        ResultDTO Update(TipoGastoPersistenciaDTO entidad, string user);
        ResultDTO Delete(TipoGastoDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(TipoGastoFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();
    }
}

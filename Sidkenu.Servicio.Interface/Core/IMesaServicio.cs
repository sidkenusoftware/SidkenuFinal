using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Mesa;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IMesaServicio
    {
        ResultDTO Add(MesaPersistenciaDTO entidad, string user);
        ResultDTO Update(MesaPersistenciaDTO entidad, string user);
        ResultDTO Delete(MesaDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(MesaFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();
    }
}

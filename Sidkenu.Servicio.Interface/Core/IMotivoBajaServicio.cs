using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.MotivoBaja;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IMotivoBajaServicio
    {
        ResultDTO Add(MotivoBajaPersistenciaDTO entidad, string user);
        ResultDTO Update(MotivoBajaPersistenciaDTO entidad, string user);
        ResultDTO Delete(MotivoBajaDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(MotivoBajaFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();

    }
}

using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.PlanTarjeta;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IPlanTarjetaServicio
    {
        ResultDTO Add(PlanTarjetaPersistenciaDTO entidad, string user);
        ResultDTO Update(PlanTarjetaPersistenciaDTO entidad, string user);
        ResultDTO Delete(PlanTarjetaDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(PlanTarjetaFilterDTO filter);
        ResultDTO GetAll(Guid tarjetaId);
        ResultDTO GetAll();
    }
}

using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.PuestoTrabajo;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IPuestoTrabajoServicio
    {
        ResultDTO Add(PuestoTrabajoPersistenciaDTO entidad, string user);
        ResultDTO Update(PuestoTrabajoPersistenciaDTO entidad, string user);
        ResultDTO Delete(PuestoTrabajoDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(PuestoTrabajoFilterDTO filter);
        ResultDTO GetAll();
        ResultDTO GetByNumeroSerie(string numeroSerie, Guid empresaId);
    }
}

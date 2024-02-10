using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Persona;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IPersonaServicio
    {
        ResultDTO Add(PersonaPersistenciaDTO entidad, string user);
        ResultDTO Update(PersonaPersistenciaDTO entidad, string user);
        ResultDTO Delete(PersonaDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(PersonaFilterDTO filter);
        ResultDTO GetByFilterLookUp(PersonaFilterDTO filter);
        ResultDTO GetAll();
    }
}

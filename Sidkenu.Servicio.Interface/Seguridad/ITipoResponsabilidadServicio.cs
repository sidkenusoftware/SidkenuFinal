using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.TipoResponsabilidad;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface ITipoResponsabilidadServicio
    {
        ResultDTO Add(TipoResponsabilidadPersistenciaDTO entidad, string user);
        ResultDTO Update(TipoResponsabilidadPersistenciaDTO entidad, string user);
        ResultDTO Delete(TipoResponsabilidadDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(TipoResponsabilidadFilterDTO filter);
        ResultDTO GetAll();
    }
}

using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Localidad;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface ILocalidadServicio
    {
        ResultDTO Add(LocalidadPersistenciaDTO entidad, string user);
        ResultDTO Update(LocalidadPersistenciaDTO entidad, string user);
        ResultDTO Delete(LocalidadDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(LocalidadFilterDTO filter);
        ResultDTO GetAll();
    }
}

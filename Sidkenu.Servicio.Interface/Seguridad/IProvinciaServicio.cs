using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Provincia;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IProvinciaServicio
    {
        ResultDTO Add(ProvinciaPersistenciaDTO entidad, string user);
        ResultDTO Update(ProvinciaPersistenciaDTO entidad, string user);
        ResultDTO Delete(ProvinciaDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(ProvinciaFilterDTO filter);
        ResultDTO GetAll();
    }
}

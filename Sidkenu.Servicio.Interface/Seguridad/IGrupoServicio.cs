using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Grupo;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IGrupoServicio
    {
        ResultDTO Add(GrupoPersistenciaDTO entidad, string user);
        ResultDTO Update(GrupoPersistenciaDTO entidad, string user);
        ResultDTO Delete(GrupoDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(GrupoFilterDTO filter);
        ResultDTO GetAll();
    }
}

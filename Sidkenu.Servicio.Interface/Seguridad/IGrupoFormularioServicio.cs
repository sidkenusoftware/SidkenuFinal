using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.GrupoFormulario;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IGrupoFormularioServicio
    {
        ResultDTO GetByFormulariosAsignadas(GrupoFormularioFilterDTO filterDTO);
        ResultDTO GetByFormulariosNoAsignadas(GrupoFormularioFilterDTO filterDTO);

        ResultDTO AddFormularioGrupo(GrupoFormularioPersistenciaDTO GrupoFormularioPersistenciaDTO, string userLogin);
        ResultDTO AddFormulariosGrupo(GrupoFormulariosPersistenciaDTO GrupoFormulariosPersistenciaDTO, string userLogin);

        ResultDTO DeleteFormularioGrupo(GrupoFormularioPersistenciaDTO GrupoFormularioPersistenciaDTO, string userLogin);
        ResultDTO DeleteFormulariosGrupo(GrupoFormulariosPersistenciaDTO GrupoFormulariosPersistenciaDTO, string userLogin);
    }
}

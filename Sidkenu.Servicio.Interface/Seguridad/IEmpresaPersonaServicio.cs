using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.EmpresaPersona;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IEmpresaPersonaServicio
    {
        ResultDTO GetByPersonasAsignadas(EmpresaPersonaFilterDTO filterDTO);
        ResultDTO GetByPersonasNoAsignadas(EmpresaPersonaFilterDTO filterDTO);

        ResultDTO AddPersonaEmpresa(EmpresaPersonaPersistenciaDTO empresaPersonaPersistenciaDTO, string userLogin);
        ResultDTO AddPersonasEmpresa(EmpresaPersonasPersistenciaDTO empresaPersonasPersistenciaDTO, string userLogin);

        ResultDTO DeletePersonaEmpresa(EmpresaPersonaPersistenciaDTO empresaPersonaPersistenciaDTO, string userLogin);
        ResultDTO DeletePersonasEmpresa(EmpresaPersonasPersistenciaDTO empresaPersonasPersistenciaDTO, string userLogin);
    }
}

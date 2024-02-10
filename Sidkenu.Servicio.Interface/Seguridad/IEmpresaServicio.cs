using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Empresa;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IEmpresaServicio
    {
        ResultDTO Add(EmpresaPersistenciaDTO entidad, string user);
        ResultDTO Update(EmpresaPersistenciaDTO entidad, string user);
        ResultDTO Delete(EmpresaDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(EmpresaFilterDTO filter);
        ResultDTO GetAll();
    }
}

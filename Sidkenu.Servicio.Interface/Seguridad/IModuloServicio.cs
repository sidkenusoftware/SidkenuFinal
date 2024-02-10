using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Modulo;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IModuloServicio
    {
        ResultDTO AddOrUpdate(ModuloPersistenciaDTO entidad, string user);

        ResultDTO Get(Guid empresaId);
    }
}

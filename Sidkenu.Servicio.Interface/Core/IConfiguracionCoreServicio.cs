using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ConfiguracionCore;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IConfiguracionCoreServicio
    {
        ResultDTO AddOrUpdate(ConfiguracionCorePersistenciaDTO entidad, string userLogin);

        ResultDTO Get(Guid empresaId);
    }
}

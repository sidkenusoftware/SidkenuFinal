using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ConfiguracionBalanza;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IConfiguracionBalanzaServicio
    {
        ResultDTO AddOrUpdate(ConfiguracionBalanzaPersistenciaDTO entidad, string userLogin);

        ResultDTO Get(Guid empresaId);
    }
}

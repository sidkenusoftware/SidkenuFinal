using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Configuracion;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IConfiguracionServicio
    {
        ResultDTO AddOrUpdate(ConfiguracionPersistenciaDTO entidad, string user);

        ResultDTO Get();
    }
}

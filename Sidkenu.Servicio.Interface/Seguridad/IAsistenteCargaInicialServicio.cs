using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.AsistenteCargaInicial;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IAsistenteCargaInicialServicio
    {
        ResultDTO Add(AsistenteDTO entidad);
    }
}

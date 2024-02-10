using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Comprobante;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IComprobanteServicio 
    {
        ResultDTO Add(ComprobanteDTO comprobante, string userLogin);
        ResultDTO GetComprobantesPendientesCobroVentaMostrador(Guid id, Guid empresaId, string cadenaBuscar);
    }
}

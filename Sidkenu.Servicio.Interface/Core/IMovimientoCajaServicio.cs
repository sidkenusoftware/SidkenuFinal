using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IMovimientoCajaServicio
    {
        ResultDTO ObtenerMovimientos(Guid? cajaDetalleId, DateTime fechaDesde, DateTime fechaHasta);
    }
}

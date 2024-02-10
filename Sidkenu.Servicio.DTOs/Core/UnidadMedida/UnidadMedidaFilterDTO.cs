using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.UnidadMedida
{
    public class UnidadMedidaFilterDTO : FilterBaseDTO
    {
        public string? CadenaBuscar { get; set; } = null;
    }
}
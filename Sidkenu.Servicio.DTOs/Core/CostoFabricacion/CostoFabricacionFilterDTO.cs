using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.CostoFabricacion
{
    public class CostoFabricacionFilterDTO : FilterBaseDTO
    {
        public string? CadenaBuscar { get; set; } = null;
    }
}
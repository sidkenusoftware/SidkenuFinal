using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Marca
{
    public class MarcaFilterDTO : FilterBaseDTO
    {
        public string? CadenaBuscar { get; set; } = null;
    }
}
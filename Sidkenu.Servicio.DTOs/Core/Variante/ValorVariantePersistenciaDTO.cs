using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Variante
{
    public class ValorVariantePersistenciaDTO : EntidadBaseDTO
    {   
        public Guid VarianteId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}

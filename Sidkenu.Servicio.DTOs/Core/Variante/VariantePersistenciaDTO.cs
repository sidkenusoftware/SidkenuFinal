using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Variante
{
    public class VariantePersistenciaDTO : EntidadBaseDTO
    {
        // Propiedades
        public Guid? EmpresaId { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

    }
}

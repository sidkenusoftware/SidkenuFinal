using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class Mesa : EntidadBase
    {
        // Propiedades
        public Guid SalonId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public EstadoMesa EstadoMesa { get; set; }

        // Propiedades de Navegacion
        public virtual Salon Salon { get; set; }
        public virtual List<ComprobanteSalon> ComprobantesSalones { get; set; }
    }
}

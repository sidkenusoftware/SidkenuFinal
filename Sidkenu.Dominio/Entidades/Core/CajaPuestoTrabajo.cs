using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class CajaPuestoTrabajo : EntidadBase
    {
        // Propiedades
        public Guid CajaId { get; set; }

        public Guid PuestoTrabajoId { get; set; }


        // Propiedades de Navegacion
        public virtual Caja Caja { get; set; }

        public virtual PuestoTrabajo PuestoTrabajo { get; set; }
    }
}

using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Core;

namespace Sidkenu.Dominio.Entidades.Seguridad
{
    public class PuestoTrabajo : EntidadBase
    {
        // Propiedades
        public Guid EmpresaId { get; set; }

        public string Descripcion { get; set; }

        public string SerialNumber { get; set; }


        // Propiedades de Navegacion
        public virtual Empresa Empresa { get; set; }

        public virtual List<CajaPuestoTrabajo> Cajas { get; set; }
    }
}

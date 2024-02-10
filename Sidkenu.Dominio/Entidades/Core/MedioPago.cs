using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class MedioPago : EntidadBase
    {
        // Propiedades
        public Guid EmpresaId { get; set; }
        public Guid ComprobanteId { get; set; }

        public TipoMedioDePago Tipo { get; set; }
        public decimal Capital { get; set; }
        public decimal Interes { get; set; }


        // Propiedades de Navegacion
        public virtual Empresa Empresa { get; set; }
        public virtual Comprobante Comprobante { get; set; }
    }
}

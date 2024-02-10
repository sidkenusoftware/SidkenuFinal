using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class UnidadMedida : EntidadBase
    {
        public Guid? EmpresaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Equivalencia { get; set; }

        // Propiedad de Navegacion
        public virtual List<Articulo> ArticulosVentas { get; set; }
        public virtual List<Articulo> ArticulosCompras { get; set; }

        public virtual Empresa Empresa { get; set; }
    }
}
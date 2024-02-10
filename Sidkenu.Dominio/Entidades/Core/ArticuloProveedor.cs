using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class ArticuloProveedor : EntidadBase
    {
        // Propiedades
        public Guid ArticuloId { get; set; }
        public Guid ProveedorId { get; set; }
        public string CodigoProveedor { get; set; }

        // Propiedades de Navegacion
        public virtual Articulo Articulo { get; set; }
        public virtual Proveedor Proveedor { get; set; }
    }
}

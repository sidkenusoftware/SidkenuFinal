using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class ArticuloFormula : EntidadBase
    {
        public Guid ArticuloId { get; set; }
        public Guid ArticuloSecundarioId { get; set; }
        public decimal Cantidad { get; set; }

        // Propiedades de Navegacion
        public virtual Articulo Articulo { get; set; }
        public virtual Articulo ArticuloSecundario { get; set; }
    }
}


using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class VarianteValor : EntidadBase
    {
        // Propiedades
        public Guid VarianteId { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }


        // Propiedades de Navegacion
        public virtual Variante Variante { get; set; }

        public virtual List<Articulo> ArticuloUnos { get; set; }
        public virtual List<Articulo> ArticuloDos { get; set; }
    }
}

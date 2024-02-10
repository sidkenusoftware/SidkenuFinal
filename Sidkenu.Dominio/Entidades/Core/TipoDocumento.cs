using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class TipoDocumento : EntidadBase
    {
        // Propiedades
        public string Descripcion { get; set; }


        // Propiedades de Navegacion
        public virtual List<Cliente> Clientes { get; set; }
    }
}

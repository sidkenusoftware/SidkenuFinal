using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Seguridad
{
    public class Localidad : EntidadBase
    {
        // Propiedades
        public Guid ProvinciaId { get; set; }

        public string Descripcion { get; set; }

        // Propiedades de Navegacion
        public Provincia Provincia { get; set; }

        public virtual List<Empresa> Empresas { get; set; }
    }
}
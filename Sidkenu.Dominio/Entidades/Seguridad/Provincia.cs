using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Seguridad
{
    public class Provincia : EntidadBase
    {
        // Propiedades
        public string Descripcion { get; set; }


        // Propiedades de Navegacion
        public virtual List<Localidad> Localidades { get; set; }
    }
}
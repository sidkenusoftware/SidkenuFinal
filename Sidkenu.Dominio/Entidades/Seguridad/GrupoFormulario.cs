using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Seguridad
{
    public class GrupoFormulario : EntidadBase
    {
        // Propiedades
        public Guid GrupoId { get; set; }
        public Guid FormularioId { get; set; }


        // Propiedades de Navegacion
        public Grupo Grupo { get; set; }
        public Formulario Formulario { get; set; }
    }
}

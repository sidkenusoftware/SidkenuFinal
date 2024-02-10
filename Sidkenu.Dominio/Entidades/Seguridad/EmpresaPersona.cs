using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Seguridad
{
    public class EmpresaPersona : EntidadBase
    {
        // Propiedades
        public Guid PersonaId { get; set; }
        public Guid EmpresaId { get; set; }

        // Propiedades de Navegacion

        public Persona Persona { get; set; }
        public Empresa Empresa { get; set; }
    }
}

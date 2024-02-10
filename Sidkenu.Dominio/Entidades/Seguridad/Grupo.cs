using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Seguridad
{
    public class Grupo : EntidadBase
    {
        // Propiedades
        public Guid EmpresaId { get; set; }

        public string Descripcion { get; set; }

        public bool PorDefecto { get; set; }


        // Propiedades de Navegacion
        public Empresa Empresa { get; set; }

        public virtual List<GrupoFormulario> GrupoFormularios { get; set; }
        public virtual List<GrupoPersona> GrupoPersonas { get; set; }
    }
}

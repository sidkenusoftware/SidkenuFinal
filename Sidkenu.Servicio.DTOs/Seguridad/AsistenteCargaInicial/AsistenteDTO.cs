using Sidkenu.Servicio.DTOs.Seguridad.Formulario;

namespace Sidkenu.Servicio.DTOs.Seguridad.AsistenteCargaInicial
{
    public class AsistenteDTO
    {
        public AsistenteDTO()
        {
            Formularios ??= new List<FormularioDTO>();
        }

        public EmpresaAsistenteDTO Empresa { get; set; }
        public PersonaAsistenteDTO Persona { get; set; }
        public ConfiguracionAsistenteDTO Configuracion { get; set; }
        public List<FormularioDTO> Formularios { get; set; }
    }
}

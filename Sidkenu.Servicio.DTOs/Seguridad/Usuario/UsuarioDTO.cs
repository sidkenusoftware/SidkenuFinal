using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Empresa;

namespace Sidkenu.Servicio.DTOs.Seguridad.Usuario
{
    public class UsuarioDTO : EntidadBaseDTO
    {
        public string Nombre { get; set; }

        public bool EstaBloqueado { get; set; }

        public bool EstaEliminado { get; set; }

        public string Password { get; set; }

        public bool Existe { get; set; }


        // Datos de la Persona

        public Guid PersonaId { get; set; }

        public string ApellidoPersona { get; set; }

        public string NombrePersona { get; set; }

        public string ApyNomPersona => $"{ApellidoPersona} {NombrePersona}";

        public string EmailPersona { get; set; }

        public byte[] FotoPersona { get; set; }

        public bool InicioPorPrimeraVez { get; set; }

        // ------------------------------------------------------ //

        public List<EmpresaDTO> Empresas { get; set; }
    }
}

using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.Persona
{
    public class PersonaPersistenciaDTO : EntidadBaseDTO
    {
        public string Apellido { get; set; }

        public string Nombre { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Mail { get; set; }

        public string Cuil { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public byte[] Foto { get; set; }
    }
}

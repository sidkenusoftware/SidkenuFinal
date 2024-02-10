﻿using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Seguridad
{
    public class Usuario : EntidadBase
    {
        // Propiedades
        public Guid PersonaId { get; set; }

        public string Nombre { get; set; }

        public string Password { get; set; }

        public bool InicioPorPrimeraVez { get; set; }

        // Propiedades de Navegacion
        public Persona Persona { get; set; }
    }
}

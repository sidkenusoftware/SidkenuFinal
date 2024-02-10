﻿using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.Formulario
{
    public class FormularioDTO : EntidadBaseDTO
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionCompleta { get; set; }
        public bool EstaVigente { get; set; }
        public bool ExisteBase { get; set; } = false;
    }
}

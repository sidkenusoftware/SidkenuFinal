﻿using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Proveedor
{
    public class ProveedorDTO : EntidadBaseDTO
    {
        public Guid? EmpresaId { get; set; }
        public Guid TipoResponsabilidadId { get; set; }
        public string TipoResponsabilidad { get; set; }

        public string Codigo { get; set; }

        public string RazonSocial { get; set; }

        public string CUIT { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Contacto { get; set; }

        public string Mail { get; set; }

        public bool ActivarCuentaCorriente { get; set; }
    }
}

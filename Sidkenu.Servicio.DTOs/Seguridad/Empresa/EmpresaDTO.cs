﻿using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.Empresa
{
    public class EmpresaDTO : EntidadBaseDTO
    {
        // Propiedades
        public Guid LocalidadId { get; set; }
        public string Localidad { get; set; }

        public Guid ProvinciaId { get; set; }
        public string Provincia { get; set; }

        public Guid TipoResponsabilidadId { get; set; }
        public string TipoResponsabilidad { get; set; }

        public Guid IngresoBrutoId { get; set; }
        public string IngresoBruto { get; set; }

        public int Codigo { get; set; }

        public string Abreviatura { get; set; }

        public string Descripcion { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Mail { get; set; }

        public string Referente { get; set; }

        public string Cuit { get; set; }

        public DateTime FechaInicioActividad { get; set; }

        public string NroIngresoBruto { get; set; }

        public byte[] Logo { get; set; }

        public string RazonSocial => $"{Descripcion} ({Direccion})";
    }
}

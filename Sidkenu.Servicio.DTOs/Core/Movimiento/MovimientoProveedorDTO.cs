﻿using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Movimiento
{
    public class MovimientoProveedorDTO : EntidadBaseDTO
    {
        // Propiedades de Navegacion
        public Guid CajaDetalleId { get; set; }
        public Guid CuentaCorrienteProveedorId { get; set; }

        public string Proveedor { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
    }
}

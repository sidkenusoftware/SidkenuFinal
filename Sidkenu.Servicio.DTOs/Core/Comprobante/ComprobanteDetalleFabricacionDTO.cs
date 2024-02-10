﻿using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Comprobante
{
    public class ComprobanteDetalleFabricacionDTO : EntidadBaseDTO
    {
        public Guid? ArticuloId { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioPublico { get; set; }
        public decimal SubTotal { get; set; }
    }
}

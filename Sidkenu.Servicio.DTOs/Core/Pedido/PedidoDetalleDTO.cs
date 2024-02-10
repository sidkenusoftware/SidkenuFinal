﻿using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Pedido
{
    public class PedidoDetalleDTO : EntidadBaseDTO
    {   
        public Guid ArticuloId { get; set; }

        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Iva { get; set; }
        public decimal Cantidad { get; set; }
        public decimal SubTotal { get; set; }
    }
}

﻿using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.OrdenFabricacion
{
    public class OrdenFabricacionDTO : EntidadBaseDTO
    {
        public OrdenFabricacionDTO()
        {
            Detalles ??= new List<OrdenFabricacionDetalleDTO>();
        }

        public DateTime Fecha { get; set; }
        public int Numero { get; set; }

        public Guid DepositoOrigenId { get; set; }
        public string DepositoOrigen { get; set; }
        
        public Guid DepositoDestinoId { get; set; }
        public string DepositoDestino { get; set; }
        
        public Guid EmpresaId { get; set; }
        
        public Guid ArticuloBaseId { get; set; }        
        public string ArticuloCodigo { get; set; }
        public string ArticuloDescripcion { get; set; }

        public EstadoOrdenFabricacion EstadoOrdenFabricacion { get; set; }

        public bool ActulizarPrecioPublico { get; set; }

        public bool SePuedeFabricar { get; set; }

        public decimal CantidadFabricar { get; set; }

        public OrigenFabricacion OrigenFabricacion { get; set; }

        public DateTime? FechaFinalizacion { get; set; }

        public List<OrdenFabricacionDetalleDTO> Detalles { get; set; }
    }
}

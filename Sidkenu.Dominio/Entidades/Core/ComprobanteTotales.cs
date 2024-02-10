﻿using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class ComprobanteTotales : EntidadBase
    {
        // Propiedades
        public Guid ComprobanteId { get; set; }

        public decimal Neto { get; set; }

        public decimal Alicuota { get; set; }

        public decimal Iva { get; set; }

        // Propiedades de Navegacion
        public virtual Comprobante Comprobante { get; set; }
    }
}

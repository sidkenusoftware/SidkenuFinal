﻿using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.CondicionIva
{
    public class CondicionIvaPersistenciaDTO : EntidadBaseDTO
    {
        public Guid? EmpresaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public bool AplicaParaFacturaElectronica { get; set; }
    }
}

﻿using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Mesa
{
    public class MesaPersistenciaDTO : EntidadBaseDTO
    {
        public Guid EmpresaId { get; set; }
        public Guid SalonId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public EstadoMesa EstadoMesa { get; set; }
    }
}

﻿using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class ArticuloPrecio : EntidadBase
    {
        // Propiedades
        public Guid ArticuloId { get; set; }
        public Guid ListaPrecioId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaActualizacion { get; set; }


        // Propiedades de Navegacion
        public Articulo Articulo { get; set; }
        public ListaPrecio ListaPrecio { get; set; }
    }
}

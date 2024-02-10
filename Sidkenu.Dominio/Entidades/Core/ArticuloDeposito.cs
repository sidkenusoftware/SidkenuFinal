﻿using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class ArticuloDeposito : EntidadBase
    {
        public Guid ArticuloId { get; set; }
        public Guid DepositoId { get; set; }
        public decimal Cantidad { get; set; }

        // Propiedades de Navegacion
        public virtual Articulo Articulo { get; set; }
        public virtual Deposito Deposito { get; set; }
    }
}

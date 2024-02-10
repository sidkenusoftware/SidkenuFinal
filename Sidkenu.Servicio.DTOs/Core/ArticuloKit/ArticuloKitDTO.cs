using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.ArticuloKit
{
    public class ArticuloKitDTO : EntidadBaseDTO
    {
        public Guid ArticuloHijoId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioCosto { get; set; }
        public decimal PrecioPublico { get; set; }
        public decimal SubTotal => PrecioPublico * Cantidad;
        public bool ExisteBase { get; set; }
    }
}

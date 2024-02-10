using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.ArticuloFormula
{
    public class ArticuloFormulaDTO : EntidadBaseDTO
    {
        public Guid ArticuloHijoId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Perdida { get; set; }
        public TipoValor TipoValor { get; set; }
        public bool ExisteBase { get; set; }
    }
}

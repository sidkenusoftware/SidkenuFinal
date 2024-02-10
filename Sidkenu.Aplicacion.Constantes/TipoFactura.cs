using System.ComponentModel;

namespace Sidkenu.Aplicacion.Constantes
{
    public enum TipoFactura
    {
        [Description("Fact. A")]
        FacturaA = 0,

        [Description("Fact. B")]
        FacturaB = 1,

        [Description("Fact. C")]
        FacturaC = 2,

        [Description("Presupuesto")]
        Presupuesto = 3,
    }
}

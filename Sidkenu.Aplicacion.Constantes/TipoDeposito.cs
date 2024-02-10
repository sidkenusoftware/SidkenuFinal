using System.ComponentModel;

namespace Sidkenu.Aplicacion.Constantes
{
    public enum TipoDeposito
    {
        [Description("Venta")]
        Venta = 0,

        [Description("Compra")]
        Compra = 1,

        [Description("Compra-Venta")]
        CompraVenta = 2,
    }
}
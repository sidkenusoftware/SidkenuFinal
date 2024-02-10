using System.ComponentModel;

namespace Sidkenu.Aplicacion.Constantes
{
    public enum TipoMedioDePago
    {
        [Description("Efectivo")]
        Efectivo = 0,

        [Description("Tarjeta")]
        Tarjeta = 1,

        [Description("Cuenta Corriente")]
        CuentaCorriente = 2,

        [Description("Cheque")]
        Cheque = 3,

        [Description("Transferencia")]
        Transferencia = 4,

        [Description("Mercado Pago")]
        MercadoPago = 5,
    }
}

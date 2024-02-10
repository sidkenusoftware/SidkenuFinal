using System.ComponentModel;

namespace Sidkenu.Aplicacion.Constantes
{
    public enum TipoMovimiento
    {
        [Description("Ingreso")]
        Ingreso = 1,

        [Description("Egreso")]
        Egreso = -1
    }
}

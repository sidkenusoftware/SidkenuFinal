using System.ComponentModel;

namespace Sidkenu.Aplicacion.Constantes
{
    public enum EstadoMesa
    {
        [Description("Cerrada")]
        Cerrada = 0,

        [Description("Abierta")]
        Abierta = 1,

        [Description("Fuera de Servicio")]
        FueraServicio = 2,

        [Description("Reservada")]
        Reservada = 3
    }
}

using System.ComponentModel;

namespace Sidkenu.Aplicacion.Constantes
{
    public enum EstadoOrdenFabricacion
    {
        [Description("Pendiente")]
        Pendiente = 0,

        [Description("En Proceso")]
        EnProceso = 1,

        [Description("Finalizada")]
        Finalizada = 2,
    }
}

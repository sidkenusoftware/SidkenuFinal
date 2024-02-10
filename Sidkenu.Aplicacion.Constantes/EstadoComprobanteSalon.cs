using System.ComponentModel;

namespace Sidkenu.Aplicacion.Constantes
{
    public enum EstadoComprobante
    {
        [Description("En Proceso")]
        EnProceso = 0,
        
        [Description("Facturado")]
        Facturado = 1,

        [Description("Pendiente")]
        Pendiente = 2,

        [Description("Facturado con Fabricación")]
        FacturadoConFabricacion = 3,

        [Description("Pendiente con Fabricación")]
        PendienteConFabricacion = 4
    }
}

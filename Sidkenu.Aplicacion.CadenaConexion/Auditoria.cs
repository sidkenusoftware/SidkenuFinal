using Sidkenu.Aplicacion.CadenaConexion.Constantes;

namespace Sidkenu.Aplicacion.CadenaConexion
{
    public static class Auditoria
    {
        public static TipoAccionAuditoria Obtener => Ambiente.Obtener switch
        {
            TipoAmbiente.Desarrollo => TipoAccionAuditoria.SinAuditoria,
            TipoAmbiente.Testing => TipoAccionAuditoria.SinAuditoria,
            TipoAmbiente.Produccion => TipoAccionAuditoria.ConAuditoria,
            _ => TipoAccionAuditoria.SinAuditoria
        };
    }
}

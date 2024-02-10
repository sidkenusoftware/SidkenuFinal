using Sidkenu.Aplicacion.Constantes;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IContadorServicio
    {
        long ObtenerNumero(TipoContador tipoContador, Guid empresaId, string user);
    }
}

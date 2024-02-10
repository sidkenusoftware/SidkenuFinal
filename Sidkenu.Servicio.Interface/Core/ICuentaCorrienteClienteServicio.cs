using Google.Protobuf.WellKnownTypes;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.CuentaCorrienteCliente;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface ICuentaCorrienteClienteServicio
    {
        ResultDTO GetByCliente(Guid clienteId);

        ResultDTO Add(CtaCteClientePersistenciaDTO ctaCteCliente, string user);
    }
}

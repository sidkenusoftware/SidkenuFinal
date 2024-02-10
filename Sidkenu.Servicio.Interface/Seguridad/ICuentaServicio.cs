using Sidkenu.Servicio.DTOs.Seguridad.Cuenta;
using Sidkenu.Servicio.DTOs.Seguridad.Usuario;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface ICuentaServicio
    {
        (bool, UsuarioDTO) GetLoginByCredentials(UserLoginDTO login);

        bool GenerarNuevoPassword(Guid userId);
    }
}

using AutoMapper;
using Dime.Servicio.Comun.EMail;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Seguridad.Cuenta;
using Sidkenu.Servicio.DTOs.Seguridad.Usuario;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class CuentaServicio : ServicioBase, ICuentaServicio
    {
        private readonly IPasswordServicio _passwordService;
        private readonly ICorreoElectronico _correoElectronico;

        public CuentaServicio(IUnidadDeTrabajo unitOfWork,
                              IMapper mapper,
                              ILogger logger,
                              IConfiguracionServicio configuracionServicio,
                              IPasswordServicio passwordService,
                              ICorreoElectronico correoElectronico)
                              : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _passwordService = passwordService;
            _correoElectronico = correoElectronico;
        }

        public (bool, UsuarioDTO) GetLoginByCredentials(UserLoginDTO login)
        {
            var users = _unitOfWork.UsuarioRepository.GetByFilter(x => x.Nombre == login.User, null,
                x => x.Include(p => p.Persona)
                        .ThenInclude(mp => mp.EmpresaPersonas)
                        .ThenInclude(m => m.Empresa));

            var user = users.FirstOrDefault();

            if (user == null)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"El Usuario {login.User} no existe.");
                }

                return (false, null);
            }

            // Comprueba si la contraseña es correcta
            var isValid = _passwordService.Check(users.First().Password, login.Password);

            if (!isValid)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"El Usuario {login.User}. Erro la contraseña");
                }

                return (false, null);
            }

            var userDTO = _mapper.Map<UsuarioDTO>(users.FirstOrDefault());

            if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
            {
                _logger.Information($"El Usuario {userDTO.Nombre} Ingresó correctamente");
            }

            return (true, userDTO);
        }

        public bool GenerarNuevoPassword(Guid userId)
        {
            try
            {
                var user = _unitOfWork.UsuarioRepository.GetById(userId, x => x.Include(z => z.Persona));

                if (user == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"El UsuarioId {userId} no Existe");
                    }
                }

                var nuevoPass = _passwordService.Generar(10);

                user.Password = _passwordService.Hash(nuevoPass);

                _unitOfWork.UsuarioRepository.Update(user);


                // ------------------------------------------------------------------------------------- //

                _unitOfWork.Commit();

                var asunto = "Recuperación de Contraseña";

                var mensaje = $"Ud. a solicitado la recuperación de la contraseña." + Environment.NewLine + Environment.NewLine +
                    $"La nueva contraseña es {nuevoPass}." + Environment.NewLine +
                    $"Saludos.";

                _correoElectronico.Enviar(user.Persona.Mail,
                    _configuracionDTO.UsuarioCredencial,
                    asunto,
                    mensaje,
                    _configuracionDTO.UsuarioCredencial,
                    _configuracionDTO.PasswordCredencial,
                    _configuracionDTO.Host, _configuracionDTO.Puerto,
                    false);

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Se envio el mail a {user.Persona.Mail}. Mensaje: {mensaje}.");
                }

                return true;
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userId}.");
                }

                return false;
            }
        }
    }
}

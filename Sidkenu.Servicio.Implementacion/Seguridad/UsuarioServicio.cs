using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Usuario;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;
using System.Text.RegularExpressions;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class UsuarioServicio : ServicioBase, IUsuarioServicio
    {
        private readonly IPasswordServicio _passwordServicio;

        public UsuarioServicio(IUnidadDeTrabajo unitOfWork,
                               IMapper mapper,
                               ILogger logger,
                               IConfiguracionServicio configuracionServicio,
                               IPasswordServicio passwordService)
                               : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _passwordServicio = passwordService;
        }

        public ResultDTO Crear(UsuarioPersistenciaDTO usuarioPersistenciaDTO, string userLogin)
        {
            try
            {
                var nombreUsuarioNuevo = CrearNombreUsuario(usuarioPersistenciaDTO.PersonaApellido, usuarioPersistenciaDTO.PersonaNombre);

                var nuevoUsuario = new Usuario
                {
                    EstaEliminado = false,
                    InicioPorPrimeraVez = true,
                    Nombre = nombreUsuarioNuevo,
                    Password = _passwordServicio.Hash(PasswordOptions.PasswordPorDefecto),
                    PersonaId = usuarioPersistenciaDTO.PersonaId,
                    User = userLogin
                };

                _unitOfWork.UsuarioRepository.Add(nuevoUsuario);

                _unitOfWork.Commit();


                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Crear Usuario - User: {userLogin}", nuevoUsuario);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "El usuario se creo correctamente",
                    Data = _mapper.Map<UsuarioDTO>(nuevoUsuario)
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO Crear(UsuarioPersistenciaDTO usuarioPersistenciaDTO, string password, string userLogin)
        {
            try
            {
                var nombreUsuarioNuevo = CrearNombreUsuario(usuarioPersistenciaDTO.PersonaApellido, usuarioPersistenciaDTO.PersonaNombre);

                var nuevoUsuario = new Usuario
                {
                    EstaEliminado = false,
                    InicioPorPrimeraVez = true,
                    Nombre = nombreUsuarioNuevo,
                    Password = _passwordServicio.Hash(password),
                    PersonaId = usuarioPersistenciaDTO.PersonaId,
                    User = userLogin
                };

                _unitOfWork.UsuarioRepository.Add(nuevoUsuario);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Crear Usuario - User: {userLogin}", nuevoUsuario);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "El usuario se creo correctamente",
                    Data = _mapper.Map<UsuarioDTO>(nuevoUsuario)
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO Eliminar(UsuarioDeleteDTO usuarioDeleteDTO, string userLogin)
        {
            try
            {
                var usuario = _unitOfWork.UsuarioRepository.GetById(usuarioDeleteDTO.Id);

                if (usuario == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"El Usuario - Id => {usuarioDeleteDTO.Id} no existe. - User: {userLogin}", usuarioDeleteDTO);
                    }

                    return new ResultDTO
                    {
                        Data = null,
                        Message = "El usuario seleccionado es inexistente",
                        State = false
                    };
                }

                var estaEliminado = usuario.EstaEliminado;
                usuario.User = userLogin;

                _unitOfWork.UsuarioRepository.Delete(usuarioDeleteDTO.Id, userLogin);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Usuario - User: {userLogin}", usuario);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = !estaEliminado ? "El usuario se Eliminó correctamente" : "El usuario se Activo correctamente",
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO Bloquear(UsuarioBloquearDTO usuarioBloquearDTO, string userLogin)
        {
            try
            {
                var usuario = _unitOfWork.UsuarioRepository.GetById(usuarioBloquearDTO.Id);

                if (usuario == null)
                {
                    return new ResultDTO
                    {
                        Data = null,
                        Message = "El usuario seleccionado es inexistente",
                        State = false
                    };
                }

                usuario.User = userLogin;

                _unitOfWork.UsuarioRepository.Update(usuario);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Bloquear Usuario - User: {userLogin}", usuario);
                }

                return new ResultDTO
                {
                    State = true,
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO GetById(Guid id)
        {
            try
            {
                var resultado = _unitOfWork.UsuarioRepository.GetById(id,
                    x => x.Include(p => p.Persona));

                if (resultado != null)
                {
                    var usuario = _mapper.Map<UsuarioDTO>(resultado);

                    return new ResultDTO
                    {
                        Data = usuario,
                        State = true
                    };
                }
                else
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "El usuario no fue encontrado"
                    };
                }
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO GetByName(string nombreUsuario)
        {
            try
            {
                var resultado = _unitOfWork.UsuarioRepository.GetByFilter(x => x.Nombre.ToLower() == nombreUsuario.ToLower(), null,
                    x => x.Include(p => p.Persona));

                if (resultado != null)
                {
                    var usuario = _mapper.Map<UsuarioDTO>(resultado.FirstOrDefault());

                    return new ResultDTO
                    {
                        Data = usuario,
                        State = true
                    };
                }
                else
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"El Usuario - Nombre => {nombreUsuario} no existe.");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = "El usuario no fue encontrado"
                    };
                }
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO GetByFilter(UsuarioFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                var resultado = _unitOfWork.PersonaRepository.GetByFilter(x => x.Mail != "superUsuario@dime.gov.ar"
                        && x.EstaEliminado == filter.VerEliminados
                        && (x.Nombre.ToLower().Contains(filter.CadenaBuscar.ToLower())
                        || x.Apellido.ToLower().Contains(filter.CadenaBuscar.ToLower())
                        || x.Cuil.ToLower() == filter.CadenaBuscar.ToLower()
                        || x.Usuarios.Any(n => n.Nombre.ToLower().Contains(filter.CadenaBuscar.ToLower()))), null,
                        x => x.Include(u => u.Usuarios));

                if (resultado != null)
                {
                    var usuarios = _mapper.Map<IEnumerable<UsuarioDTO>>(resultado);

                    return new ResultDTO
                    {
                        Data = usuarios,
                        State = true
                    };
                }
                else
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "El usuario no fue encontrado"
                    };
                }
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public bool VerificarSiExiste(string usuario)
        {
            var resultado = _unitOfWork.UsuarioRepository
            .GetByFilter(x => x.Nombre == usuario);

            return resultado != null && resultado.Any();
        }

        public bool VerificarSiExiste(Guid personaId)
        {
            var resultado = _unitOfWork.UsuarioRepository
                .GetByFilter(x => x.PersonaId == personaId);

            return resultado != null && resultado.Any();
        }

        public string CrearNombreUsuario(string apellido, string nombre)
        {
            var _apellido = apellido.ToLower().Replace(" ", string.Empty);
            var _nombre = nombre.ToLower().Replace(" ", string.Empty);

            int contador = 1;
            int cantidadLetras = 1;
            var nombreUsuario = RemoverAcentos($"{_nombre.Substring(0, cantidadLetras)}{_apellido}");

            var existe = _unitOfWork.UsuarioRepository.GetByFilter(x => x.Nombre == nombreUsuario);

            while (VerificarSiExiste(nombreUsuario))
            {
                if (cantidadLetras <= _nombre.Length)
                {
                    cantidadLetras++;
                    nombreUsuario = RemoverAcentos($"{_nombre.Substring(0, cantidadLetras)}{_apellido}");
                }
                else
                {
                    nombreUsuario = RemoverAcentos($"{_nombre.Substring(0, 1)}{_apellido}{contador}");
                    contador++;
                }
            }

            return nombreUsuario.ToLower();
        }

        public ResultDTO CambiarPassword(UsuarioCambioPasswordDTO usuarioCambioPasswordDTO, string userLogin)
        {
            try
            {
                var usuario = _unitOfWork.UsuarioRepository.GetById(usuarioCambioPasswordDTO.UserId);

                if (usuario == null)
                {
                    return new ResultDTO
                    {
                        Data = null,
                        Message = "El usuario seleccionado es inexistente",
                        State = false
                    };
                }

                if (!_passwordServicio.Check(usuario.Password, usuarioCambioPasswordDTO.PasswordActual))
                {
                    return new ResultDTO
                    {
                        Data = null,
                        Message = "La contraseña vieja no coincide con la actual",
                        State = false
                    };
                }

                usuario.User = userLogin;

                usuario.Password = _passwordServicio.Hash(usuarioCambioPasswordDTO.PasswordNueva);

                _unitOfWork.UsuarioRepository.Update(usuario);

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Message = "La contraseña se cambió correctamente",
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO ResetPassword(UsuarioResetPasswordDTO usuarioResetPasswordDTO, string userLogin)
        {
            try
            {
                var usuarios = _unitOfWork.UsuarioRepository
                    .GetByFilter(x => x.PersonaId == usuarioResetPasswordDTO.Id);

                if (usuarios == null && !usuarios.Any())
                {
                    return new ResultDTO
                    {
                        Data = null,
                        Message = "El usuario seleccionado es inexistente",
                        State = false
                    };
                }

                var usuario = usuarios.First();

                usuario.User = userLogin;
                usuario.Password = _passwordServicio.Hash(PasswordOptions.PasswordPorDefecto);

                _unitOfWork.UsuarioRepository.Update(usuario);

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Message = "La contraseña se restauro correctamente",
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        // ======================================================================================= //
        // =============================   Metodos Privados  ===================================== //
        // ======================================================================================= //

        private string RemoverAcentos(string inputString)
        {
            var replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            var replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            var replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            var replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            var replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);

            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_u_Accents.Replace(inputString, "u");

            return inputString;
        }
    }
}

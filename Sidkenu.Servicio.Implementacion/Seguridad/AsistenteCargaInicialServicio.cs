using AutoMapper;
using Dime.Servicio.Comun.EMail;
using FluentValidation;
using Serilog;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Infraestructura;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.AsistenteCargaInicial;
using Sidkenu.Servicio.DTOs.Seguridad.Configuracion;
using Sidkenu.Servicio.DTOs.Seguridad.Empresa;
using Sidkenu.Servicio.DTOs.Seguridad.EmpresaPersona;
using Sidkenu.Servicio.DTOs.Seguridad.Formulario;
using Sidkenu.Servicio.DTOs.Seguridad.Grupo;
using Sidkenu.Servicio.DTOs.Seguridad.GrupoFormulario;
using Sidkenu.Servicio.DTOs.Seguridad.GrupoPersona;
using Sidkenu.Servicio.DTOs.Seguridad.Persona;
using Sidkenu.Servicio.DTOs.Seguridad.Usuario;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class AsistenteCargaInicialServicio : ServicioBase, IAsistenteCargaInicialServicio
    {
        private readonly IValidator<EmpresaAsistenteDTO> _validatorEmpresaDto;
        private readonly IValidator<PersonaAsistenteDTO> _validatorPersonaDto;
        private readonly IValidator<ConfiguracionAsistenteDTO> _validatorConfiguracionDto;

        private readonly IEmpresaServicio _empresaServicio;
        private readonly IEmpresaPersonaServicio _empresaPersonaServicio;
        private readonly IConfiguracionServicio _configuracionServicio;
        private readonly IPersonaServicio _personaServicio;
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly IFormularioServicio _formularioServicio;
        private readonly IGrupoServicio _grupoServicio;
        private readonly IGrupoFormularioServicio _grupoFormularioServicio;
        private readonly IGrupoPersonaServicio _grupoPersonaServicio;
        private readonly ICorreoElectronico _correoElectronico;

        private readonly IMapper _mapper;

        public AsistenteCargaInicialServicio(IUnidadDeTrabajo unitOfWork,
            IMapper mapper,
            ILogger logger,
            IValidator<EmpresaAsistenteDTO> validatorEmpresaDto,
            IValidator<PersonaAsistenteDTO> validatorPersonaDto,
            IValidator<ConfiguracionAsistenteDTO> validatorConfiguracionDto,
            IEmpresaServicio empresaServicio,
            IConfiguracionServicio configuracionServicio,
            IPersonaServicio personaServicio,
            IUsuarioServicio usuarioServicio,
            IEmpresaPersonaServicio empresaPersonaServicio,
            IFormularioServicio formularioServicio,
            IGrupoServicio grupoServicio,
            IGrupoFormularioServicio grupoFormularioServicio,
            IGrupoPersonaServicio grupoPersonaServicio,
            ICorreoElectronico correoElectronico)
            : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _validatorEmpresaDto = validatorEmpresaDto;
            _validatorPersonaDto = validatorPersonaDto;
            _validatorConfiguracionDto = validatorConfiguracionDto;

            _empresaServicio = empresaServicio;
            _configuracionServicio = configuracionServicio;
            _personaServicio = personaServicio;
            _mapper = mapper;
            _usuarioServicio = usuarioServicio;
            _empresaPersonaServicio = empresaPersonaServicio;
            _formularioServicio = formularioServicio;
            _grupoServicio = grupoServicio;
            _grupoFormularioServicio = grupoFormularioServicio;
            _grupoPersonaServicio = grupoPersonaServicio;
            _correoElectronico = correoElectronico;
        }


        public ResultDTO Add(AsistenteDTO entidad)
        {
            using var transaccion = new DataContext().Database.BeginTransaction();

            try
            {
                var validationEmpresaResult = _validatorEmpresaDto.Validate(entidad.Empresa);

                if (!validationEmpresaResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationEmpresaResult);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validación de Datos de Empresa: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var validationPersonaResult = _validatorPersonaDto.Validate(entidad.Persona);

                if (!validationPersonaResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationPersonaResult);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validación de Datos de Empresa: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var validationConfiguracionResult = _validatorConfiguracionDto.Validate(entidad.Configuracion);

                if (!validationConfiguracionResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationConfiguracionResult);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validación de Datos de Empresa: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var entidadEmpresaDTO = _mapper.Map<EmpresaPersistenciaDTO>(entidad.Empresa);
                var entidadPersonaDTO = _mapper.Map<PersonaPersistenciaDTO>(entidad.Persona);
                var entidadConfiguracionDTO = _mapper.Map<ConfiguracionPersistenciaDTO>(entidad.Configuracion);

                var userAsistente = "CargaPorAsistente";

                var empresaResult = _empresaServicio.Add(entidadEmpresaDTO, userAsistente);

                if (!empresaResult.State || empresaResult.Data == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                    {
                        _logger.Error($"Ocurrió un error al grabar la Empresa. {empresaResult.Message}", entidadEmpresaDTO);
                    }

                    throw new Exception($"Ocurrió un error al grabar la Empresa. {empresaResult.Message}");
                }

                var empresaEntity = (Empresa)empresaResult.Data;

                var personaResult = _personaServicio.Add(entidadPersonaDTO, userAsistente);

                if (!personaResult.State || personaResult.Data == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                    {
                        _logger.Error($"Ocurrió un error al grabar la Persona. {empresaResult.Message}", entidadPersonaDTO);
                    }

                    throw new Exception($"Ocurrió un error al grabar la Persona. {personaResult.Message}");
                }

                var personaEntity = (Persona)personaResult.Data;

                var usuarioDto = new DTOs.Seguridad.Usuario.UsuarioPersistenciaDTO
                {
                    InicioPorPrimeraVez = true,
                    PersonaApellido = personaEntity.Apellido,
                    PersonaNombre = personaEntity.Nombre,
                    PersonaId = personaEntity.Id,
                };

                var usuarioResult = _usuarioServicio.Crear(usuarioDto, userAsistente);

                if (!usuarioResult.State || usuarioResult.Data == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                    {
                        _logger.Error($"Ocurrió un error al Crear el Usuario. {usuarioResult.Message}", usuarioDto);
                    }

                    throw new Exception($"Ocurrió un error al Crear el Usuario. {usuarioResult.Message}");
                }

                var empresaPersonaDto = new EmpresaPersonaPersistenciaDTO
                {
                    EmpresaId = empresaEntity.Id,
                    PersonaId = personaEntity.Id
                };

                var empresaPersonaResult = _empresaPersonaServicio.AddPersonaEmpresa(empresaPersonaDto, userAsistente);

                if (!empresaPersonaResult.State)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                    {
                        _logger.Error("Ocurrió un error al grabar la Persona en una Empresa");
                    }

                    throw new Exception("Ocurrió un error al grabar la Persona en una Empresa");
                }

                var grupoAdmin = new GrupoPersistenciaDTO
                {
                    EmpresaId = empresaEntity.Id,
                    Descripcion = "Administrador",
                };

                var grupoResult = _grupoServicio.Add(grupoAdmin, userAsistente);

                if (!grupoResult.State)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                    {
                        _logger.Error("Ocurrió un error al grabar el grupo Administrador");
                    }

                    throw new Exception("Ocurrió un error al grabar el grupo Administrador");
                }

                var formularioResult = _formularioServicio.Add(entidad.Formularios, userAsistente);

                if (!formularioResult.State)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                    {
                        _logger.Error("Ocurrió un error al grabar los formularios/pantallas");
                    }

                    throw new Exception("Ocurrió un error al grabar los formularios/pantallas");
                }

                var grupoPersona = new GrupoPersonaPersistenciaDTO
                {
                    GrupoId = ((Grupo)grupoResult.Data).Id,
                    PersonaId = ((Persona)personaResult.Data).Id
                };

                var grupoPersonaResult = _grupoPersonaServicio.AddPersonaGrupo(grupoPersona, userAsistente);

                if (!grupoPersonaResult.State)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                    {
                        _logger.Error("Ocurrió un error al asignar la persona al grupo de administrador");
                    }

                    throw new Exception("Ocurrió un error al asignar la persona al grupo de administrador");
                }

                var formularioBdResult = _formularioServicio.GetAll();

                if (formularioBdResult.State)
                {
                    foreach (var formulario in (List<FormularioDTO>)formularioBdResult.Data)
                    {
                        var grupoFormulario = new GrupoFormularioPersistenciaDTO
                        {
                            GrupoId = ((Grupo)grupoResult.Data).Id,
                            FormularioId = formulario.Id
                        };

                        _grupoFormularioServicio.AddFormularioGrupo(grupoFormulario, userAsistente);
                    }
                }

                // Envio de Mail

                var configResult = _configuracionServicio.Get();

                if (configResult.State)
                {
                    var config = (ConfiguracionDTO)configResult.Data;

                    var asunto = $"Bienvenido al Sistema de Gestión Tsidkenu";

                    var mensaje = $"Hola {personaEntity.Apellido}, {personaEntity.Nombre}" + Environment.NewLine + Environment.NewLine;

                    mensaje += $"El usuario para ingresar al sistema es: {((UsuarioDTO)usuarioResult.Data).Nombre} " +
                               $"y la contraseña es: {Aplicacion.Comun.PasswordOptions.PasswordPorDefecto}." + Environment.NewLine + Environment.NewLine +
                               $"Gracias por elegir la plataforma Tsidkenu, la cual transformara todas tus actividades";

                    _correoElectronico.Enviar(personaEntity.Mail,
                        empresaEntity.Mail,
                        asunto,
                        mensaje,
                        config.UsuarioCredencial,
                        config.PasswordCredencial,
                        config.Host,
                        config.Puerto);
                }

                transaccion.Commit();

                return new ResultDTO
                {
                    State = true,
                    Message = "Los datos se grabaron correctamente",
                };
            }
            catch (Exception ex)
            {
                transaccion.Rollback();

                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - Asistente Inicial.");
                }

                return new ResultDTO
                {
                    State = false,
                    Message = "Ocurrió un error al Grabar los Datos"
                };
            }
        }
    }
}

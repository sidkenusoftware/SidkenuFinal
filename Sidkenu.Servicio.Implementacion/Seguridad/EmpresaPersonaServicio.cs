using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.EmpresaPersona;
using Sidkenu.Servicio.DTOs.Seguridad.Persona;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class EmpresaPersonaServicio : ServicioBase, IEmpresaPersonaServicio
    {
        public EmpresaPersonaServicio(IUnidadDeTrabajo unitOfWork,
                                      IMapper mapper,
                                      ILogger logger,
                                      IConfiguracionServicio configuracionServicio)
                                      : base(unitOfWork, mapper, logger, configuracionServicio)
        {
        }

        public ResultDTO AddPersonaEmpresa(EmpresaPersonaPersistenciaDTO empresaPersonaPersistenciaDTO, string userLogin)
        {
            try
            {
                var result = _unitOfWork.EmpresaPersonaRepository
                    .GetByFilter(x => x.EmpresaId == empresaPersonaPersistenciaDTO.EmpresaId
                                      && x.PersonaId == empresaPersonaPersistenciaDTO.PersonaId, null, null, false);

                if (result == null || !result.Any())
                {
                    _unitOfWork.EmpresaPersonaRepository
                       .Add(new EmpresaPersona
                       {
                           EmpresaId = empresaPersonaPersistenciaDTO.EmpresaId,
                           PersonaId = empresaPersonaPersistenciaDTO.PersonaId,
                           User = userLogin,
                           EstaEliminado = false
                       });

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Add Empresa {empresaPersonaPersistenciaDTO.EmpresaId} " +
                            $"- Persona {empresaPersonaPersistenciaDTO.PersonaId} " +
                            $"- User: {userLogin}", empresaPersonaPersistenciaDTO);
                    }
                }
                else
                {
                    var grupoPersona = result.FirstOrDefault();

                    grupoPersona.EstaEliminado = false;

                    _unitOfWork.EmpresaPersonaRepository.Update(grupoPersona);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Update Empresa {empresaPersonaPersistenciaDTO.EmpresaId} " +
                            $"- Persona {empresaPersonaPersistenciaDTO.PersonaId} " +
                            $"- User: {userLogin}", empresaPersonaPersistenciaDTO);
                    }
                }

                _unitOfWork.Commit();



                return new ResultDTO
                {
                    State = true,
                    Message = "El persona se asigno correctamente"
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", empresaPersonaPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO AddPersonasEmpresa(EmpresaPersonasPersistenciaDTO empresaPersonasPersistenciaDTO, string userLogin)
        {
            try
            {
                foreach (var personaId in empresaPersonasPersistenciaDTO.PersonaIds)
                {
                    var result = _unitOfWork.EmpresaPersonaRepository
                        .GetByFilter(x => x.EmpresaId == empresaPersonasPersistenciaDTO.EmpresaId
                                          && x.PersonaId == personaId, null, null, false);

                    if (result == null || !result.Any())
                    {
                        _unitOfWork.EmpresaPersonaRepository
                           .Add(new EmpresaPersona
                           {
                               EmpresaId = empresaPersonasPersistenciaDTO.EmpresaId,
                               PersonaId = personaId,
                               User = userLogin,
                               EstaEliminado = false
                           });

                        if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                        {
                            _logger.Information($"Add Empresa {empresaPersonasPersistenciaDTO.EmpresaId} " +
                            $"- Persona {personaId} " +
                            $"- User: {userLogin}", empresaPersonasPersistenciaDTO);
                        }
                    }
                    else
                    {
                        var grupoPersona = result.FirstOrDefault();

                        grupoPersona.EstaEliminado = false;

                        _unitOfWork.EmpresaPersonaRepository.Update(grupoPersona);

                        _logger.Information($"Update Empresa {empresaPersonasPersistenciaDTO.EmpresaId} " +
                            $"- Persona {personaId} " +
                            $"- User: {userLogin}", empresaPersonasPersistenciaDTO);
                    }
                }

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Message = "Los personas se asignaron correctamente"
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", empresaPersonasPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO GetByPersonasAsignadas(EmpresaPersonaFilterDTO filterDTO)
        {
            try
            {
                var result = _unitOfWork.EmpresaPersonaRepository
                    .GetByFilter(x => !x.EstaEliminado && x.Persona.Cuil != "99999999999"
                                                       && x.EmpresaId == filterDTO.EmpresaId
                                                       && (x.Persona.Apellido.ToLower().Contains(filterDTO.CadenaBuscar.ToLower())
                                                           || x.Persona.Nombre.ToLower().Contains(filterDTO.CadenaBuscar.ToLower()))
                        , o => o.OrderBy(r => r.Persona.Apellido)
                        , i => i.Include(r => r.Persona).ThenInclude(x => x.Usuarios));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<PersonaDTO>>(result)
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

        public ResultDTO GetByPersonasNoAsignadas(EmpresaPersonaFilterDTO filterDTO)
        {
            try
            {
                var result = _unitOfWork.EmpresaPersonaRepository
                    .GetByFilter(x => !x.EstaEliminado && x.Persona.Cuil != "99999999999"
                                                       && x.EmpresaId == filterDTO.EmpresaId
                        , o => o.OrderBy(r => r.Persona.Apellido).ThenBy(r => r.Persona.Nombre)
                        , i => i.Include(r => r.Persona).ThenInclude(x => x.Usuarios));

                var personasAsignadas = result.Select(x => x.Persona);

                var personas = _unitOfWork.PersonaRepository.GetByFilter(x => x.Cuil != "99999999999");

                var personasNoAsignados = personas.Except(personasAsignadas, new GenericCompare<Persona>());

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<PersonaDTO>>(personasNoAsignados
                                            .Where(x => x.Apellido.ToLower().Contains(filterDTO.CadenaBuscar.ToLower())
                                                       || x.Nombre.ToLower().Contains(filterDTO.CadenaBuscar.ToLower())).ToList())
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

        public ResultDTO DeletePersonaEmpresa(EmpresaPersonaPersistenciaDTO empresaPersonaPersistenciaDTO, string userLogin)
        {
            try
            {
                var result = _unitOfWork.EmpresaPersonaRepository
                    .GetByFilter(x => x.EmpresaId == empresaPersonaPersistenciaDTO.EmpresaId && x.PersonaId == empresaPersonaPersistenciaDTO.PersonaId, null, null, false);

                if (result == null || !result.Any())
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "No se encontro el persona asignado al grupo"
                    };
                }

                var grupoPersona = result.FirstOrDefault();

                grupoPersona.EstaEliminado = true;

                _unitOfWork.EmpresaPersonaRepository.Update(grupoPersona);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Empresa - Persona => EmpresaId: {empresaPersonaPersistenciaDTO.EmpresaId} " +
                        $"- PersonaId: {empresaPersonaPersistenciaDTO.PersonaId}" +
                        $"- User: {userLogin}", empresaPersonaPersistenciaDTO);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "El persona fue quitado correctamente"
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", empresaPersonaPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO DeletePersonasEmpresa(EmpresaPersonasPersistenciaDTO empresaPersonasPersistenciaDTO, string userLogin)
        {
            try
            {
                foreach (var personaId in empresaPersonasPersistenciaDTO.PersonaIds)
                {
                    var result = _unitOfWork.EmpresaPersonaRepository
                        .GetByFilter(x => x.EmpresaId == empresaPersonasPersistenciaDTO.EmpresaId && x.PersonaId == personaId, null, null, false);

                    if (result == null || !result.Any())
                    {
                        return new ResultDTO
                        {
                            State = false,
                            Message = "No se encontro el persona asignado al grupo"
                        };
                    }

                    var grupoPersona = result.FirstOrDefault();

                    grupoPersona.EstaEliminado = true;

                    _unitOfWork.EmpresaPersonaRepository.Update(grupoPersona);

                    _logger.Information($"Delete Empresa - Persona => EmpresaId: {empresaPersonasPersistenciaDTO.EmpresaId} " +
                        $"- PersonaId: {personaId}" +
                        $"- User: {userLogin}", empresaPersonasPersistenciaDTO);
                }

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Message = "Los personas fueron quitados correctamente"
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", empresaPersonasPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }
    }
}

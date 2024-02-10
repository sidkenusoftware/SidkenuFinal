using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.GrupoPersona;
using Sidkenu.Servicio.DTOs.Seguridad.Persona;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class GrupoPersonaServicio : ServicioBase, IGrupoPersonaServicio
    {
        public GrupoPersonaServicio(IUnidadDeTrabajo unitOfWork,
                                       IMapper mapper,
                                       ILogger logger,
                                       IConfiguracionServicio configuracionServicio)
                                       : base(unitOfWork, mapper, logger, configuracionServicio)
        {
        }


        public ResultDTO AddPersonaGrupo(GrupoPersonaPersistenciaDTO grupoPersonaPersistenciaDTO, string userLogin)
        {
            try
            {
                var result = _unitOfWork.GrupoPersonaRepository
                    .GetByFilter(x => x.GrupoId == grupoPersonaPersistenciaDTO.GrupoId
                                      && x.PersonaId == grupoPersonaPersistenciaDTO.PersonaId, null, null, false);

                if (result == null || !result.Any())
                {
                    _unitOfWork.GrupoPersonaRepository
                       .Add(new GrupoPersona
                       {
                           GrupoId = grupoPersonaPersistenciaDTO.GrupoId,
                           PersonaId = grupoPersonaPersistenciaDTO.PersonaId,
                           User = userLogin,
                           EstaEliminado = false
                       });

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Add Grupo {grupoPersonaPersistenciaDTO.GrupoId} " +
                            $"- Persona {grupoPersonaPersistenciaDTO.PersonaId} " +
                            $"- User: {userLogin}", grupoPersonaPersistenciaDTO);
                    }
                }
                else
                {
                    var grupoPersona = result.FirstOrDefault();

                    grupoPersona.EstaEliminado = false;

                    _unitOfWork.GrupoPersonaRepository.Update(grupoPersona);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Update Grupo {grupoPersonaPersistenciaDTO.GrupoId} " +
                            $"- Persona {grupoPersonaPersistenciaDTO.PersonaId} " +
                            $"- User: {userLogin}", grupoPersonaPersistenciaDTO);
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
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", grupoPersonaPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO AddPersonasGrupo(GrupoPersonasPersistenciaDTO grupoPersonasPersistenciaDTO, string userLogin)
        {
            try
            {
                foreach (var personaId in grupoPersonasPersistenciaDTO.PersonaIds)
                {
                    var result = _unitOfWork.GrupoPersonaRepository
                        .GetByFilter(x => x.GrupoId == grupoPersonasPersistenciaDTO.GrupoId
                                          && x.PersonaId == personaId, null, null, false);

                    if (result == null || !result.Any())
                    {
                        _unitOfWork.GrupoPersonaRepository
                           .Add(new GrupoPersona
                           {
                               GrupoId = grupoPersonasPersistenciaDTO.GrupoId,
                               PersonaId = personaId,
                               User = userLogin,
                               EstaEliminado = false
                           });

                        _logger.Information($"Add Grupo {grupoPersonasPersistenciaDTO.GrupoId} " +
                            $"- Persona {personaId} " +
                            $"- User: {userLogin}", grupoPersonasPersistenciaDTO);
                    }
                    else
                    {
                        var grupoPersona = result.FirstOrDefault();

                        grupoPersona.EstaEliminado = false;

                        _unitOfWork.GrupoPersonaRepository.Update(grupoPersona);

                        _logger.Information($"Update Grupo {grupoPersonasPersistenciaDTO.GrupoId} " +
                            $"- Persona {personaId} " +
                            $"- User: {userLogin}", grupoPersonasPersistenciaDTO);
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
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", grupoPersonasPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO GetByPersonasAsignadas(GrupoPersonaFilterDTO filterDTO)
        {
            try
            {
                var result = _unitOfWork.GrupoPersonaRepository
                    .GetByFilter(x => !x.EstaEliminado && x.Persona.Cuil != "99999999999"
                                                       && x.GrupoId == filterDTO.GrupoId
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

        public ResultDTO GetByPersonasNoAsignadas(GrupoPersonaFilterDTO filterDTO)
        {
            try
            {
                var result = _unitOfWork.GrupoPersonaRepository
                    .GetByFilter(x => !x.EstaEliminado && x.Persona.Cuil != "99999999999"
                                                       && x.GrupoId == filterDTO.GrupoId
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

        public ResultDTO DeletePersonaGrupo(GrupoPersonaPersistenciaDTO grupoPersonaPersistenciaDTO, string userLogin)
        {
            try
            {
                var result = _unitOfWork.GrupoPersonaRepository
                    .GetByFilter(x => x.GrupoId == grupoPersonaPersistenciaDTO.GrupoId && x.PersonaId == grupoPersonaPersistenciaDTO.PersonaId, null, null, false);

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

                _unitOfWork.GrupoPersonaRepository.Update(grupoPersona);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Grupo - Persona => GrupoId: {grupoPersonaPersistenciaDTO.GrupoId} " +
                        $"- PersonaId: {grupoPersonaPersistenciaDTO.PersonaId}" +
                        $"- User: {userLogin}", grupoPersonaPersistenciaDTO);
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
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", grupoPersonaPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO DeletePersonasGrupo(GrupoPersonasPersistenciaDTO grupoPersonasPersistenciaDTO, string userLogin)
        {
            try
            {
                foreach (var personaId in grupoPersonasPersistenciaDTO.PersonaIds)
                {
                    var result = _unitOfWork.GrupoPersonaRepository
                        .GetByFilter(x => x.GrupoId == grupoPersonasPersistenciaDTO.GrupoId && x.PersonaId == personaId, null, null, false);

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

                    _unitOfWork.GrupoPersonaRepository.Update(grupoPersona);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Delete Grupo - Persona => GrupoId: {grupoPersonasPersistenciaDTO.GrupoId} " +
                            $"- PersonaId: {personaId}" +
                            $"- User: {userLogin}", grupoPersonasPersistenciaDTO);
                    }
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
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", grupoPersonasPersistenciaDTO);
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

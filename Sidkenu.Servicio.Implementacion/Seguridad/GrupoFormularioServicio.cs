using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Formulario;
using Sidkenu.Servicio.DTOs.Seguridad.GrupoFormulario;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class GrupoFormularioServicio : ServicioBase, IGrupoFormularioServicio
    {
        public GrupoFormularioServicio(IUnidadDeTrabajo unitOfWork,
                                       IMapper mapper,
                                       ILogger logger,
                                       IConfiguracionServicio configuracionServicio)
                                       : base(unitOfWork, mapper, logger, configuracionServicio)
        {
        }


        public ResultDTO AddFormularioGrupo(GrupoFormularioPersistenciaDTO grupoFormularioPersistenciaDTO, string userLogin)
        {
            try
            {
                var result = _unitOfWork.GrupoFormularioRepository
                    .GetByFilter(x => x.GrupoId == grupoFormularioPersistenciaDTO.GrupoId
                                      && x.FormularioId == grupoFormularioPersistenciaDTO.FormularioId, null, null, false);

                if (result == null || !result.Any())
                {
                    _unitOfWork.GrupoFormularioRepository
                       .Add(new GrupoFormulario
                       {
                           GrupoId = grupoFormularioPersistenciaDTO.GrupoId,
                           FormularioId = grupoFormularioPersistenciaDTO.FormularioId,
                           User = userLogin,
                           EstaEliminado = false
                       });

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Add Grupo {grupoFormularioPersistenciaDTO.GrupoId} " +
                            $"- Formulario {grupoFormularioPersistenciaDTO.FormularioId} " +
                            $"- User: {userLogin}", grupoFormularioPersistenciaDTO);
                    }
                }
                else
                {
                    var grupoFormulario = result.FirstOrDefault();

                    grupoFormulario.EstaEliminado = false;

                    _unitOfWork.GrupoFormularioRepository.Update(grupoFormulario);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Update Grupo {grupoFormularioPersistenciaDTO.GrupoId} " +
                            $"- Formulario {grupoFormularioPersistenciaDTO.FormularioId} " +
                            $"- User: {userLogin}", grupoFormularioPersistenciaDTO);
                    }
                }

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Message = "El formulario se asigno correctamente"
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", grupoFormularioPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO AddFormulariosGrupo(GrupoFormulariosPersistenciaDTO grupoFormulariosPersistenciaDTO, string userLogin)
        {
            try
            {
                foreach (var formularioId in grupoFormulariosPersistenciaDTO.FormularioIds)
                {
                    var result = _unitOfWork.GrupoFormularioRepository
                        .GetByFilter(x => x.GrupoId == grupoFormulariosPersistenciaDTO.GrupoId
                                          && x.FormularioId == formularioId, null, null, false);

                    if (result == null || !result.Any())
                    {
                        _unitOfWork.GrupoFormularioRepository
                           .Add(new GrupoFormulario
                           {
                               GrupoId = grupoFormulariosPersistenciaDTO.GrupoId,
                               FormularioId = formularioId,
                               User = userLogin,
                               EstaEliminado = false
                           });

                        _logger.Information($"Add Grupo {grupoFormulariosPersistenciaDTO.GrupoId} " +
                                $"- Formulario {formularioId} " +
                                $"- User: {userLogin}", grupoFormulariosPersistenciaDTO);
                    }
                    else
                    {
                        var grupoFormulario = result.FirstOrDefault();

                        grupoFormulario.EstaEliminado = false;

                        _unitOfWork.GrupoFormularioRepository.Update(grupoFormulario);

                        if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                        {
                            _logger.Information($"Update Grupo {grupoFormulariosPersistenciaDTO.GrupoId} " +
                                $"- Formulario {formularioId} " +
                                $"- User: {userLogin}", grupoFormulariosPersistenciaDTO);
                        }
                    }
                }

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Message = "Los formularios se asignaron correctamente"
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", grupoFormulariosPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO GetByFormulariosAsignadas(GrupoFormularioFilterDTO filterDTO)
        {
            try
            {
                var result = _unitOfWork.GrupoFormularioRepository
                    .GetByFilter(x => !x.EstaEliminado && x.GrupoId == filterDTO.GrupoId
                                                       && x.Formulario.DescripcionCompleta.ToLower().Contains(filterDTO.CadenaBuscar.ToLower())

                        , o => o.OrderBy(r => r.Formulario.Codigo)
                        , i => i.Include(z => z.Formulario));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<FormularioDTO>>(result)
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

        public ResultDTO GetByFormulariosNoAsignadas(GrupoFormularioFilterDTO filterDTO)
        {
            try
            {
                var result = _unitOfWork.GrupoFormularioRepository
                    .GetByFilter(x => !x.EstaEliminado && x.GrupoId == filterDTO.GrupoId
                                                       && x.Formulario.DescripcionCompleta.ToLower().Contains(filterDTO.CadenaBuscar.ToLower())
                        , o => o.OrderBy(r => r.Formulario.Codigo)
                        , i => i.Include(z => z.Formulario));

                var formulariosAsignadas = result.Select(x => x.Formulario);

                var formularios = _unitOfWork.FormularioRepository.GetAll();

                var formulariosNoAsignados = formularios.Except(formulariosAsignadas, new GenericCompare<Formulario>());

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<FormularioDTO>>(formulariosNoAsignados
                                            .Where(x => x.DescripcionCompleta.ToLower().Contains(filterDTO.CadenaBuscar.ToLower()))
                                            .ToList())
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

        public ResultDTO DeleteFormularioGrupo(GrupoFormularioPersistenciaDTO grupoFormularioPersistenciaDTO, string userLogin)
        {
            try
            {
                var result = _unitOfWork.GrupoFormularioRepository
                    .GetByFilter(x => x.GrupoId == grupoFormularioPersistenciaDTO.GrupoId && x.FormularioId == grupoFormularioPersistenciaDTO.FormularioId, null, null, false);

                if (result == null || !result.Any())
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "No se encontro el formulario asignado al grupo"
                    };
                }

                var grupoFormulario = result.FirstOrDefault();

                grupoFormulario.EstaEliminado = true;

                _unitOfWork.GrupoFormularioRepository.Update(grupoFormulario);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Grupo - Formulario => GrupoId: {grupoFormularioPersistenciaDTO.GrupoId} " +
                        $"- FormularioId: {grupoFormularioPersistenciaDTO.FormularioId}" +
                        $"- User: {userLogin}", grupoFormularioPersistenciaDTO);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "El formulario fue quitado correctamente"
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", grupoFormularioPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO DeleteFormulariosGrupo(GrupoFormulariosPersistenciaDTO grupoFormulariosPersistenciaDTO, string userLogin)
        {
            try
            {
                foreach (var formularioId in grupoFormulariosPersistenciaDTO.FormularioIds)
                {
                    var result = _unitOfWork.GrupoFormularioRepository
                        .GetByFilter(x => x.GrupoId == grupoFormulariosPersistenciaDTO.GrupoId && x.FormularioId == formularioId, null, null, false);

                    if (result == null || !result.Any())
                    {
                        return new ResultDTO
                        {
                            State = false,
                            Message = "No se encontro el formulario asignado al grupo"
                        };
                    }

                    var grupoFormulario = result.FirstOrDefault();

                    grupoFormulario.EstaEliminado = true;

                    _unitOfWork.GrupoFormularioRepository.Update(grupoFormulario);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Delete Grupo - Formulario => GrupoId: {grupoFormulariosPersistenciaDTO.GrupoId} " +
                            $"- FormularioId: {formularioId}" +
                            $"- User: {userLogin}", grupoFormulariosPersistenciaDTO);
                    }
                }

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Message = "Los formularios fueron quitados correctamente"
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", grupoFormulariosPersistenciaDTO);
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

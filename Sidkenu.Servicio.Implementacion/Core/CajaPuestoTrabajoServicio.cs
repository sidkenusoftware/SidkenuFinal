using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Caja;
using Sidkenu.Servicio.DTOs.Core.CajaPuestoTrabajo;
using Sidkenu.Servicio.DTOs.Seguridad.PuestoTrabajo;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class CajaPuestoTrabajoServicio : ServicioBase, ICajaPuestoTrabajoServicio
    {
        public CajaPuestoTrabajoServicio(IUnidadDeTrabajo unitOfWork,
                                         IMapper mapper,
                                         ILogger logger,
                                         IConfiguracionServicio configuracionServicio)
                                         : base(unitOfWork, mapper, logger, configuracionServicio)
        {
        }

        public ResultDTO AddPuestoTrabajoCaja(CajaPuestoTrabajoPersistenciaDTO cajaPuestoTrabajoPersistenciaDTO, string userLogin)
        {
            try
            {
                var result = _unitOfWork.CajaPuestoTrabajoRepository
                    .GetByFilter(x => x.CajaId == cajaPuestoTrabajoPersistenciaDTO.CajaId
                                      && x.PuestoTrabajoId == cajaPuestoTrabajoPersistenciaDTO.PuestoTrabajoId, null, null, false);

                if (result == null || !result.Any())
                {
                    _unitOfWork.CajaPuestoTrabajoRepository
                       .Add(new CajaPuestoTrabajo
                       {
                           CajaId = cajaPuestoTrabajoPersistenciaDTO.CajaId,
                           PuestoTrabajoId = cajaPuestoTrabajoPersistenciaDTO.PuestoTrabajoId,
                           User = userLogin,
                           EstaEliminado = false
                       });

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Add Caja {cajaPuestoTrabajoPersistenciaDTO.CajaId} " +
                            $"- PuestoTrabajo {cajaPuestoTrabajoPersistenciaDTO.PuestoTrabajoId} " +
                            $"- User: {userLogin}", cajaPuestoTrabajoPersistenciaDTO);
                    }
                }
                else
                {
                    var cajaPuestoTrabajo = result.FirstOrDefault();

                    cajaPuestoTrabajo.EstaEliminado = false;

                    _unitOfWork.CajaPuestoTrabajoRepository.Update(cajaPuestoTrabajo);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Update Caja {cajaPuestoTrabajoPersistenciaDTO.CajaId} " +
                            $"- PuestoTrabajo {cajaPuestoTrabajoPersistenciaDTO.PuestoTrabajoId} " +
                            $"- User: {userLogin}", cajaPuestoTrabajoPersistenciaDTO);
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
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", cajaPuestoTrabajoPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO AddPuestosTrabajosCaja(CajaPuestoTrabajosPersistenciaDTO cajaPuestoTrabajosPersistenciaDTO, string userLogin)
        {
            try
            {
                foreach (var formularioId in cajaPuestoTrabajosPersistenciaDTO.PuestoTrabajoIds)
                {
                    var result = _unitOfWork.CajaPuestoTrabajoRepository
                        .GetByFilter(x => x.CajaId == cajaPuestoTrabajosPersistenciaDTO.CajaId
                                          && x.PuestoTrabajoId == formularioId, null, null, false);

                    if (result == null || !result.Any())
                    {
                        _unitOfWork.CajaPuestoTrabajoRepository
                           .Add(new CajaPuestoTrabajo
                           {
                               CajaId = cajaPuestoTrabajosPersistenciaDTO.CajaId,
                               PuestoTrabajoId = formularioId,
                               User = userLogin,
                               EstaEliminado = false
                           });

                        _logger.Information($"Add Caja {cajaPuestoTrabajosPersistenciaDTO.CajaId} " +
                                $"- PuestoTrabajo {formularioId} " +
                                $"- User: {userLogin}", cajaPuestoTrabajosPersistenciaDTO);
                    }
                    else
                    {
                        var cajaPuestoTrabajo = result.FirstOrDefault();

                        cajaPuestoTrabajo.EstaEliminado = false;

                        _unitOfWork.CajaPuestoTrabajoRepository.Update(cajaPuestoTrabajo);

                        if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                        {
                            _logger.Information($"Update Caja {cajaPuestoTrabajosPersistenciaDTO.CajaId} " +
                                $"- PuestoTrabajo {formularioId} " +
                                $"- User: {userLogin}", cajaPuestoTrabajosPersistenciaDTO);
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
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", cajaPuestoTrabajosPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO GetByPuestosTrabajosAsignadas(CajaPuestoTrabajoFilterDTO filterDTO)
        {
            try
            {
                var result = _unitOfWork.CajaPuestoTrabajoRepository
                    .GetByFilter(x => !x.EstaEliminado && x.CajaId == filterDTO.CajaId
                                                       && x.PuestoTrabajo.Descripcion.ToLower().Contains(filterDTO.CadenaBuscar.ToLower())

                        , o => o.OrderBy(r => r.PuestoTrabajo.Descripcion)
                        , i => i.Include(z => z.PuestoTrabajo));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<PuestoTrabajoDTO>>(result)
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
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

        public ResultDTO GetByPuestosTrabajosNoAsignadas(CajaPuestoTrabajoFilterDTO filterDTO)
        {
            try
            {
                var result = _unitOfWork.CajaPuestoTrabajoRepository
                    .GetByFilter(x => !x.EstaEliminado && x.CajaId == filterDTO.CajaId
                                                       && x.Caja.EmpresaId == filterDTO.EmpresaId
                                                       && x.PuestoTrabajo.Descripcion.ToLower().Contains(filterDTO.CadenaBuscar.ToLower())
                        , o => o.OrderBy(r => r.PuestoTrabajo.Descripcion)
                        , i => i.Include(z => z.PuestoTrabajo));

                var puestosTrabajosAsignadas = result.Select(x => x.PuestoTrabajo);

                var puestosTrabajos = _unitOfWork.PuestoTrabajoRepository.GetByFilter(x=>x.EmpresaId == filterDTO.EmpresaId);

                var puestosTrabajosNoAsignados = puestosTrabajos.Except(puestosTrabajosAsignadas, new GenericCompare<PuestoTrabajo>());

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<PuestoTrabajoDTO>>(puestosTrabajosNoAsignados
                                            .Where(x => x.Descripcion.ToLower().Contains(filterDTO.CadenaBuscar.ToLower()))
                                            .ToList())
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
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

        public ResultDTO GetByCajasAsignadas(Guid puestoTrabajoId)
        {
            try
            {
                var result = _unitOfWork.CajaPuestoTrabajoRepository
                    .GetByFilter(x => !x.EstaEliminado && x.PuestoTrabajoId == puestoTrabajoId
                        , o => o.OrderBy(r => r.Caja.Descripcion)
                        , i => i.Include(z => z.Caja).ThenInclude(z=>z.CajaDetalles));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<CajaDTO>>(result)
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
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

        public ResultDTO DeleteCajaPuestoTrabajo(CajaPuestoTrabajoPersistenciaDTO cajaPuestoTrabajoPersistenciaDTO, string userLogin)
        {
            try
            {
                var result = _unitOfWork.CajaPuestoTrabajoRepository
                    .GetByFilter(x => x.CajaId == cajaPuestoTrabajoPersistenciaDTO.CajaId && x.PuestoTrabajoId == cajaPuestoTrabajoPersistenciaDTO.PuestoTrabajoId, null, null, false);

                if (result == null || !result.Any())
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "No se encontro el formulario asignado al grupo"
                    };
                }

                var cajaPuestoTrabajo = result.FirstOrDefault();

                cajaPuestoTrabajo.EstaEliminado = true;

                _unitOfWork.CajaPuestoTrabajoRepository.Update(cajaPuestoTrabajo);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Caja - PuestoTrabajo => CajaId: {cajaPuestoTrabajoPersistenciaDTO.CajaId} " +
                        $"- PuestoTrabajoId: {cajaPuestoTrabajoPersistenciaDTO.PuestoTrabajoId}" +
                        $"- User: {userLogin}", cajaPuestoTrabajoPersistenciaDTO);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "El formulario fue quitado correctamente"
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", cajaPuestoTrabajoPersistenciaDTO);
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO DeleteCajaPuestosTrabajos(CajaPuestoTrabajosPersistenciaDTO cajaPuestoTrabajosPersistenciaDTO, string userLogin)
        {
            try
            {
                foreach (var formularioId in cajaPuestoTrabajosPersistenciaDTO.PuestoTrabajoIds)
                {
                    var result = _unitOfWork.CajaPuestoTrabajoRepository
                        .GetByFilter(x => x.CajaId == cajaPuestoTrabajosPersistenciaDTO.CajaId && x.PuestoTrabajoId == formularioId, null, null, false);

                    if (result == null || !result.Any())
                    {
                        return new ResultDTO
                        {
                            State = false,
                            Message = "No se encontro el formulario asignado al grupo"
                        };
                    }

                    var cajaPuestoTrabajo = result.FirstOrDefault();

                    cajaPuestoTrabajo.EstaEliminado = true;

                    _unitOfWork.CajaPuestoTrabajoRepository.Update(cajaPuestoTrabajo);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Delete Caja - PuestoTrabajo => CajaId: {cajaPuestoTrabajosPersistenciaDTO.CajaId} " +
                            $"- PuestoTrabajoId: {formularioId}" +
                            $"- User: {userLogin}", cajaPuestoTrabajosPersistenciaDTO);
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
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.", cajaPuestoTrabajosPersistenciaDTO);
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

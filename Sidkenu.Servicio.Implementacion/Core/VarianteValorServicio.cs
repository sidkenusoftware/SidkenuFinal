using AutoMapper;
using FluentValidation;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Variante;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class VarianteValorServicio : ServicioBase, IVarianteValorServicio
    {
        public VarianteValorServicio(IUnidadDeTrabajo unitOfWork,
                              IMapper mapper,
                              ILogger logger,
                              IConfiguracionServicio configuracionServicio)
                              : base(unitOfWork, mapper, logger, configuracionServicio)
        {
        }

        public ResultDTO Add(ValorVariantePersistenciaDTO entidad, string user)
        {
            try
            {
                var existeEntidad = VerificarSiExiste(entidad.Codigo, entidad.Descripcion, entidad.VarianteId);

                if (existeEntidad)
                    return new ResultDTO
                    {
                        Message = "Los datos ingresados ya existen",
                        State = false,
                    };

                var entity = _mapper.Map<Dominio.Entidades.Core.VarianteValor>(entidad);

                entity.User = user;
                entity.EstaEliminado = false;

                _unitOfWork.VarianteValorRepository.Add(entity);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Variante Valor - User: {user} - {entity.GetPropValue()}");
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "Los datos se grabaron correctamente",
                    Data = entity
                };
            }
            catch (ValidationException ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error de validaciones {ErrorValidator.ObtenerErrores(ex.Errors)} - User: {user}.");
                }

                return new ResultDTO
                {
                    Message = ErrorValidator.ObtenerErrores(ex.Errors),
                    State = false
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {user}.");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }
        public ResultDTO Delete(ValorVarianteDeleteDTO deleteDTO, string user)
        {
            try
            {
                var entidad = _unitOfWork.VarianteValorRepository.GetById(deleteDTO.Id);

                if (entidad == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos de la Variante. Id: {deleteDTO.Id}");
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrio un error al obtener los datos",
                        State = false
                    };
                }

                _unitOfWork.VarianteValorRepository.DeleteFisico(deleteDTO.Id, user);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Variante - User: {user}", entidad);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = !entidad.EstaEliminado ? "Los datos se eliminaron correctamente" : "Los datos se recuperaron correctamente"
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {user}.");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }
        
        public ResultDTO GetAll(Guid escalaId)
        {
            try
            {
                var entities = _unitOfWork.VarianteValorRepository
                    .GetByFilter(x => x.Codigo.ToLower() != VarianteDefecto.VarianteValorCodigo && x.VarianteId == escalaId);

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ValorVarianteDTO>>(entities)
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

        // ------------------------------------------------------------------------------------------------------ //
        // ------------------------------             Metodos Privados              ----------------------------- //
        // ------------------------------------------------------------------------------------------------------ //
        private bool VerificarSiExiste(string codigo, string descripcion, Guid escalaId, Guid? id = null)
        {
            var entidads = _unitOfWork.VarianteValorRepository.GetAll();

            if (entidads != null && entidads.Any())
            {
                if (id == null)
                {
                    return entidads.Any(x => x.VarianteId == escalaId
                                             && (x.Codigo.ToLower() == codigo.ToLower() || x.Descripcion.ToLower() == descripcion.ToLower()));
                }
                else
                {
                    return entidads.Any(x => x.Id != id.Value
                        && x.VarianteId == escalaId
                        && (x.Codigo.ToLower() == codigo.ToLower() || x.Descripcion.ToLower() == descripcion.ToLower()));
                }
            }
            else
            {
                return false;
            }
        }
    }
}

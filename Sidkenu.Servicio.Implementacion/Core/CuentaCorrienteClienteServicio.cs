using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Cliente;
using Sidkenu.Servicio.DTOs.Core.CuentaCorrienteCliente;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class CuentaCorrienteClienteServicio : ServicioBase, ICuentaCorrienteClienteServicio
    {
        public CuentaCorrienteClienteServicio(IUnidadDeTrabajo unitOfWork,
                                              IMapper mapper,
                                              ILogger logger,
                                              IConfiguracionServicio configuracionServicio)
                                              : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            
        }

        public ResultDTO Add(CtaCteClientePersistenciaDTO entidad, string user)
        {
            try
            {
                var entity = _mapper.Map<Dominio.Entidades.Core.CuentaCorrienteCliente>(entidad);

                entity.User = user;

                _unitOfWork.CuentaCorrienteClienteRepository.Add(entity);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Cuenta Corriente Cliente - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<ClienteDTO>(entity),
                    Message = "Los datos se actualizaron correctamente"
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

        public ResultDTO GetByCliente(Guid clienteId)
        {
            try
            { 
                Expression<Func<Dominio.Entidades.Core.CuentaCorrienteCliente, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.ClienteId == clienteId);

                var entities = _unitOfWork.CuentaCorrienteClienteRepository
                    .GetByFilter(filtro, null, x => x.Include(z => z.Cliente));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<CtaCteClienteDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos de la Cliente. {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = $"Ocurrió un error grave al obtener los datos - {ex.Message}",
                    State = false
                };
            }
        }
    }
}

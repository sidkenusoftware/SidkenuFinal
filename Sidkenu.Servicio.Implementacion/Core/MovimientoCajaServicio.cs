using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Caja;
using Sidkenu.Servicio.DTOs.Core.Movimiento;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class MovimientoCajaServicio : ServicioBase, IMovimientoCajaServicio
    {
        public MovimientoCajaServicio(IUnidadDeTrabajo unitOfWork,
                                      IMapper mapper,
                                      ILogger logger,
                                      IConfiguracionServicio configuracionServicio)
                                      : base(unitOfWork, mapper, logger, configuracionServicio)
        {
        }

        public ResultDTO ObtenerMovimientos(Guid? cajaDetalleId, DateTime fechaDesde, DateTime fechaHasta)
        {
            var _esPrimeraPasada = true;

            try
            {
                var _fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);
                var _fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);

                Expression<Func<MovimientoCaja, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.Fecha >= _fechaDesde && x.Fecha <= _fechaHasta);

                if (cajaDetalleId.HasValue)
                {
                    filtro = filtro.And(x => x.CajaDetalleId == cajaDetalleId.Value);

                    var movimientos = _unitOfWork.MovimientoCajaRepository
                                                 .GetByFilter(filtro,
                                                              o => o.OrderByDescending(i => i.Fecha),
                                                              i => i.Include(z => z.CajaDetalle));

                    return new ResultDTO
                    {
                        State = true,
                        Data = _mapper.Map<IEnumerable<MovimientoCajaDTO>>(movimientos)
                    };
                }
                else
                {
                    return new ResultDTO
                    {
                        State = true,
                        Data = new List<MovimientoCajaDTO>()
                    };
                }
            }
            catch (ValidationException ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error de validaciones {ErrorValidator.ObtenerErrores(ex.Errors)}");
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
                    _logger.Error(ex, $"Error {ex.Message}");
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

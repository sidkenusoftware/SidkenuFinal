using AutoMapper;
using FluentValidation;
using Serilog;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Cliente;
using Sidkenu.Servicio.DTOs.Core.Comprobante;
using Sidkenu.Servicio.DTOs.Core.Gasto;
using Sidkenu.Servicio.DTOs.Seguridad.Persona;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class GastoServicio : ServicioBase, IGastosServicio
    {
        private readonly IValidator<GastosPersistenciaDTO> _validatorDto;
        private readonly IComprobanteServicio _comprobanteServicio;
        private readonly IClienteServicio _clienteServicio;
        private readonly IPersonaServicio _personaServicio;

        public GastoServicio(IUnidadDeTrabajo unitOfWork,
                             IMapper mapper,
                             ILogger logger,
                             IConfiguracionServicio configuracionServicio,
                             IValidator<GastosPersistenciaDTO> validatorDto,
                             IComprobanteServicio comprobanteServicio,
                             IClienteServicio clienteServicio,
                             IPersonaServicio personaServicio)
                             : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _validatorDto = validatorDto;
            _comprobanteServicio = comprobanteServicio;
            _clienteServicio = clienteServicio;
            _personaServicio = personaServicio;
        }

        public ResultDTO Add(GastosPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validacion de Datos de Gastos: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var entityGasto = _mapper.Map<Dominio.Entidades.Core.Gasto>(entidad);

                entityGasto.User = user;
                entityGasto.EstaEliminado = false;

                _unitOfWork.GastoRepository.Add(entityGasto);

                // Comprobante de Gastos

                var _clienteResult = _clienteServicio.GetConsumidorFinal();

                if (_clienteResult == null || !_clienteResult.State) 
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "Ocurrió un error al obtener el cliente por defecto para asignarlo al comprobante"
                    };
                }

                var _personaResult = _personaServicio.GetById(entidad.PersonaId);

                if (_personaResult == null || !_personaResult.State)
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "Ocurrió un error al obtener la persona que esta registrando el gasto"
                    };
                }

                var _comprobanteGasto = new ComprobanteGastoDTO();

                _comprobanteGasto.CajaDetalleId = entidad.CajaDetalleId;
                _comprobanteGasto.EmpresaId = entidad.EmpresaId;
                _comprobanteGasto.Cliente = (ClienteDTO)_clienteResult.Data;
                _comprobanteGasto.Persona = (PersonaDTO)_personaResult.Data;
                _comprobanteGasto.Fecha = entidad.Fecha;
                _comprobanteGasto.TipoGastoId = entidad.TipoGastoId;
                _comprobanteGasto.CajaId = entidad.CajaId;
                _comprobanteGasto.SubTotal = entidad.Monto;
                _comprobanteGasto.Descuento = 0m;
                _comprobanteGasto.Total = entidad.Monto;
                _comprobanteGasto.TipoComprobante = Aplicacion.Constantes.TipoComprobante.Gastos;
                _comprobanteGasto.Descripcion = entidad.Descripcion;
                                
                var resultDto = _comprobanteServicio.Add(_comprobanteGasto, user);

                var _comprobanteResult = (ComprobanteDTO)resultDto.Data;

                // Movimiento Caja

                var _movimientoCaja = new Dominio.Entidades.Core.MovimientoCaja
                {
                    TipoMovimiento = Aplicacion.Constantes.TipoMovimiento.Egreso,
                    TipoOperacion = Aplicacion.Constantes.TipoOperacionMovimiento.Gastos,
                    CajaDetalleId = entidad.CajaDetalleId,
                    Capital = entidad.Monto,
                    Interes = 0m,
                    Descripcion = entidad.Descripcion,
                    Fecha = entidad.Fecha,
                    User = user,
                    ComprobanteId = _comprobanteResult.Id,
                    EstaEliminado = false,                    
                };

                _unitOfWork.MovimientoCajaRepository.Add(_movimientoCaja);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Movimiento Gastos - User: {user}", entityGasto);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "Los datos se grabaron correctamente",
                    Data = entityGasto
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
    }
}

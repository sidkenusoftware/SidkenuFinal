using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Caja;
using Sidkenu.Servicio.DTOs.Core.Cliente;
using Sidkenu.Servicio.DTOs.Core.Comprobante;
using Sidkenu.Servicio.DTOs.Seguridad.Persona;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Implementacion.Seguridad;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class CajaServicio : ServicioBase, ICajaServicio
    {
        private readonly IValidator<CajaPersistenciaDTO> _validatorDto;

        private readonly IComprobanteServicio _comprobanteServicio;
        private readonly IClienteServicio _clienteServicio;
        private readonly IPersonaServicio _personaServicio;

        public CajaServicio(IUnidadDeTrabajo unitOfWork,
                            IMapper mapper,
                            ILogger logger,
                            IConfiguracionServicio configuracionServicio,  
                            IComprobanteServicio comprobanteServicio,
                            IValidator<CajaPersistenciaDTO> validatorDto,
                            IClienteServicio clienteServicio,
                            IPersonaServicio personaServicio)
                            : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _validatorDto = validatorDto;
            _comprobanteServicio = comprobanteServicio;
            _clienteServicio = clienteServicio;
            _personaServicio = personaServicio;
        }

        public ResultDTO Add(CajaPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validacion de Datos de la Caja: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var existeEntidad = VerificarSiExiste(entidad.Descripcion, entidad.EmpresaId);

                if (existeEntidad)
                    return new ResultDTO
                    {
                        Message = "Los datos ingresados ya existen",
                        State = false,
                    };

                var entity = _mapper.Map<Caja>(entidad);

                entity.User = user;
                entity.EstaEliminado = false;

                _unitOfWork.CajaRepository.Add(entity);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Caja - User: {user}", entity);
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

        public ResultDTO Update(CajaPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validacion de Datos de la Caja: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var entityActual = _unitOfWork.CajaRepository.GetById(entidad.Id);

                if (entityActual == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information("Ocurrio un error al obtener los datos de la Caja", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrio un error al obtener los datos del Caja",
                        State = false
                    };

                }

                if (entityActual.EstaEliminado)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information("No se puede Actualizar los datos porque la entidad seleccionada se encuentra eliminada", entityActual);
                    }

                    return new ResultDTO
                    {
                        Message = "No se puede Actualizar los datos porque la entidad seleccionada se encuentra eliminada",
                        State = false,
                    };
                }

                var entity = _mapper.Map<Caja>(entidad);

                entity.User = user;

                _unitOfWork.CajaRepository.Update(entity);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Update Caja - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<CajaDTO>(entity),
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

        public ResultDTO Delete(CajaDeleteDTO deleteDTO, string user)
        {
            try
            {
                var entidad = _unitOfWork.CajaRepository.GetById(deleteDTO.Id);

                if (entidad == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos de la Caja. Id: {deleteDTO.Id}", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrio un error al obtener los datos",
                        State = false
                    };
                }

                _unitOfWork.CajaRepository.Delete(deleteDTO.Id, user);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Caja - User: {user}", entidad);
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

        public ResultDTO Transferir(CajaTransferenciaDTO cajaTransferencia, string user)
        {
            try
            {
                var fechaActual = DateTime.Now;

                var cajaOrigen = _unitOfWork.CajaRepository.GetById(cajaTransferencia.CajaOrigenId,
                    i=>i.Include(z=>z.CajaDetalles));
                
                var cajaDestino = _unitOfWork.CajaRepository.GetById(cajaTransferencia.CajaDestinoId,
                    i => i.Include(z => z.CajaDetalles));

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

                var _personaResult = _personaServicio.GetById(cajaTransferencia.PersonaId);

                if (_personaResult == null || !_personaResult.State)
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "Ocurrió un error al obtener la persona que esta registrando el gasto"
                    };
                }

                // Caja Origen

                var _comprobanteTransferenciaOrigen = new ComprobanteTransferenciaDTO();

                _comprobanteTransferenciaOrigen.CajaDetalleId = cajaOrigen.CajaDetalles.First(x=>x.EstadoCaja == Aplicacion.Constantes.EstadoCaja.Abierta).Id;

                _comprobanteTransferenciaOrigen.EmpresaId = cajaTransferencia.EmpresaId;

                _comprobanteTransferenciaOrigen.Cliente = (ClienteDTO)_clienteResult.Data;
                _comprobanteTransferenciaOrigen.Persona = (PersonaDTO)_personaResult.Data;
                _comprobanteTransferenciaOrigen.Fecha = fechaActual;
                _comprobanteTransferenciaOrigen.CajaId = cajaOrigen.Id;
                _comprobanteTransferenciaOrigen.SubTotal = cajaTransferencia.Monto;
                _comprobanteTransferenciaOrigen.Descuento = 0m;
                _comprobanteTransferenciaOrigen.Total = cajaTransferencia.Monto;
                _comprobanteTransferenciaOrigen.TipoComprobante = Aplicacion.Constantes.TipoComprobante.TransferenciaCaja;
                _comprobanteTransferenciaOrigen.Descripcion = $"Transf. {cajaOrigen.Descripcion} a {cajaDestino.Descripcion} un monto {cajaTransferencia.Monto.ToString("C")}";

                var resultDtoOrigen = _comprobanteServicio.Add(_comprobanteTransferenciaOrigen, user);

                var _comprobanteResultOrigen = (ComprobanteDTO)resultDtoOrigen.Data;

                // Movimiento Caja Origen

                var _movimientoCajaOrigen = new Dominio.Entidades.Core.MovimientoCaja
                {
                    TipoMovimiento = Aplicacion.Constantes.TipoMovimiento.Egreso,
                    TipoOperacion = Aplicacion.Constantes.TipoOperacionMovimiento.TransferenciaCaja,
                    CajaDetalleId = _comprobanteTransferenciaOrigen.CajaDetalleId,
                    Capital = cajaTransferencia.Monto,
                    Interes = 0m,
                    Descripcion = _comprobanteTransferenciaOrigen.Descripcion,
                    Fecha = fechaActual,
                    User = user,
                    ComprobanteId = _comprobanteResultOrigen.Id,
                    EstaEliminado = false,
                };

                _unitOfWork.MovimientoCajaRepository.Add(_movimientoCajaOrigen);

                // Caja Destino

                var _comprobanteTransferenciaDestino = new ComprobanteTransferenciaDTO();

                _comprobanteTransferenciaDestino.CajaDetalleId = cajaDestino.CajaDetalles.First(x => x.EstadoCaja == Aplicacion.Constantes.EstadoCaja.Abierta).Id;

                _comprobanteTransferenciaDestino.EmpresaId = cajaTransferencia.EmpresaId;

                _comprobanteTransferenciaDestino.Cliente = (ClienteDTO)_clienteResult.Data;
                _comprobanteTransferenciaDestino.Persona = (PersonaDTO)_personaResult.Data;
                _comprobanteTransferenciaDestino.Fecha = fechaActual;
                _comprobanteTransferenciaDestino.CajaId = cajaDestino.Id;
                _comprobanteTransferenciaDestino.SubTotal = cajaTransferencia.Monto;
                _comprobanteTransferenciaDestino.Descuento = 0m;
                _comprobanteTransferenciaDestino.Total = cajaTransferencia.Monto;
                _comprobanteTransferenciaDestino.TipoComprobante = Aplicacion.Constantes.TipoComprobante.TransferenciaCaja;
                _comprobanteTransferenciaDestino.Descripcion = $"Transf. {cajaOrigen.Descripcion} a {cajaDestino.Descripcion} un monto {cajaTransferencia.Monto.ToString("C")}";

                var resultDtoDestino = _comprobanteServicio.Add(_comprobanteTransferenciaDestino, user);

                var _comprobanteResultDestino = (ComprobanteDTO)resultDtoDestino.Data;

                // Movimiento Caja Destino

                var _movimientoCajaDestino = new Dominio.Entidades.Core.MovimientoCaja
                {
                    TipoMovimiento = Aplicacion.Constantes.TipoMovimiento.Ingreso,
                    TipoOperacion = Aplicacion.Constantes.TipoOperacionMovimiento.TransferenciaCaja,
                    CajaDetalleId = _comprobanteTransferenciaDestino.CajaDetalleId,
                    Capital = cajaTransferencia.Monto,
                    Interes = 0m,
                    Descripcion = _comprobanteTransferenciaDestino.Descripcion,
                    Fecha = fechaActual,
                    User = user,
                    ComprobanteId = _comprobanteResultDestino.Id,
                    EstaEliminado = false,
                };

                _unitOfWork.MovimientoCajaRepository.Add(_movimientoCajaDestino);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Movimiento Gastos - User: {user}", cajaTransferencia);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "Los datos se grabaron correctamente",
                    Data = null
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

        public ResultDTO GetAll(Guid empresaId)
        {
            try
            {
                var entities = _unitOfWork.CajaRepository.GetByFilter(x => x.EmpresaId == empresaId);

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<CajaDTO>>(entities)
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

        public ResultDTO GetByFilter(CajaFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<Caja, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == filter.EmpresaId);

                if (filter.CadenaBuscar.IndexOf(SeparacionFiltroBusqueda.CaracterSeparador) != -1)
                {
                    var primeraPasada = true;

                    var listaCadenas = filter.CadenaBuscar.Split(SeparacionFiltroBusqueda.CaracterSeparador, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cadena in listaCadenas)
                    {
                        if (primeraPasada)
                        {
                            filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && x.Descripcion.ToLower().Contains(cadena.ToLower()));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                && x.Descripcion.ToLower().Contains(cadena.ToLower()));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && x.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower()));
                }

                var entities = _unitOfWork.CajaRepository.GetByFilter(filtro, null, 
                                                         i => i.Include(z => z.CajaDetalles).ThenInclude(z=>z.Movimientos));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<CajaDTO>>(entities)
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

        public ResultDTO GetById(Guid id)
        {
            try
            {
                var entity = _unitOfWork.CajaRepository.GetById(id);

                if (entity == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos de la Caja. Id: {id}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = "No se encontro el dato solicitado"
                    };
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<CajaDTO>(entity)
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

        public ResultDTO AbrirCaja(CajaAperturaDTO cajaAperturaDTO, string user)
        {
            try
            {
                var detalle = new CajaDetalle
                {
                    CajaId = cajaAperturaDTO.Id,
                    MontoApertura = cajaAperturaDTO.Monto,
                    FechaApertura = DateTime.Now,
                    PersonaAperturaId = cajaAperturaDTO.PersonaAperturaId,
                    Diferencia = 0m,
                    EstadoCaja = Aplicacion.Constantes.EstadoCaja.Abierta,
                    FechaCierre = null,
                    PersonaCierreId = null,
                    MontoCierre = null,
                    User = user,
                    EstaEliminado = false,
                    MontoSistema = 0m
                };

                _unitOfWork.CajaDetalleRepository.Add(detalle);

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Message = "La caja se abrió correctamente",
                    Data = detalle
                };
            }
            catch (Exception ex)
            {
                return new ResultDTO
                {
                    State = false,
                    Message = $"Ocurrio un error al abrir la Caja. {ex.Message}",
                };
            }
        }

        public ResultDTO GetDetalle(Guid cajaId, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                var fechaInicio = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);

                var fechaFin = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 23, 59);

                Expression<Func<CajaDetalle, bool>> filter = filter => true;

                filter = filter.And(x => x.CajaId == cajaId
                                       && x.FechaApertura.Date >= fechaInicio && x.FechaApertura.Date <= fechaFin);

                var result = _unitOfWork.CajaDetalleRepository
                                        .GetByFilter(filter, null, i => i.Include(z => z.PersonaApertura)
                                                                         .Include(z => z.PersonaCierre)
                                                                         .Include(z => z.Movimientos));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<List<CajaDetalleDTO>>(result)
                };
            }
            catch (Exception ex)
            {
                return new ResultDTO
                {
                    State = false,
                    Message = $"Ocurrió un error al obtener el detalle. {ex.Message}",
                };
            }
        }

        public ResultDTO GetDetalle(Guid cajaDetalleId)
        {
            try
            {
                Expression<Func<CajaDetalle, bool>> filter = filter => true;

                filter = filter.And(x => x.Id == cajaDetalleId && x.EstadoCaja == Aplicacion.Constantes.EstadoCaja.Abierta);

                var result = _unitOfWork.CajaDetalleRepository
                                        .GetByFilter(filter, null, i => i.Include(z => z.PersonaApertura)
                                                                         .Include(z => z.PersonaCierre)
                                                                         .Include(z => z.Movimientos));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<CajaDetalleDTO>(result.FirstOrDefault())
                };
            }
            catch (Exception ex)
            {
                return new ResultDTO
                {
                    State = false,
                    Message = $"Ocurrió un error al obtener el detalle. {ex.Message}",
                };
            }
        }

        public ResultDTO GetUltimaCajaAbierta(Guid empresaId)
        {
            try
            {
                var result = _unitOfWork.CajaDetalleRepository
                    .GetByFilter(x => x.Caja.EmpresaId == empresaId && x.EstadoCaja == Aplicacion.Constantes.EstadoCaja.Abierta,
                                      null,
                                      i => i.Include(z => z.Caja)
                                            .Include(z => z.Movimientos));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<List<CajaDetalleDTO>>(result),
                };
            }
            catch (Exception)
            {
                return new ResultDTO
                {
                    State = false,
                    Data = null,
                    Message = "Ocurrió un error al obtener la ultima caja Abierta"
                };
            }
        }

        public ResultDTO GetUltimaCajaAbierta(Guid empresaId, Guid cajaId)
        {
            try
            {
                var result = _unitOfWork.CajaDetalleRepository
                    .GetByFilter(x => x.Caja.EmpresaId == empresaId
                                      && x.CajaId == cajaId
                                      && x.EstadoCaja == Aplicacion.Constantes.EstadoCaja.Abierta,
                                      null,
                                      i => i.Include(z => z.Caja));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<List<CajaDetalleDTO>>(result),
                };
            }
            catch (Exception)
            {
                return new ResultDTO
                {
                    State = false,
                    Data = null,
                    Message = "Ocurrió un error al obtener la ultima caja Abierta"
                };
            }
        }

        public ResultDTO CerrarCaja(CajaCerrarDTO cajaCerrarDTO, string user)
        {
            try
            {
                var resultCajaDetalleOrigen = _unitOfWork.CajaDetalleRepository
                    .GetByFilter(x => x.CajaId == cajaCerrarDTO.Id && x.EstadoCaja == Aplicacion.Constantes.EstadoCaja.Abierta);

                if (resultCajaDetalleOrigen == null || !resultCajaDetalleOrigen.Any())
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "Ocurrio un error al obtener el detalle de la Caja seleccionada"
                    };
                }

                var cajaDetalleOrigen = resultCajaDetalleOrigen.First();

                if (cajaDetalleOrigen == null)
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "Ocurrio un error al obtener el detalle de la Caja seleccionada"
                    };
                }

                var fechaOperacion = DateTime.Now;

                cajaDetalleOrigen.FechaCierre = fechaOperacion;
                cajaDetalleOrigen.EstadoCaja = Aplicacion.Constantes.EstadoCaja.Cerrada;
                cajaDetalleOrigen.MontoCierre = cajaCerrarDTO.MontoCierre;
                cajaDetalleOrigen.MontoSistema = cajaCerrarDTO.MontoSistema;
                cajaDetalleOrigen.Diferencia = cajaCerrarDTO.Diferencia;
                cajaDetalleOrigen.PersonaCierreId = cajaCerrarDTO.PersonaCierreId;

                if (cajaCerrarDTO.EstaPorTransferirDinero)
                {
                    cajaDetalleOrigen.Movimientos = new List<MovimientoCaja>();

                    //cajaDetalleOrigen.Movimientos.Add(new MovimientoCaja
                    //{
                    //    CajaDetalleId = cajaDetalleOrigen.Id,
                    //    EstaEliminado = false,
                    //    Fecha = fechaOperacion,
                    //    Monto = cajaCerrarDTO.MontoTransferir,
                    //    TipoMovimiento = Aplicacion.Constantes.TipoMovimiento.Egreso,
                    //    Descripcion = $"Nro de Transf: {1}"
                    //});

                    var cajaDetalleDestino = _unitOfWork.CajaDetalleRepository.GetById(cajaCerrarDTO.CajaDetalleId);

                    if (cajaDetalleDestino == null)
                    {
                        return new ResultDTO
                        {
                            State = false,
                            Message = "Ocurrio un error al obtener el detalle de la Caja seleccionada para realizar la Transferencia"
                        };
                    }

                    cajaDetalleDestino.Movimientos = new List<MovimientoCaja>();

                    //cajaDetalleDestino.Movimientos.Add(new MovimientoCaja
                    //{
                    //    CajaDetalleId = cajaCerrarDTO.Id,
                    //    EstaEliminado = false,
                    //    Fecha = fechaOperacion,
                    //    Monto = cajaCerrarDTO.MontoTransferir,
                    //    TipoMovimiento = Aplicacion.Constantes.TipoMovimiento.Ingreso,
                    //    Descripcion = $"Nro de Transf: {1}"
                    //});

                    _unitOfWork.CajaDetalleRepository.Update(cajaDetalleDestino);
                }

                _unitOfWork.CajaDetalleRepository.Update(cajaDetalleOrigen);

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Message = "La caja se cerró correctamente",
                };
            }
            catch (Exception ex)
            {
                return new ResultDTO
                {
                    State = false,
                    Message = $"Ocurrió un error al cerrar la caja (turno). {ex.Message}"
                };
            }
        }

        public ResultDTO GetCajasParaHacerTransferencia(Guid empresaId)
        {
            try
            {
                Expression<Func<CajaDetalle, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.Caja.EmpresaId == empresaId
                                         && !x.EstaEliminado
                                         && x.EstadoCaja == Aplicacion.Constantes.EstadoCaja.Abierta);

                var entities = _unitOfWork.CajaDetalleRepository.GetByFilter(filtro, null, i => i.Include(z => z.Caja));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<CajaDTO>>(entities)
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

        public bool VerificarSiEstaAbiertaCaja(Guid empresaId)
        {
            try
            {
                var cajas = _unitOfWork.CajaDetalleRepository
                                        .GetByFilter(x => x.Caja.EmpresaId == empresaId
                                                          && x.EstadoCaja == Aplicacion.Constantes.EstadoCaja.Abierta,
                                                          o => o.OrderBy(z => z.Caja.Descripcion),
                                                          i => i.Include(z => z.Caja));

                return cajas.Any();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool VerificarSiEstaAbiertaCaja(Guid empresaId, Guid cajaId)
        {
            try
            {
                var cajas = _unitOfWork.CajaDetalleRepository
                                        .GetByFilter(x => x.Caja.EmpresaId == empresaId
                                                          && x.CajaId == cajaId
                                                          && x.EstadoCaja == Aplicacion.Constantes.EstadoCaja.Abierta,
                                                          o => o.OrderBy(z => z.Caja.Descripcion),
                                                          i => i.Include(z => z.Caja));

                return cajas.Any();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int ObtenerCantidadCajasAbiertas(Guid empresaId)
        {
            try
            {
                var cajas = _unitOfWork.CajaDetalleRepository
                                        .GetByFilter(x => x.Caja.EmpresaId == empresaId                                                          
                                                          && x.EstadoCaja == Aplicacion.Constantes.EstadoCaja.Abierta,
                                                          o => o.OrderBy(z => z.Caja.Descripcion),
                                                          i => i.Include(z => z.Caja));

                return cajas.Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        // ------------------------------------------------------------------------------------------------------ //
        // ------------------------------             Metodos Privados              ----------------------------- //
        // ------------------------------------------------------------------------------------------------------ //

        private bool VerificarSiExiste(string descripcion, Guid empresaId, Guid? id = null)
        {
            var entidads = _unitOfWork.FamiliaRepository.GetAll();

            if (entidads != null && entidads.Any())
            {
                if (id == null)
                {

                    return entidads.Any(x => x.EmpresaId == empresaId && x.Descripcion.ToLower() == descripcion.ToLower());
                }
                else
                {
                    return entidads.Any(x => x.Id != id.Value
                        && x.EmpresaId == empresaId
                        && x.Descripcion.ToLower() == descripcion.ToLower());
                }
            }
            else
            {
                return false;
            }
        }

        
    }
}

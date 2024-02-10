using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Articulo;
using Sidkenu.Servicio.DTOs.Core.ArticuloPrecio;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class ArticuloServicio : ServicioBase, IArticuloServicio
    {
        private readonly IValidator<ArticuloPersistenciaDTO> _validatorDto;
        private readonly IArticuloPrecioServicio _articuloPrecioServicio;

        public ArticuloServicio(IUnidadDeTrabajo unitOfWork,
                                IMapper mapper,
                                ILogger logger,
                                IConfiguracionServicio configuracionServicio,
                                IArticuloPrecioServicio articuloPrecioServicio,
                                IValidator<ArticuloPersistenciaDTO> validatorDto)
                                : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _validatorDto = validatorDto;
            _articuloPrecioServicio = articuloPrecioServicio;
        }

        public ResultDTO Add(ArticuloPersistenciaDTO entidad, string user)
        {
            try
            {
                // ======================================================================================================== //
                // ==================                            ADD ARTICULO                           =================== //
                // ======================================================================================================== //

                var _fechaActualizacion = DateTime.Now;

                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validacion de Datos del Articulo: {mensaje}");
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

                var entity = _mapper.Map<Articulo>(entidad);

                entity.User = user;
                entity.EstaEliminado = false;
                entity.EmpresaId = entidad.SeleccionoEmpresa ? entidad.EmpresaId : null;
                entity.PrecioCosto = entidad.PrecioCostoInicial.HasValue ? entidad.PrecioCostoInicial.Value : 0;

                // Aplico la variante por defecto

                var _varianteValorDefault = _unitOfWork.VarianteValorRepository
                    .GetByFilter(x => x.Codigo.ToLower() == VarianteDefecto.VarianteValorCodigo);

                if (_varianteValorDefault == null || !_varianteValorDefault.Any())
                {
                    return new ResultDTO { State = false, Message = "Ocurrio un error al obtener la variante por defecto" };
                }

                entity.VarianteValorUnoId = _varianteValorDefault.First().Id;
                entity.VarianteValorDosId = _varianteValorDefault.First().Id;

                _unitOfWork.ArticuloRepository.Add(entity);

                // ======================================================================================================== //
                // ==================                           ASIGNO A PROVEEDORES                    =================== //
                // ======================================================================================================== //

                if (entidad.ArticuloProveedores.Any())
                {
                    foreach (var articuloProveedor in entidad.ArticuloProveedores.ToList())
                    {
                        var _articuloProveedorNuevo = new ArticuloProveedor
                        {
                            ArticuloId = entity.Id,
                            ProveedorId = articuloProveedor.ProveedorId.Value,
                            CodigoProveedor = articuloProveedor.CodigoProveedor,
                            EstaEliminado = false,
                            User = user,
                        };
                        _unitOfWork.ArticuloProveedorRepository.Add(_articuloProveedorNuevo);
                    }
                }

                // ======================================================================================================== //
                // ==================                           ASIGNO A DEPOSITO                       =================== //
                // ======================================================================================================== //
                
                // Asigno el articulo a todos los depositos de la empresa y solo al que selecciono le aplico el stock inicial

                var _depositoResult = _unitOfWork.DepositoRepository
                    .GetByFilter(x => x.EmpresaId == entidad.EmpresaId);

                Parallel.ForEach(_depositoResult, x =>
                {
                    var _articuloDepositoNuevo = new ArticuloDeposito
                    {
                        ArticuloId = entity.Id,
                        DepositoId = entidad.DepositoId.Value,
                        EstaEliminado = false,
                        User = user,
                        Cantidad = x.Id == entidad.DepositoId.Value ? entidad.StockInicial.Value : 0
                    };

                    _unitOfWork.ArticuloDepositoRepository.Add(_articuloDepositoNuevo);
                });

                // ======================================================================================================== //
                // ==================                           CALCULO  PRECIO                         =================== //
                // ======================================================================================================== //

                var articuloPrecio = new ArticuloPrecioPersistenciaDTO
                {
                    EmpresaId = entidad.EmpresaId.Value,
                    ArticuloId = entity.Id,
                    MarcaId = entidad.MarcaId,
                    FamiliaId = entidad.FamiliaId,
                    PrecioCostoArticulo = entidad.PrecioCostoInicial,
                    RentabilidadArticulo = entidad.Rentabilidad,
                    TieneRentabilidad = entidad.TieneRentabilidad,
                    EsFabricado = false
                };

                _articuloPrecioServicio.AddOrUpdate(articuloPrecio,user);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Articulo {entity.GetPropValue()} - User: {user}");
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
        
        public ResultDTO Delete(ArticuloDeleteDTO deleteDTO, string user)
        {
            try
            {
                var entidad = _unitOfWork.ArticuloRepository
                    .GetById(deleteDTO.Id);

                if (entidad == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos del Articulo. Id: {deleteDTO.Id}", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrio un error al obtener los datos",
                        State = false
                    };
                }

                _unitOfWork.ArticuloRepository.Delete(entidad.Id, user);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Articulo - User: {user}", entidad);
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

        public ResultDTO Update(ArticuloPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validacion de Datos del Articulo: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var entityActual = _unitOfWork.ArticuloRepository.GetById(entidad.Id);

                if (entityActual == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information("Ocurrio un error al obtener los datos del Articulo", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrio un error al obtener los datos del Articulo",
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

                var entity = _mapper.Map<Articulo>(entidad);

                entity.User = user;
                entity.Id = entidad.Id;
                entity.VarianteValorUnoId = entityActual.VarianteValorUnoId;
                entity.VarianteValorDosId = entityActual.VarianteValorDosId;
                entity.EmpresaId = entidad.SeleccionoEmpresa ? entidad.EmpresaId : null;
                entity.PrecioCosto = entityActual.PrecioCosto;
                entity.FechaVigenciaKit = entityActual.FechaVigenciaKit;

                _unitOfWork.ArticuloRepository.Update(entity);

                // Elimino lo de la Base de Datos
                foreach (var articuloProveedor in entidad.ArticuloProveedores.Where(x => x.EstaBD && x.Eliminar).ToList())
                {
                    _unitOfWork.ArticuloProveedorRepository
                        .DeleteFisico(articuloProveedor.Id.Value, user);
                }

                foreach (var articuloProveedor in entidad.ArticuloProveedores.Where(x => x.EstaBD && !x.Eliminar).ToList())
                {
                    var _artProv = _unitOfWork.ArticuloProveedorRepository
                        .GetById(articuloProveedor.Id.Value);

                    _artProv.CodigoProveedor = articuloProveedor.CodigoProveedor;
                    _artProv.User = user;

                    _unitOfWork.ArticuloProveedorRepository.Update(_artProv);
                }

                foreach (var articuloProveedor in entidad.ArticuloProveedores.Where(x => !x.EstaBD).ToList())
                {
                    var _articuloProveedorNuevo = new ArticuloProveedor
                    {
                        ArticuloId = entity.Id,
                        ProveedorId = articuloProveedor.ProveedorId.Value,
                        CodigoProveedor = articuloProveedor.CodigoProveedor,
                        EstaEliminado = false,
                        User = user,
                    };

                    _unitOfWork.ArticuloProveedorRepository.Add(_articuloProveedorNuevo);
                }

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Update Articulo {entity.GetPropValue()} - User: {user}");
                }

                return new ResultDTO
                {
                    State = true,
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

        public ResultDTO GetAll(Guid empresaId)
        {
            try
            {
                Expression<Func<Articulo, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == empresaId);

                var entities = _unitOfWork.ArticuloRepository.GetByFilter(filtro,
                                           x => x.OrderBy(d => d.Descripcion),
                                           i => i.Include(x => x.CondicionIva)
                                                                          .Include(x => x.Marca)
                                                                          .Include(x => x.Familia)
                                                                          .Include(x => x.UnidadMedidaVenta)
                                                                          .Include(x => x.UnidadMedidaCompra)
                                                                          .Include(x => x.VarianteValorUno)
                                                                          .Include(x => x.VarianteValorDos)
                                                                          .Include(x => x.ArticuloBajas)
                                                                          .Include(x => x.ArticuloPrecios).ThenInclude(x => x.ListaPrecio)
                                                                          .Include(x => x.ArticuloFormulas).ThenInclude(x => x.ArticuloSecundario)
                                                                          .Include(x => x.ArticuloPadreKits).ThenInclude(x => x.ArticuloHijo)
                                                                          .Include(x => x.ArticuloDepositos).ThenInclude(x => x.Deposito).ThenInclude(x => x.Empresa)
                                                                          .Include(x => x.ArticuloProveedores).ThenInclude(x => x.Proveedor)
                                                                          .Include(x => x.ArticuloHijoOpcionales));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ArticuloDTO>>(entities)
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

        public ResultDTO GetAll()
        {
            try
            {
                var entities = _unitOfWork.ArticuloRepository.GetAll(i => i.Include(x => x.CondicionIva)
                                                                          .Include(x => x.Marca)
                                                                          .Include(x => x.Familia)
                                                                          .Include(x => x.UnidadMedidaVenta)
                                                                          .Include(x => x.UnidadMedidaCompra)
                                                                          .Include(x => x.VarianteValorUno)
                                                                          .Include(x => x.VarianteValorDos)
                                                                          .Include(x => x.ArticuloBajas)
                                                                          .Include(x => x.ArticuloPrecios).ThenInclude(x => x.ListaPrecio)
                                                                          .Include(x => x.ArticuloFormulas).ThenInclude(x => x.ArticuloSecundario)
                                                                          .Include(x => x.ArticuloPadreKits).ThenInclude(x => x.ArticuloHijo)
                                                                          .Include(x => x.ArticuloDepositos).ThenInclude(x => x.Deposito).ThenInclude(x => x.Empresa)
                                                                          .Include(x => x.ArticuloProveedores).ThenInclude(x => x.Proveedor)
                                                                          .Include(x => x.ArticuloHijoOpcionales));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ArticuloDTO>>(entities)
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

        public ResultDTO GetByFilter(ArticuloFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<Articulo, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == filter.EmpresaId || !x.EmpresaId.HasValue);


                if (filter.CadenaBuscar.IndexOf(SeparacionFiltroBusqueda.CaracterSeparador) != -1)
                {
                    var primeraPasada = true;

                    var listaCadenas = filter.CadenaBuscar.Split(SeparacionFiltroBusqueda.CaracterSeparador, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cadena in listaCadenas)
                    {
                        if (primeraPasada)
                        {
                            filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                                     && (x.Descripcion.ToLower().Contains(cadena.ToLower())
                                                         || x.Codigo == filter.CadenaBuscar
                                                         || x.CodigoBarra == filter.CadenaBuscar
                                                         || x.Abreviatura == filter.CadenaBuscar));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                                       && (x.Descripcion.ToLower().Contains(cadena.ToLower())
                                                        || x.Codigo == filter.CadenaBuscar
                                                        || x.CodigoBarra == filter.CadenaBuscar
                                                        || x.Abreviatura == filter.CadenaBuscar));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                                && (x.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                                 || x.Codigo == filter.CadenaBuscar
                                                 || x.CodigoBarra == filter.CadenaBuscar
                                                 || x.Abreviatura == filter.CadenaBuscar));
                }

                var entities = _unitOfWork.ArticuloRepository.GetByFilterTake(filtro,
                                                                          x => x.OrderBy(d => d.Descripcion),
                                                                          i => i.Include(x => x.CondicionIva)
                                                                                .Include(x => x.Marca)
                                                                                .Include(x => x.Familia)
                                                                                .Include(x => x.UnidadMedidaVenta)
                                                                                .Include(x => x.UnidadMedidaCompra)
                                                                                .Include(x => x.VarianteValorUno)
                                                                                .Include(x => x.VarianteValorDos)
                                                                                .Include(x => x.ArticuloBajas)
                                                                                .Include(x => x.ArticuloPrecios).ThenInclude(x => x.ListaPrecio)
                                                                                .Include(x => x.ArticuloFormulas).ThenInclude(x => x.ArticuloSecundario)
                                                                                .Include(x => x.ArticuloPadreKits).ThenInclude(x => x.ArticuloHijo)
                                                                                .Include(x => x.ArticuloDepositos).ThenInclude(x => x.Deposito).ThenInclude(x => x.Empresa)
                                                                                .Include(x => x.ArticuloProveedores).ThenInclude(x => x.Proveedor)
                                                                                .Include(x => x.ArticuloHijoOpcionales),
                                                                          false);

                var result = _mapper.Map<IEnumerable<ArticuloDTO>>(entities);

                var _configCore = _unitOfWork.ConfiguracionCoreRepository
                                    .GetByFilter(x => x.EmpresaId == filter.EmpresaId);

                if (_configCore != null)
                {
                    Parallel.ForEach(result.ToList(), x =>
                    {
                        x.PrecioPublico = x.ListaPrecios.Any(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                                            && lp.FechaActualizacion == x.ListaPrecios
                                                                                                   .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                                                   .Max(f => f.FechaActualizacion))
                                                        ? x.ListaPrecios.First(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                                     && lp.FechaActualizacion == x.ListaPrecios
                                                                                    .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                                    .Max(f => f.FechaActualizacion)).Monto
                                                        : 0;

                        x.Stock = x.Cantidades.Any(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId)
                                  ? x.Cantidades.First(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId).Cantidad
                                  : 0;
                    });
                }

                return new ResultDTO
                {
                    State = true,
                    Data = result.ToList(),
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

        public ResultDTO GetByFilterLookUp(ArticuloFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<Articulo, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == filter.EmpresaId || !x.EmpresaId.HasValue);

                filtro = filtro.And(x => x.TipoArticulo != TipoArticulo.Base
                                         && x.TipoArticulo != TipoArticulo.BienUso
                                         && x.TipoArticulo != TipoArticulo.Kit);

                if (filter.CadenaBuscar.IndexOf(SeparacionFiltroBusqueda.CaracterSeparador) != -1)
                {
                    var primeraPasada = true;

                    var listaCadenas = filter.CadenaBuscar.Split(SeparacionFiltroBusqueda.CaracterSeparador, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cadena in listaCadenas)
                    {
                        if (primeraPasada)
                        {
                            filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                                     && (x.Descripcion.ToLower().Contains(cadena.ToLower())
                                                         || x.Codigo == filter.CadenaBuscar
                                                         || x.CodigoBarra == filter.CadenaBuscar
                                                         || x.Abreviatura == filter.CadenaBuscar));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                                       && (x.Descripcion.ToLower().Contains(cadena.ToLower())
                                                        || x.Codigo == filter.CadenaBuscar
                                                        || x.CodigoBarra == filter.CadenaBuscar
                                                        || x.Abreviatura == filter.CadenaBuscar));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                                && (x.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                                 || x.Codigo == filter.CadenaBuscar
                                                 || x.CodigoBarra == filter.CadenaBuscar
                                                 || x.Abreviatura == filter.CadenaBuscar));
                }

                var entities = _unitOfWork.ArticuloRepository.GetByFilterTake(filtro,
                                                                          x => x.OrderBy(d => d.Descripcion),
                                                                          i => i.Include(x => x.CondicionIva)
                                                                                .Include(x => x.Marca)
                                                                                .Include(x => x.Familia)
                                                                                .Include(x => x.UnidadMedidaVenta)
                                                                                .Include(x => x.UnidadMedidaCompra)
                                                                                .Include(x => x.VarianteValorUno)
                                                                                .Include(x => x.VarianteValorDos)
                                                                                .Include(x => x.ArticuloBajas)
                                                                                .Include(x => x.ArticuloPrecios).ThenInclude(x => x.ListaPrecio)
                                                                                .Include(x => x.ArticuloFormulas).ThenInclude(x => x.ArticuloSecundario)
                                                                                .Include(x => x.ArticuloPadreKits).ThenInclude(x => x.ArticuloHijo)
                                                                                .Include(x => x.ArticuloDepositos).ThenInclude(x => x.Deposito).ThenInclude(x => x.Empresa)
                                                                                .Include(x => x.ArticuloProveedores).ThenInclude(x => x.Proveedor)
                                                                                .Include(x => x.ArticuloHijoOpcionales),
                                                                          false);

                var result = _mapper.Map<IEnumerable<ArticuloDTO>>(entities);

                var _configCore = _unitOfWork.ConfiguracionCoreRepository
                                    .GetByFilter(x => x.EmpresaId == filter.EmpresaId);

                if (_configCore != null)
                {
                    Parallel.ForEach(result.ToList(), x =>
                    {
                        x.PrecioPublico = x.ListaPrecios.Any(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                                            && lp.FechaActualizacion == x.ListaPrecios
                                                                                                   .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                                                   .Max(f => f.FechaActualizacion))
                                                        ? x.ListaPrecios.First(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                                     && lp.FechaActualizacion == x.ListaPrecios
                                                                                    .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                                    .Max(f => f.FechaActualizacion)).Monto
                                                        : 0;

                        x.Stock = x.Cantidades.Any(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId)
                                  ? x.Cantidades.First(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId).Cantidad
                                  : 0;
                    });
                }

                return new ResultDTO
                {
                    State = true,
                    Data = result.ToList(),
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

        public ResultDTO GetByFilterLookUp(ArticuloFilterDTO filter, List<TipoArticulo> tipos)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<Articulo, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == filter.EmpresaId || !x.EmpresaId.HasValue);

                filtro = filtro.And(x => tipos.Contains(x.TipoArticulo));

                if (filter.CadenaBuscar.IndexOf(SeparacionFiltroBusqueda.CaracterSeparador) != -1)
                {
                    var primeraPasada = true;

                    var listaCadenas = filter.CadenaBuscar.Split(SeparacionFiltroBusqueda.CaracterSeparador, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cadena in listaCadenas)
                    {
                        if (primeraPasada)
                        {
                            filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                                     && (x.Descripcion.ToLower().Contains(cadena.ToLower())
                                                         || x.Codigo == filter.CadenaBuscar
                                                         || x.CodigoBarra == filter.CadenaBuscar
                                                         || x.Abreviatura == filter.CadenaBuscar));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                                       && (x.Descripcion.ToLower().Contains(cadena.ToLower())
                                                        || x.Codigo == filter.CadenaBuscar
                                                        || x.CodigoBarra == filter.CadenaBuscar
                                                        || x.Abreviatura == filter.CadenaBuscar));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                                && (x.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                                 || x.Codigo == filter.CadenaBuscar
                                                 || x.CodigoBarra == filter.CadenaBuscar
                                                 || x.Abreviatura == filter.CadenaBuscar));
                }

                var entities = _unitOfWork.ArticuloRepository.GetByFilterTake(filtro,
                                                                          x => x.OrderBy(d => d.Descripcion),
                                                                          i => i.Include(x => x.CondicionIva)                                                                                
                                                                                .Include(x => x.Marca)
                                                                                .Include(x => x.Familia)
                                                                                .Include(x => x.UnidadMedidaVenta)
                                                                                .Include(x => x.UnidadMedidaCompra)
                                                                                .Include(x => x.VarianteValorUno)
                                                                                .Include(x => x.VarianteValorDos)
                                                                                .Include(x => x.ArticuloBajas)
                                                                                .Include(x => x.ArticuloPrecios).ThenInclude(x => x.ListaPrecio)
                                                                                .Include(x => x.ArticuloFormulas).ThenInclude(x => x.ArticuloSecundario)
                                                                                .Include(x => x.ArticuloPadreKits).ThenInclude(x => x.ArticuloHijo)
                                                                                .Include(x => x.ArticuloDepositos).ThenInclude(x => x.Deposito).ThenInclude(x => x.Empresa)
                                                                                .Include(x => x.ArticuloProveedores).ThenInclude(x => x.Proveedor)
                                                                                .Include(x => x.ArticuloHijoOpcionales),
                                                                          false);

                var result = _mapper.Map<IEnumerable<ArticuloDTO>>(entities);

                var _configCore = _unitOfWork.ConfiguracionCoreRepository
                                    .GetByFilter(x => x.EmpresaId == filter.EmpresaId);

                if (_configCore != null)
                {
                    Parallel.ForEach(result.ToList(), x =>
                    {
                        x.PrecioPublico = x.ListaPrecios.Any(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                                            && lp.FechaActualizacion == x.ListaPrecios
                                                                                                   .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                                                   .Max(f => f.FechaActualizacion))
                                                        ? x.ListaPrecios.First(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                                     && lp.FechaActualizacion == x.ListaPrecios
                                                                                    .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                                    .Max(f => f.FechaActualizacion)).Monto
                                                        : 0;

                        x.Stock = x.Cantidades.Any(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId)
                                  ? x.Cantidades.First(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId).Cantidad
                                  : 0;
                    });
                }

                return new ResultDTO
                {
                    State = true,
                    Data = result.ToList(),
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

        public ResultDTO GetByIdPivot(Guid empresaId, Guid articuloId)
        {
            try
            {
                var result = _unitOfWork.ArticuloDepositoRepository
                    .GetByFilter(x => x.Deposito.EmpresaId == empresaId && x.ArticuloId == articuloId
                                      , null, i => i.Include(z => z.Deposito).Include(z => z.Articulo));

                var resultPivot = result.Pivot(rowSelector: d => d.Deposito.Descripcion,
                    columnSelector: d => d.Articulo.Descripcion,
                    valueSelector: d => d.Sum(s => s.Cantidad));



                return new ResultDTO { State = true };
            }
            catch (Exception ex)
            {
                return new ResultDTO
                {
                    State = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public ResultDTO GetById(Guid id, Guid empresaId)
        {
            try
            {
                var entity = _unitOfWork.ArticuloRepository.GetById(id,
                                                                    i => i.Include(x => x.CondicionIva)
                                                                          .Include(x => x.Marca)
                                                                          .Include(x => x.Familia)
                                                                          .Include(x => x.UnidadMedidaVenta)
                                                                          .Include(x => x.UnidadMedidaCompra)
                                                                          .Include(x => x.VarianteValorUno)
                                                                          .Include(x => x.VarianteValorDos)
                                                                          .Include(x => x.ArticuloBajas)
                                                                          .Include(x => x.ArticuloPrecios).ThenInclude(x => x.ListaPrecio)
                                                                          .Include(x => x.ArticuloFormulas).ThenInclude(x => x.ArticuloSecundario)
                                                                          .Include(x => x.ArticuloPadreKits).ThenInclude(x => x.ArticuloHijo)
                                                                          .Include(x => x.ArticuloDepositos).ThenInclude(x => x.Deposito).ThenInclude(x => x.Empresa)
                                                                          .Include(x => x.ArticuloProveedores).ThenInclude(x => x.Proveedor)
                                                                          .Include(x => x.ArticuloHijoOpcionales),
                                                           false);

                if (entity == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos del Articulo. Id: {id}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = "No se encontro el dato solicitado"
                    };
                }

                var result = _mapper.Map<ArticuloDTO>(entity);

                var _configCore = _unitOfWork.ConfiguracionCoreRepository
                                             .GetByFilter(x => x.EmpresaId == empresaId);

                if (_configCore != null)
                {
                    result.PrecioPublico = result.ListaPrecios.Any(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                      && lp.FechaActualizacion == result.ListaPrecios
                                                                           .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                           .Max(f => f.FechaActualizacion))
                                                    ? result.ListaPrecios.First(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                    && lp.FechaActualizacion == result.ListaPrecios
                                                                         .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                         .Max(f => f.FechaActualizacion)).Monto
                                                    : 0;

                    result.Stock = result.Cantidades.Any(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId)
                              ? result.Cantidades.First(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId).Cantidad
                              : 0;
                }

                return new ResultDTO
                {
                    State = true,
                    Data = result
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

        public ResultDTO GetById(Guid id, Guid empresaId, Guid depositoId)
        {
            try
            {
                var entity = _unitOfWork.ArticuloRepository.GetById(id,
                                                     i => i.Include(x => x.CondicionIva)
                                                           .Include(x => x.Marca)
                                                           .Include(x => x.Familia)
                                                           .Include(x => x.UnidadMedidaVenta)
                                                           .Include(x => x.UnidadMedidaCompra)
                                                           .Include(x => x.VarianteValorUno)
                                                           .Include(x => x.VarianteValorDos)
                                                           .Include(x => x.ArticuloBajas)
                                                           .Include(x => x.ArticuloPrecios).ThenInclude(x => x.ListaPrecio)
                                                           .Include(x => x.ArticuloFormulas).ThenInclude(x => x.ArticuloSecundario)
                                                           .Include(x => x.ArticuloPadreKits).ThenInclude(x => x.ArticuloHijo)
                                                           .Include(x => x.ArticuloDepositos).ThenInclude(x => x.Deposito).ThenInclude(x => x.Empresa)
                                                           .Include(x => x.ArticuloProveedores).ThenInclude(x => x.Proveedor)
                                                           .Include(x => x.ArticuloHijoOpcionales),
                                                           false);

                if (entity == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos del Articulo. Id: {id}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = "No se encontro el dato solicitado"
                    };
                }

                var result = _mapper.Map<ArticuloDTO>(entity);

                var _configCore = _unitOfWork.ConfiguracionCoreRepository
                                             .GetByFilter(x => x.EmpresaId == empresaId);

                if (_configCore != null)
                {
                    result.PrecioPublico = result.ListaPrecios.Any(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                      && lp.FechaActualizacion == result.ListaPrecios
                                                                           .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                           .Max(f => f.FechaActualizacion))
                                                    ? result.ListaPrecios.First(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                    && lp.FechaActualizacion == result.ListaPrecios
                                                                         .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                         .Max(f => f.FechaActualizacion)).Monto
                                                    : 0;

                    result.Stock = result.Cantidades.Any(d => d.DepositoId == depositoId)
                              ? result.Cantidades.First(d => d.DepositoId == depositoId).Cantidad
                              : 0;
                }

                return new ResultDTO
                {
                    State = true,
                    Data = result
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

        public ResultDTO GetByIds(List<Guid> ids, Guid empresaId)
        {
            try
            {
                var entities = _unitOfWork.ArticuloRepository.GetByIds(ids,
                                                                    i => i.Include(x => x.CondicionIva)
                                                                          .Include(x => x.Marca)
                                                                          .Include(x => x.Familia)
                                                                          .Include(x => x.UnidadMedidaVenta)
                                                                          .Include(x => x.UnidadMedidaCompra)
                                                                          .Include(x => x.VarianteValorUno)
                                                                          .Include(x => x.VarianteValorDos)
                                                                          .Include(x => x.ArticuloBajas)
                                                                          .Include(x => x.ArticuloPrecios).ThenInclude(x => x.ListaPrecio)
                                                                          .Include(x => x.ArticuloFormulas).ThenInclude(x => x.ArticuloSecundario)
                                                                          .Include(x => x.ArticuloPadreKits).ThenInclude(x => x.ArticuloHijo)
                                                                          .Include(x => x.ArticuloDepositos).ThenInclude(x => x.Deposito).ThenInclude(x => x.Empresa)
                                                                          .Include(x => x.ArticuloProveedores).ThenInclude(x => x.Proveedor)
                                                                          .Include(x => x.ArticuloHijoOpcionales),
                                                                    false);

                var result = _mapper.Map<IEnumerable<ArticuloDTO>>(entities);

                var _configCore = _unitOfWork.ConfiguracionCoreRepository
                                             .GetByFilter(x => x.EmpresaId == empresaId);

                if (_configCore != null)
                {
                    Parallel.ForEach(result.ToList(), x =>
                    {
                        x.PrecioPublico = x.ListaPrecios.Any(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                                            && lp.FechaActualizacion == x.ListaPrecios
                                                                                                   .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                                                   .Max(f => f.FechaActualizacion))
                                                        ? x.ListaPrecios.First(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                                     && lp.FechaActualizacion == x.ListaPrecios
                                                                                    .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                                    .Max(f => f.FechaActualizacion)).Monto
                                                        : 0;

                        x.Stock = x.Cantidades.Any(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId)
                                  ? x.Cantidades.First(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId).Cantidad
                                  : 0;
                    });
                }

                return new ResultDTO
                {
                    State = true,
                    Data = result.ToList()
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

        public ResultDTO GetByIds(List<Guid> ids, Guid empresaId, Guid depositoId)
        {
            try
            {
                var entities = _unitOfWork.ArticuloRepository.GetByIds(ids,
                                                     i => i.Include(x => x.CondicionIva)
                                                                          .Include(x => x.Marca)
                                                                          .Include(x => x.Familia)
                                                                          .Include(x => x.UnidadMedidaVenta)
                                                                          .Include(x => x.UnidadMedidaCompra)
                                                                          .Include(x => x.VarianteValorUno)
                                                                          .Include(x => x.VarianteValorDos)
                                                                          .Include(x => x.ArticuloBajas)
                                                                          .Include(x => x.ArticuloPrecios).ThenInclude(x => x.ListaPrecio)
                                                                          .Include(x => x.ArticuloFormulas).ThenInclude(x => x.ArticuloSecundario)
                                                                          .Include(x => x.ArticuloPadreKits).ThenInclude(x => x.ArticuloHijo)
                                                                          .Include(x => x.ArticuloDepositos).ThenInclude(x => x.Deposito).ThenInclude(x => x.Empresa)
                                                                          .Include(x => x.ArticuloProveedores).ThenInclude(x => x.Proveedor)
                                                                          .Include(x => x.ArticuloHijoOpcionales),
                                                           false);

                var result = _mapper.Map<IEnumerable<ArticuloDTO>>(entities);

                var _configCore = _unitOfWork.ConfiguracionCoreRepository
                                             .GetByFilter(x => x.EmpresaId == empresaId);

                if (_configCore != null)
                {
                    Parallel.ForEach(result.ToList(), x =>
                    {
                        x.PrecioPublico = x.ListaPrecios.Any(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                                            && lp.FechaActualizacion == x.ListaPrecios
                                                                                                   .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                                                   .Max(f => f.FechaActualizacion))
                                                        ? x.ListaPrecios.First(lp => lp.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId
                                                                                     && lp.FechaActualizacion == x.ListaPrecios
                                                                                    .Where(lp2 => lp2.ListaPrecioId == _configCore.First().ListaPrecioPorDefectoParaVentaId)
                                                                                    .Max(f => f.FechaActualizacion)).Monto
                                                        : 0;

                        x.Stock = x.Cantidades.Any(d => d.DepositoId == depositoId)
                                  ? x.Cantidades.First(d => d.DepositoId == depositoId).Cantidad
                                  : 0;
                    });
                }

                return new ResultDTO
                {
                    State = true,
                    Data = result.ToList()
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

        private bool VerificarSiExiste(string descripcion, Guid? empresaId, Guid? id = null)
        {
            var entidads = _unitOfWork.FamiliaRepository.GetAll();

            if (entidads != null && entidads.Any())
            {
                if (id == null)
                {
                    if (empresaId.HasValue)
                    {
                        return entidads.Any(x => x.EmpresaId == empresaId.Value && x.Descripcion.ToLower() == descripcion.ToLower());
                    }
                    else
                    {
                        return entidads.Any(x => x.Descripcion.ToLower() == descripcion.ToLower());
                    }
                }
                else
                {
                    if (empresaId.HasValue)
                    {
                        return entidads.Any(x => x.Id != id.Value
                            && x.EmpresaId == empresaId
                            && x.Descripcion.ToLower() == descripcion.ToLower());
                    }
                    else
                    {
                        return entidads.Any(x => x.Id != id.Value
                            && x.Descripcion.ToLower() == descripcion.ToLower());
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public ResultDTO GetByCodigo(Guid listaPrecioId, string cadenaBuscar, Guid empresaId)
        {
            try
            {
                cadenaBuscar = !string.IsNullOrEmpty(cadenaBuscar) ? cadenaBuscar : string.Empty;

                Expression<Func<Articulo, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == empresaId || !x.EmpresaId.HasValue);

                filtro = filtro.And(x => x.TipoArticulo != TipoArticulo.Base && (x.PerfilArticulo == PerfilArticulo.CompraVenta || x.PerfilArticulo == PerfilArticulo.Venta));

                filtro = filtro.And(x => !x.EstaEliminado
                                            && x.Codigo == cadenaBuscar
                                             || x.CodigoBarra == cadenaBuscar
                                             || x.Abreviatura == cadenaBuscar);
                

                var entities = _unitOfWork.ArticuloRepository.GetByFilterTake(filtro,
                                                                          x => x.OrderBy(d => d.Descripcion),
                                                                          i => i.Include(x => x.CondicionIva)
                                                                          .Include(x => x.Marca)
                                                                          .Include(x => x.Familia)
                                                                          .Include(x => x.UnidadMedidaVenta)
                                                                          .Include(x => x.UnidadMedidaCompra)
                                                                          .Include(x => x.VarianteValorUno)
                                                                          .Include(x => x.VarianteValorDos)
                                                                          .Include(x => x.ArticuloBajas)
                                                                          .Include(x => x.ArticuloPrecios).ThenInclude(x => x.ListaPrecio)
                                                                          .Include(x => x.ArticuloFormulas).ThenInclude(x => x.ArticuloSecundario)
                                                                          .Include(x => x.ArticuloPadreKits).ThenInclude(x => x.ArticuloHijo)
                                                                          .Include(x => x.ArticuloDepositos).ThenInclude(x => x.Deposito).ThenInclude(x => x.Empresa)
                                                                          .Include(x => x.ArticuloProveedores).ThenInclude(x => x.Proveedor)
                                                                          .Include(x => x.ArticuloHijoOpcionales),
                                                                          false);

                var result = _mapper.Map<ArticuloDTO>(entities.FirstOrDefault());

                if (result == null)
                {
                    return new ResultDTO
                    {
                        State = false,
                        Data = null
                    };
                }

                var _configCore = _unitOfWork.ConfiguracionCoreRepository
                                    .GetByFilter(x => x.EmpresaId == empresaId);

                if (_configCore != null)
                {
                    result.PrecioPublico = result.ListaPrecios.Any(lp => lp.ListaPrecioId == listaPrecioId
                                                                                        && lp.FechaActualizacion == result.ListaPrecios
                                                                                               .Where(lp2 => lp2.ListaPrecioId == listaPrecioId)
                                                                                               .Max(f => f.FechaActualizacion))
                                                    ? result.ListaPrecios.First(lp => lp.ListaPrecioId == listaPrecioId
                                                                                 && lp.FechaActualizacion == result.ListaPrecios
                                                                                .Where(lp2 => lp2.ListaPrecioId == listaPrecioId)
                                                                                .Max(f => f.FechaActualizacion)).Monto
                                                    : 0;

                    result.Stock = result.Cantidades.Any(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId)
                              ? result.Cantidades.First(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId).Cantidad
                              : 0;
                }

                return new ResultDTO
                {
                    State = true,
                    Data = result,
                };
            }
            catch (Exception)
            {
                return new ResultDTO
                {
                    State = false,
                    Message = "Ocurrio un error al obtener el articulo"
                };
            }
        }

        public ResultDTO GetByCodigoProveedor(Guid? proveedorId, string cadenaBuscar, Guid empresaId)
        {
            try
            {
                cadenaBuscar = !string.IsNullOrEmpty(cadenaBuscar) ? cadenaBuscar : string.Empty;

                Expression<Func<Articulo, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == empresaId || !x.EmpresaId.HasValue);

                if (proveedorId.HasValue && proveedorId.Value != Guid.Empty)
                {
                    filtro = filtro.And(x => x.ArticuloProveedores.Any(p => p.ProveedorId == proveedorId.Value));
                }

                filtro = filtro.And(x => x.PerfilArticulo == PerfilArticulo.CompraVenta || x.PerfilArticulo == PerfilArticulo.Compra);

                filtro = filtro.And(x => x.TipoArticulo == TipoArticulo.Simple);

                if (proveedorId.HasValue && proveedorId.Value != Guid.Empty)
                {
                    filtro = filtro.And(x => !x.EstaEliminado && x.Codigo == cadenaBuscar
                                          || x.CodigoBarra == cadenaBuscar
                                          || x.Abreviatura == cadenaBuscar
                                          || x.ArticuloProveedores.Any(p => p.CodigoProveedor == cadenaBuscar));
                }
                else
                {
                    filtro = filtro.And(x => !x.EstaEliminado && x.Codigo == cadenaBuscar
                                          || x.CodigoBarra == cadenaBuscar
                                          || x.Abreviatura == cadenaBuscar);
                }

                var entities = _unitOfWork.ArticuloRepository.GetByFilterTake(filtro,
                                                                          x => x.OrderBy(d => d.Descripcion),
                                                                          i => i.Include(x => x.CondicionIva)
                                                                                .Include(x => x.ArticuloPrecios).ThenInclude(x => x.ListaPrecio)
                                                                                .Include(x => x.Marca)
                                                                                .Include(x => x.Familia)
                                                                                .Include(x => x.UnidadMedidaVenta)
                                                                                .Include(x => x.UnidadMedidaCompra)
                                                                                .Include(x => x.VarianteValorUno)
                                                                                .Include(x => x.VarianteValorDos)
                                                                                .Include(x => x.ArticuloBajas)
                                                                                .Include(x => x.ArticuloFormulas).ThenInclude(x => x.ArticuloSecundario)
                                                                                .Include(x => x.ArticuloPadreKits).ThenInclude(x => x.ArticuloHijo)
                                                                                .Include(x => x.ArticuloDepositos).ThenInclude(x => x.Deposito)
                                                                                .Include(x => x.ArticuloProveedores).ThenInclude(x => x.Proveedor),
                                                                          false);

                var result = _mapper.Map<ArticuloDTO>(entities.FirstOrDefault());

                if (result == null)
                {
                    return new ResultDTO
                    {
                        State = false,
                        Data = null
                    };
                }

                var _configCore = _unitOfWork.ConfiguracionCoreRepository.GetByFilter(x => x.EmpresaId == empresaId);

                if (_configCore != null)
                {
                    result.Stock = result.Cantidades.Any(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId)
                                   ? result.Cantidades.First(d => d.DepositoId == _configCore.First().DepositoPorDefectoParaVentaId).Cantidad
                                   : 0;
                }

                return new ResultDTO
                {
                    State = true,
                    Data = result,
                };
            }
            catch (Exception)
            {
                return new ResultDTO
                {
                    State = false,
                    Message = "Ocurrio un error al obtener el articulo"
                };
            }
        }
    }
}

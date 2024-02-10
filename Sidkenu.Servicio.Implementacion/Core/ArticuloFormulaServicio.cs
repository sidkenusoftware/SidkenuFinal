using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ArticuloFormula;
using Sidkenu.Servicio.DTOs.Core.ConfiguracionCore;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class ArticuloFormulaServicio : ServicioBase, IArticuloFormulaServicio
    {
        private readonly IConfiguracionCoreServicio _configuracionCoreServicio;

        public ArticuloFormulaServicio(IUnidadDeTrabajo unitOfWork,
                                   IMapper mapper,
                                   ILogger logger,
                                   IConfiguracionCoreServicio configuracionCoreServicio)
                                   : base(unitOfWork, mapper, logger)
        {
            _configuracionCoreServicio = configuracionCoreServicio;
        }        

        public ResultDTO Add(ArticuloFormulaPersistenciaDTO articuloFormula, string user)
        {
            try
            {
                var _configCoreResult = _configuracionCoreServicio
                    .Get(articuloFormula.EmpresaId);

                if (_configCoreResult == null || !_configCoreResult.State)
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "Ocurrió un error al obtener la configuracion del Inventario"
                    };
                }

                var _configCore = (ConfiguracionCoreDTO)_configCoreResult.Data;

                var _articulo = _unitOfWork.ArticuloRepository
                    .GetById(articuloFormula.ArticuloBaseId, x=> x.Include(i=>i.ArticuloDepositos)
                                                              .Include(i => i.ArticuloPrecios));

                if(_articulo == null ) 
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "Ocurrió un error al obtener el Articulo BASE"
                    };
                }

                var _artFormulas = _unitOfWork.ArticuloFormulaRepository.GetByFilter(x => x.ArticuloId == _articulo.Id);

                foreach (var artFormula in articuloFormula.Articulos.ToList())
                {
                    if (artFormula.ExisteBase)
                    {
                        var art = _artFormulas.FirstOrDefault(x => x.ArticuloSecundarioId == artFormula.ArticuloHijoId);

                        if (art == null)
                            return new ResultDTO
                            {
                                State = false,
                                Message = "Ocurrió un error al obtener el Articulo Formula"
                            };

                        if (artFormula.EstaEliminado)
                        {
                            _unitOfWork.ArticuloFormulaRepository.DeleteFisico(art.Id, user);
                        }
                        else
                        {
                            art.Cantidad = artFormula.Cantidad;

                            _unitOfWork.ArticuloFormulaRepository.Update(art);
                        }
                    }
                    else
                    {
                        var artFormulaNuevo = new ArticuloFormula
                        {
                            ArticuloSecundarioId = artFormula.Id,
                            ArticuloId = _articulo.Id,
                            Cantidad = artFormula.Cantidad,
                            EstaEliminado = false,
                            User = user,                            
                        };

                        _unitOfWork.ArticuloFormulaRepository.Add(artFormulaNuevo);
                    }
                }
                
                _unitOfWork.ArticuloRepository.Update(_articulo);

                _unitOfWork.Commit();                

                return new ResultDTO
                {
                    State = true,
                    Message = "La Formula se grabo correctamente"
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

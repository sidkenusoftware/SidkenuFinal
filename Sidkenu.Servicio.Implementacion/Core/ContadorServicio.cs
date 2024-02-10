using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.Interface.Core;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class ContadorServicio : IContadorServicio
    {
        private readonly IUnidadDeTrabajo _unitOfWork;

        public ContadorServicio(IUnidadDeTrabajo unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public long ObtenerNumero(TipoContador tipoContador, Guid empresaId, string user)
        {
			try
			{
                long _numero = 0;

                var result = _unitOfWork.ContadorRepository
                                        .GetByFilter(x => x.TipoContador == tipoContador
                                                          && x.EmpresaId == empresaId);

                if (result != null && result.Any())
                {
                    var _contador = result.FirstOrDefault();

                    if (tipoContador == TipoContador.ArticuloTemporal)
                    {
                        _contador.Numero--;
                    }
                    else
                    {
                        _contador.Numero++;
                    }
                    _contador.User = user;

                    _unitOfWork.ContadorRepository.Update(_contador);

                    _numero = _contador.Numero;
                }
                else
                {
                    _unitOfWork.ContadorRepository.Add(new Dominio.Entidades.Core.Contador
                    {
                        EmpresaId = empresaId,
                        EstaEliminado = false,
                        Numero = tipoContador == TipoContador.ArticuloTemporal ? 999999 : 1,
                        TipoContador = tipoContador,
                        User = user
                    });

                    _numero = tipoContador == TipoContador.ArticuloTemporal ? 999999 : 1;
                }

                _unitOfWork.Commit();

                return _numero;
			}
			catch (Exception ex)
			{
                throw new Exception($"Ocurrio un error al obtener el contador. {ex.Message}");
            }
        }
    }
}

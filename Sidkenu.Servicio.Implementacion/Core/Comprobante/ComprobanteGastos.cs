using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Comprobante;

namespace Sidkenu.Servicio.Implementacion.Core.Comprobante
{
    public class ComprobanteGastos : Comprobante
    {
        public override ResultDTO Add(ComprobanteDTO comprobante, string userLogin)
        {
            try
            {
                var _comprobanteGastoDto = (ComprobanteGastoDTO)comprobante;

                var comprobanteResult = base.Add(comprobante, userLogin);

                if (comprobanteResult != null && comprobanteResult.State)
                {
                    var nuevoComprobante = (ComprobanteGasto)comprobanteResult.Data;

                    nuevoComprobante.TipoGastoId = _comprobanteGastoDto.TipoGastoId;
                    nuevoComprobante.Descripcion = _comprobanteGastoDto.Descripcion;

                    UnitOfWork.ComprobanteGastoRepository.Add(nuevoComprobante);

                    UnitOfWork.Commit();

                    comprobante.Id = nuevoComprobante.Id;

                    return new ResultDTO
                    {
                        State = true,
                        Message = "Los datos se grabaron correctamente",
                        Data = comprobante
                    };
                }
                else
                {
                    return comprobanteResult;
                }
            }
            catch (Exception ex)
            {
                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }
    }
}

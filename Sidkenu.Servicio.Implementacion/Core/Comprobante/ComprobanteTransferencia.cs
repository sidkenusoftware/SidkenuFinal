using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Comprobante;

namespace Sidkenu.Servicio.Implementacion.Core.Comprobante
{
    public class ComprobanteTransferencia : Comprobante
    {
        public override ResultDTO Add(ComprobanteDTO comprobante, string userLogin)
        {
            try
            {
                var _comprobanteTransferenciaDto = (ComprobanteTransferenciaDTO)comprobante;

                var comprobanteResult = base.Add(comprobante, userLogin);

                if (comprobanteResult != null && comprobanteResult.State)
                {
                    var nuevoComprobante = (Dominio.Entidades.Core.ComprobanteTransferencia)comprobanteResult.Data;

                    nuevoComprobante.Descripcion = _comprobanteTransferenciaDto.Descripcion;

                    UnitOfWork.ComprobanteTransferenciaRepository.Add(nuevoComprobante);

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

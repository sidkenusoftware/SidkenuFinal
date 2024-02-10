using FluentValidation;
using Sidkenu.Servicio.DTOs.Core.ConfiguracionCore;

namespace Sidkenu.Servicio.Validator.Core
{
    public class ConfiguracionCoreValidator : AbstractValidator<ConfiguracionCorePersistenciaDTO>
    {
        public ConfiguracionCoreValidator()
        {
            RuleFor(x => x.ActivarStockPorVencimientoLote);

            RuleFor(x => x.ActivarAumentoPrecioPorFamilia);

            RuleFor(x => x.ActivarAumentoPrecioPorFamiliaListaPrecio);

            RuleFor(x => x.ActivarAumentoPrecioPorMarca);

            RuleFor(x => x.ActivarAumentoPrecioPorMarcaListaPrecio);

            RuleFor(x => x.DepositoPorDefectoParaCompraId);

            RuleFor(x => x.DepositoPorDefectoParaVentaId);

            RuleFor(x => x.ListaPrecioPorDefectoParaVentaId);

            RuleFor(x => x.ActualizarPrecioFinalizarLaFabricacion);
        }
    }
}
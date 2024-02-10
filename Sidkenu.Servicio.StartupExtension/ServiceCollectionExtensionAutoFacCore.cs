using Autofac;
using FluentValidation;
using Sidkenu.Servicio.DTOs.Core.Caja;
using Sidkenu.Servicio.DTOs.Core.TipoGasto;
using Sidkenu.Servicio.Implementacion.Core;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Validator.Core;

namespace Sidkenu.Servicio.StartupExtension
{
    public static class ServiceCollectionExtensionAutoFacCore
    {
        public static void ConfigureAutoFacServicios(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<CajaServicio>().As<ICajaServicio>();
            containerBuilder.RegisterType<TipoGastoServicio>().As<ITipoGastoServicio>();

            // Validaciones
            containerBuilder.RegisterType<CajaValidator>().As<IValidator<CajaPersistenciaDTO>>();
            containerBuilder.RegisterType<TipoGastoValidator>().As<IValidator<TipoGastoPersistenciaDTO>>();
        }
    }
}
using Autofac;
using Dime.Servicio.Comun.EMail;
using FluentValidation;
using Sidkenu.Dominio.Repositorio;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Infraestructura;
using Sidkenu.Servicio.DTOs.Seguridad.AsistenteCargaInicial;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Implementacion.Seguridad;
using Sidkenu.Servicio.Interface.Base;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator.Seguridad.Asistente;

namespace Sidkenu.Servicio.StartupExtension
{
    public static class ServiceCollectionExtensionAutoFacComun
    {
        public static void ConfigureAutoFacServicios(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DataContext>().AsSelf().SingleInstance();

            containerBuilder.RegisterType<UnidadDeTrabajo>().As<IUnidadDeTrabajo>().InstancePerDependency();

            containerBuilder.RegisterGeneric(typeof(RepositoryGeneric<>)).As(typeof(IRepositoryGeneric<>))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterType<ServicioBase>().As<IServicioBase>();

            // Servicios            
            containerBuilder.RegisterType<CorreoElectronico>().As<ICorreoElectronico>();
            containerBuilder.RegisterType<ConectividadServicio>().As<IConectividadServicio>();
            containerBuilder.RegisterType<SeguridadServicio>().As<ISeguridadServicio>();
            containerBuilder.RegisterType<AsistenteCargaInicialServicio>().As<IAsistenteCargaInicialServicio>();

            // Validaciones
            containerBuilder.RegisterType<AsistentePersonaValidator>().As<IValidator<PersonaAsistenteDTO>>();
            containerBuilder.RegisterType<AsistenteEmpresaValidator>().As<IValidator<EmpresaAsistenteDTO>>();
            containerBuilder.RegisterType<AsistenteConfiguracionValidator>().As<IValidator<ConfiguracionAsistenteDTO>>();


        }
    }
}
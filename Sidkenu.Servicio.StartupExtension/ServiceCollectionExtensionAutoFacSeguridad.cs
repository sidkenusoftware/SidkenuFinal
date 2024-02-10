using Autofac;
using FluentValidation;
using Sidkenu.Servicio.DTOs.Seguridad.Configuracion;
using Sidkenu.Servicio.DTOs.Seguridad.Empresa;
using Sidkenu.Servicio.DTOs.Seguridad.Grupo;
using Sidkenu.Servicio.DTOs.Seguridad.IngresoBruto;
using Sidkenu.Servicio.DTOs.Seguridad.Localidad;
using Sidkenu.Servicio.DTOs.Seguridad.Modulo;
using Sidkenu.Servicio.DTOs.Seguridad.Persona;
using Sidkenu.Servicio.DTOs.Seguridad.Provincia;
using Sidkenu.Servicio.DTOs.Seguridad.TipoResponsabilidad;
using Sidkenu.Servicio.Implementacion.Seguridad;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator.Core;
using Sidkenu.Servicio.Validator.Seguridad;

namespace Sidkenu.Servicio.StartupExtension
{
    public static class ServiceCollectionExtensionAutoFacSeguridad
    {
        public static void ConfigureAutoFacServicios(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<CuentaServicio>().As<ICuentaServicio>();
            containerBuilder.RegisterType<PasswordServicio>().As<IPasswordServicio>();
            containerBuilder.RegisterType<UsuarioServicio>().As<IUsuarioServicio>();
            containerBuilder.RegisterType<PersonaServicio>().As<IPersonaServicio>();
            containerBuilder.RegisterType<EmpresaPersonaServicio>().As<IEmpresaPersonaServicio>();
            containerBuilder.RegisterType<EmpresaServicio>().As<IEmpresaServicio>();
            containerBuilder.RegisterType<ProvinciaServicio>().As<IProvinciaServicio>();
            containerBuilder.RegisterType<IngresoBrutoServicio>().As<IIngresoBrutoServicio>();
            containerBuilder.RegisterType<TipoResponsabilidadServicio>().As<ITipoResponsabilidadServicio>();
            containerBuilder.RegisterType<LocalidadServicio>().As<ILocalidadServicio>();
            containerBuilder.RegisterType<ConfiguracionServicio>().As<IConfiguracionServicio>();
            containerBuilder.RegisterType<GrupoServicio>().As<IGrupoServicio>();
            containerBuilder.RegisterType<FormularioServicio>().As<IFormularioServicio>();
            containerBuilder.RegisterType<GrupoPersonaServicio>().As<IGrupoPersonaServicio>();
            containerBuilder.RegisterType<GrupoFormularioServicio>().As<IGrupoFormularioServicio>();
            containerBuilder.RegisterType<ModuloServicio>().As<IModuloServicio>();

            // Validaciones
            containerBuilder.RegisterType<EmpresaValidator>().As<IValidator<EmpresaPersistenciaDTO>>();
            containerBuilder.RegisterType<TipoResponsabilidadValidator>().As<IValidator<TipoResponsabilidadPersistenciaDTO>>();
            containerBuilder.RegisterType<LocalidadValidator>().As<IValidator<LocalidadPersistenciaDTO>>();
            containerBuilder.RegisterType<ProvinciaValidator>().As<IValidator<ProvinciaPersistenciaDTO>>();
            containerBuilder.RegisterType<IngresoBrutoValidator>().As<IValidator<IngresoBrutoPersistenciaDTO>>();
            containerBuilder.RegisterType<PersonaValidator>().As<IValidator<PersonaPersistenciaDTO>>();
            containerBuilder.RegisterType<ConfiguracionValidator>().As<IValidator<ConfiguracionPersistenciaDTO>>();
            containerBuilder.RegisterType<GrupoValidator>().As<IValidator<GrupoPersistenciaDTO>>();
            containerBuilder.RegisterType<ModuloValidator>().As<IValidator<ModuloPersistenciaDTO>>();
        }
    }
}
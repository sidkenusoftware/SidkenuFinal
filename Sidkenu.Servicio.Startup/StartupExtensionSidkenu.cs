using Dime.Servicio.Comun.EMail;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sidkenu.Dominio.Repositorio;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Infraestructura;
using Sidkenu.Servicio.DTOs.Core.Caja;
using Sidkenu.Servicio.DTOs.Core.TipoGasto;
using Sidkenu.Servicio.DTOs.Seguridad.AsistenteCargaInicial;
using Sidkenu.Servicio.DTOs.Seguridad.Configuracion;
using Sidkenu.Servicio.DTOs.Seguridad.Empresa;
using Sidkenu.Servicio.DTOs.Seguridad.Grupo;
using Sidkenu.Servicio.DTOs.Seguridad.IngresoBruto;
using Sidkenu.Servicio.DTOs.Seguridad.Localidad;
using Sidkenu.Servicio.DTOs.Seguridad.Modulo;
using Sidkenu.Servicio.DTOs.Seguridad.Persona;
using Sidkenu.Servicio.DTOs.Seguridad.Provincia;
using Sidkenu.Servicio.DTOs.Seguridad.TipoResponsabilidad;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Implementacion.Core;
using Sidkenu.Servicio.Implementacion.Seguridad;
using Sidkenu.Servicio.Interface.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator.Core;
using Sidkenu.Servicio.Validator.Seguridad;
using Sidkenu.Servicio.Validator.Seguridad.Asistente;

namespace Sidkenu.Servicio.Startup
{
    public static class StartupExtensionSidkenu
    {
        public static void AddServicios(this IServiceCollection services)
        {
            services.AddDbContext<DataContext>();
            services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));
            services.AddScoped(typeof(IUnidadDeTrabajo), typeof(UnidadDeTrabajo));

            // Autenticacion
            services.AddScoped<IServicioBase, ServicioBase>();
            services.AddScoped<ICuentaServicio, CuentaServicio>();
            services.AddScoped<IPasswordServicio, PasswordServicio>();
            services.AddScoped<IUsuarioServicio, UsuarioServicio>();
            services.AddScoped<ICorreoElectronico, CorreoElectronico>();
            services.AddScoped<ISeguridadServicio, SeguridadServicio>();

            // Servicios Seguridad
            services.AddScoped<ICuentaServicio, CuentaServicio>();
            services.AddScoped<IPasswordServicio, PasswordServicio>();
            services.AddScoped<IUsuarioServicio, UsuarioServicio>();
            services.AddScoped<IPersonaServicio, PersonaServicio>();
            services.AddScoped<IEmpresaPersonaServicio, EmpresaPersonaServicio>();
            services.AddScoped<IEmpresaServicio, EmpresaServicio>();
            services.AddScoped<IProvinciaServicio, ProvinciaServicio>();
            services.AddScoped<IIngresoBrutoServicio, IngresoBrutoServicio>();
            services.AddScoped<ITipoResponsabilidadServicio, TipoResponsabilidadServicio>();
            services.AddScoped<ILocalidadServicio, LocalidadServicio>();
            services.AddScoped<IConfiguracionServicio, ConfiguracionServicio>();
            services.AddScoped<IGrupoServicio, GrupoServicio>();
            services.AddScoped<IFormularioServicio, FormularioServicio>();
            services.AddScoped<IGrupoPersonaServicio, GrupoPersonaServicio>();
            services.AddScoped<IGrupoFormularioServicio, GrupoFormularioServicio>();
            services.AddScoped<IModuloServicio, ModuloServicio>();

            // Validaciones Seguridad
            services.AddScoped<IValidator<PersonaAsistenteDTO>, AsistentePersonaValidator>();
            services.AddScoped<IValidator<EmpresaAsistenteDTO>, AsistenteEmpresaValidator>();
            services.AddScoped<IValidator<ConfiguracionAsistenteDTO>, AsistenteConfiguracionValidator>();
            services.AddScoped<IValidator<EmpresaPersistenciaDTO>, EmpresaValidator>();
            services.AddScoped<IValidator<TipoResponsabilidadPersistenciaDTO>, TipoResponsabilidadValidator>();
            services.AddScoped<IValidator<LocalidadPersistenciaDTO>, LocalidadValidator>();
            services.AddScoped<IValidator<ProvinciaPersistenciaDTO>, ProvinciaValidator>();
            services.AddScoped<IValidator<IngresoBrutoPersistenciaDTO>, IngresoBrutoValidator>();
            services.AddScoped<IValidator<PersonaPersistenciaDTO>, PersonaValidator>();
            services.AddScoped<IValidator<ConfiguracionPersistenciaDTO>, ConfiguracionValidator>();
            services.AddScoped<IValidator<GrupoPersistenciaDTO>, GrupoValidator>();
            services.AddScoped<IValidator<ModuloPersistenciaDTO>, ModuloValidator>();

            // Servicios Core
            services.AddScoped<ICajaServicio, CajaServicio>();
            services.AddScoped<ITipoGastoServicio, TipoGastoServicio>();

            // Validaciones Core
            services.AddScoped<IValidator<CajaPersistenciaDTO>, CajaValidator>();
            services.AddScoped<IValidator<TipoGastoPersistenciaDTO>, TipoGastoValidator>();
        }
    }
}

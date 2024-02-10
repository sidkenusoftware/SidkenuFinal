using FluentValidation;
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
using Sidkenu.Servicio.Implementacion.Seguridad;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator.Seguridad;
using Sidkenu.Servicio.Validator.Seguridad.Asistente;
using StructureMap;

namespace Sidkenu.Servicio.StructureMapStartupExtension
{
    public class StructureMapRegistrySeguridad : Registry
    {
        public StructureMapRegistrySeguridad()
        {
            // Servicios Seguridad
            For<ICuentaServicio>().Use<CuentaServicio>();
            For<IPasswordServicio>().Use<PasswordServicio>();
            For<IUsuarioServicio>().Use<UsuarioServicio>();
            For<IPersonaServicio>().Use<PersonaServicio>();
            For<IEmpresaPersonaServicio>().Use<EmpresaPersonaServicio>();
            For<IEmpresaServicio>().Use<EmpresaServicio>();
            For<IProvinciaServicio>().Use<ProvinciaServicio>();
            For<IIngresoBrutoServicio>().Use<IngresoBrutoServicio>();
            For<ITipoResponsabilidadServicio>().Use<TipoResponsabilidadServicio>();
            For<ILocalidadServicio>().Use<LocalidadServicio>();
            For<IConfiguracionServicio>().Use<ConfiguracionServicio>();
            For<IGrupoServicio>().Use<GrupoServicio>();
            For<IFormularioServicio>().Use<FormularioServicio>();
            For<IGrupoPersonaServicio>().Use<GrupoPersonaServicio>();
            For<IGrupoFormularioServicio>().Use<GrupoFormularioServicio>();
            For<IModuloServicio>().Use<ModuloServicio>();
            For<IPuestoTrabajoServicio>().Use<PuestoTrabajoServicio>();

            // Validaciones Seguridad
            For<IValidator<PersonaAsistenteDTO>>().Use<AsistentePersonaValidator>();
            For<IValidator<EmpresaAsistenteDTO>>().Use<AsistenteEmpresaValidator>();
            For<IValidator<ConfiguracionAsistenteDTO>>().Use<AsistenteConfiguracionValidator>();
            For<IValidator<EmpresaPersistenciaDTO>>().Use<EmpresaValidator>();
            For<IValidator<TipoResponsabilidadPersistenciaDTO>>().Use<TipoResponsabilidadValidator>();
            For<IValidator<LocalidadPersistenciaDTO>>().Use<LocalidadValidator>();
            For<IValidator<ProvinciaPersistenciaDTO>>().Use<ProvinciaValidator>();
            For<IValidator<IngresoBrutoPersistenciaDTO>>().Use<IngresoBrutoValidator>();
            For<IValidator<PersonaPersistenciaDTO>>().Use<PersonaValidator>();
            For<IValidator<ConfiguracionPersistenciaDTO>>().Use<ConfiguracionValidator>();
            For<IValidator<GrupoPersistenciaDTO>>().Use<GrupoValidator>();
            For<IValidator<ModuloPersistenciaDTO>>().Use<ModuloValidator>();
        }
    }
}

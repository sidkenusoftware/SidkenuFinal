using Dime.Servicio.Comun.EMail;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Aplicacion.CadenaConexion;
using Sidkenu.Dominio.Repositorio;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Infraestructura;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Implementacion.Seguridad;
using Sidkenu.Servicio.Interface.Base;
using Sidkenu.Servicio.Interface.Seguridad;
using StructureMap;

namespace Sidkenu.Servicio.StructureMapStartupExtension
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {

            For<DbContext>().Use(() => new DataContext()).ContainerScoped();

            For<IUnidadDeTrabajo>().Use<UnidadDeTrabajo>();
            For(typeof(IRepositoryGeneric<>)).Use(typeof(RepositoryGeneric<>));

            For<IServicioBase>().Use<ServicioBase>();
            For<ICuentaServicio>().Use<CuentaServicio>();
            For<ICorreoElectronico>().Use<CorreoElectronico>();
            For<IConexionServicio>().Use<ConexionServicio>();
            For<IConectividadServicio>().Use<ConectividadServicio>();
            For<ISeguridadServicio>().Use<SeguridadServicio>();
            For<IAsistenteCargaInicialServicio>().Use<AsistenteCargaInicialServicio>();
        }
    }
}

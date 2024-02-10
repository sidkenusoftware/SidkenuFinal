using Sidkenu.Aplicacion.CadenaConexion.Constantes;

namespace Sidkenu.Aplicacion.CadenaConexion
{
    public interface IConexionServicio
    {
        // Metodo
        string ObtenerCadenaConexion(TipoMotorBaseDatos tipoMotorBase);
    }
}

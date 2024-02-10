using Microsoft.Data.SqlClient;
using Sidkenu.Aplicacion.CadenaConexion;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class ConectividadServicio : IConectividadServicio
    {
        private readonly IConexionServicio _conexionServicio;

        public ConectividadServicio()
        {
            _conexionServicio = new ConexionServicio();
        }

        public bool VerificarSiBaseDatosEstaOperativa()
        {
            switch (MotoBaseDatos.Obtener)
            {
                case Aplicacion.CadenaConexion.Constantes.TipoMotorBaseDatos.SqlServer:
                    {
                        try
                        {
                            SqlConnection sqlConnection = new SqlConnection(
                                _conexionServicio.ObtenerCadenaConexion(
                                    Aplicacion.CadenaConexion.Constantes.TipoMotorBaseDatos.SqlServer));

                            sqlConnection.Open();

                            if (sqlConnection.State == System.Data.ConnectionState.Open)
                            {
                                sqlConnection.Close();
                                return true;
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            return false;
                        }
                    }
                    break;
                case Aplicacion.CadenaConexion.Constantes.TipoMotorBaseDatos.MySql:
                    {
                        try
                        {
                            MySql.Data.MySqlClient.MySqlConnection mySqlConnection = new MySql.Data.MySqlClient.MySqlConnection(
                            _conexionServicio.ObtenerCadenaConexion(
                                Aplicacion.CadenaConexion.Constantes.TipoMotorBaseDatos.SqlServer));

                            mySqlConnection.Open();

                            if (mySqlConnection.State == System.Data.ConnectionState.Open)
                            {
                                mySqlConnection.Close();
                                return true;
                            }
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            return false;
                        }
                    }
                    break;
                default:
                    return false;
            }

            return false;
        }
    }
}

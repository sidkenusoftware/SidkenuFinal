namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IPasswordServicio
    {
        string Hash(string password);

        bool Check(string hash, string password);

        string Generar(int cantidadCaracteres = 10);
    }
}

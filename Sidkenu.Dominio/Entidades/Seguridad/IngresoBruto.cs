using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Seguridad
{
    public class IngresoBruto : EntidadBase
    {
        public string Descripcion { get; set; }

        public List<Empresa> Empresas { get; set; }
    }
}
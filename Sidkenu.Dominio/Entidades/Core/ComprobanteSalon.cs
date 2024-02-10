namespace Sidkenu.Dominio.Entidades.Core
{
    public class ComprobanteSalon : Comprobante
    {
        // Propiedades
        public Guid MesaId { get; set; }        


        // Propiedades de Navegacion
        public virtual Mesa Mesa { get; set; }        
    }
}

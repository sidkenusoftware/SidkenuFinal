namespace Sidkenu.Servicio.DTOs.Core.CajaPuestoTrabajo
{
    public class CajaPuestoTrabajosPersistenciaDTO
    {
        public Guid CajaId { get; set; }

        public List<Guid> PuestoTrabajoIds { get; set; }
    }
}

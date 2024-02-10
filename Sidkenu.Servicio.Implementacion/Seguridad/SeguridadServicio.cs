using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class SeguridadServicio : ISeguridadServicio
    {
        private readonly IUnidadDeTrabajo _unitOfWork;

        public SeguridadServicio(IUnidadDeTrabajo unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool VerificarAcceso(Guid personaId, Guid empresaId, string formulario)
        {
            var result = _unitOfWork.GrupoPersonaRepository
                .GetByFilter(x => !x.EstaEliminado
                                && !x.Grupo.EstaEliminado
                                && x.Grupo.EmpresaId == empresaId
                                && x.PersonaId == personaId
                                && x.Grupo.GrupoFormularios.Where(gf => !gf.EstaEliminado).Any(gf => gf.Formulario.DescripcionCompleta == formulario)
                                , null, i => i.Include(g => g.Grupo).ThenInclude(gp => gp.GrupoFormularios).ThenInclude(f => f.Formulario));

            return result.Any();
        }
    }
}

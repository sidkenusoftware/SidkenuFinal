using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Formulario;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IFormularioServicio
    {
        ResultDTO Add(List<FormularioDTO> formularios, string userLogin);

        ResultDTO GetAll();
    }
}

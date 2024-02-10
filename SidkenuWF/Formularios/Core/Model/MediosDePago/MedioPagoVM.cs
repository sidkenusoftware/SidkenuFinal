using Sidkenu.Aplicacion.Comun;
using Sidkenu.Aplicacion.Constantes;

namespace SidkenuWF.Formularios.Core.Model.MediosDePago
{
    public class MedioPagoVM : BaseVM
    {
        public TipoMedioDePago Tipo { get; set; }

        public string TipoMedioDePagoStr => EnumDescription.Get(Tipo);

        public decimal Capital { get; set; }
        public decimal Interes { get; set; }
    }
}

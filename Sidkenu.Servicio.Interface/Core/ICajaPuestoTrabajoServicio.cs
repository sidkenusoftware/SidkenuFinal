﻿using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.CajaPuestoTrabajo;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface ICajaPuestoTrabajoServicio
    {
        ResultDTO GetByPuestosTrabajosAsignadas(CajaPuestoTrabajoFilterDTO filterDTO);
        ResultDTO GetByPuestosTrabajosNoAsignadas(CajaPuestoTrabajoFilterDTO filterDTO);
        ResultDTO GetByCajasAsignadas(Guid puestoTrabajoId);
        ResultDTO AddPuestoTrabajoCaja(CajaPuestoTrabajoPersistenciaDTO CajaPuestoTrabajoPersistenciaDTO, string userLogin);
        ResultDTO AddPuestosTrabajosCaja(CajaPuestoTrabajosPersistenciaDTO CajaPuestoTrabajosPersistenciaDTO, string userLogin);

        ResultDTO DeleteCajaPuestoTrabajo(CajaPuestoTrabajoPersistenciaDTO CajaPuestoTrabajoPersistenciaDTO, string userLogin);
        ResultDTO DeleteCajaPuestosTrabajos(CajaPuestoTrabajosPersistenciaDTO CajaPuestoTrabajosPersistenciaDTO, string userLogin);
    }
}

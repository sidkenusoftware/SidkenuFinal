﻿using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Usuario;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IUsuarioServicio
    {


        ResultDTO GetByFilter(UsuarioFilterDTO filterUsuarioDTO);

        ResultDTO GetByName(string nomuario);

        ResultDTO GetById(Guid id);

        ResultDTO Crear(UsuarioPersistenciaDTO usuarioPersistenciaDTO, string userLogin);
        ResultDTO Crear(UsuarioPersistenciaDTO usuarioPersistenciaDTO, string password, string userLogin);

        ResultDTO Eliminar(UsuarioDeleteDTO deleteUsuarioDTO, string Login);

        ResultDTO Bloquear(UsuarioBloquearDTO bloquearUsuarioDTO, string userLogin);

        ResultDTO CambiarPassword(UsuarioCambioPasswordDTO dto, string userLogin);

        bool VerificarSiExiste(string usuario);

        bool VerificarSiExiste(Guid personaId);

        string CrearNombreUsuario(string apellido, string nombre);

        ResultDTO ResetPassword(UsuarioResetPasswordDTO resetPasswordDTO, string userLogin);
    }
}

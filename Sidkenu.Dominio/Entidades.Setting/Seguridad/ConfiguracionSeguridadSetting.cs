using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Seguridad
{
    public class ConfiguracionSeguridadSetting : EntidadBaseSetting, Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<ConfiguracionSeguridad>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionSeguridad> builder)
        {
            // Propiedades
            builder.Property(x => x.EmpresaId)
                .IsRequired();

            builder.Property(x => x.LoginNormal)
                .IsRequired();

            builder.Property(x => x.UsuarioCredencial)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.PasswordCredencial)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.Puerto)
                .IsRequired();

            builder.Property(x => x.Host)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.LogError)
                .IsRequired();

            builder.Property(x => x.LogWarning)
                .IsRequired();

            builder.Property(x => x.LogInformacion)
                .IsRequired();

            builder.Property(x => x.EnviarMailCreateUsuario)
                .IsRequired();
        }
    }
}

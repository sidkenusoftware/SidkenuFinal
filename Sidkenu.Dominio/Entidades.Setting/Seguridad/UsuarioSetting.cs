using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Seguridad
{
    public class UsuarioSetting : EntidadBaseSetting, Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // Propiedades
            builder.Property(x => x.Nombre)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Password)
                .HasMaxLength(400)
                .IsRequired();

            // Propiedades de Navegacion
            builder.HasOne(x => x.Persona)
                .WithMany(x => x.Usuarios)
                .HasForeignKey(x => x.PersonaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

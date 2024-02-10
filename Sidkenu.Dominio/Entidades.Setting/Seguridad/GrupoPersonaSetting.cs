using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Seguridad
{
    public class GrupoPersonaSetting : EntidadBaseSetting, Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<GrupoPersona>
    {
        public void Configure(EntityTypeBuilder<GrupoPersona> builder)
        {
            // Propiedades
            builder.Property(x => x.GrupoId)
                .IsRequired();

            builder.Property(x => x.PersonaId)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Grupo)
                .WithMany(x => x.GrupoPersonas)
                .HasForeignKey(x => x.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Persona)
                .WithMany(x => x.GrupoPersonas)
                .HasForeignKey(x => x.PersonaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

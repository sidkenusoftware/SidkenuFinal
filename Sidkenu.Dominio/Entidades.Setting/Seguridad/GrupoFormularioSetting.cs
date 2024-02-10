using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Seguridad
{
    public class GrupoFormularioSetting : EntidadBaseSetting, Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<GrupoFormulario>
    {
        public void Configure(EntityTypeBuilder<GrupoFormulario> builder)
        {
            // Propiedades
            builder.Property(x => x.GrupoId)
                .IsRequired();

            builder.Property(x => x.FormularioId)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Grupo)
                .WithMany(x => x.GrupoFormularios)
                .HasForeignKey(x => x.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Formulario)
                .WithMany(x => x.GruposFormularios)
                .HasForeignKey(x => x.FormularioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

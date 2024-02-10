using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Seguridad
{
    public class GrupoSetting : EntidadBaseSetting, Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            // Propiedades
            builder.Property(x => x.EmpresaId)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.PorDefecto)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.Grupos)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Propiedades de Navegacion

            builder.HasMany(x => x.GrupoFormularios)
                .WithOne(x => x.Grupo)
                .HasForeignKey(x => x.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

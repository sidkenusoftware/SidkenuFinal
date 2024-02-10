using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Seguridad
{
    public class TipoResponsabilidadSetting : EntidadBaseSetting, Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<TipoResponsabilidad>
    {
        public void Configure(EntityTypeBuilder<TipoResponsabilidad> builder)
        {
            // Propiedades

            builder.Property(x => x.Codigo)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasMany(x => x.Empresas)
                .WithOne(x => x.TipoResponsabilidad)
                .HasForeignKey(x => x.TipoResponsabilidadId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Proveedores)
                .WithOne(x => x.TipoResponsabilidad)
                .HasForeignKey(x => x.TipoResponsabilidadId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Clientes)
                .WithOne(x => x.TipoResponsabilidad)
                .HasForeignKey(x => x.TipoResponsabilidadId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

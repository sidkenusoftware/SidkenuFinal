using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Seguridad
{
    public class ProvinciaSetting : EntidadBaseSetting, Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Provincia>
    {
        public void Configure(EntityTypeBuilder<Provincia> builder)
        {
            // Propiedades

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasMany(x => x.Localidades)
                .WithOne(x => x.Provincia)
                .HasForeignKey(x => x.ProvinciaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

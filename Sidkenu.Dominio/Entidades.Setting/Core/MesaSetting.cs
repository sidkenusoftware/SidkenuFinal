using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class MesaSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Mesa>
    {
        public void Configure(EntityTypeBuilder<Mesa> builder)
        {
            // Propiedades

            builder.Property(x => x.SalonId)
                .IsRequired();

            builder.Property(x => x.Codigo)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            // Propiedades de Navegacion
            builder.HasOne(x => x.Salon)
                .WithMany(x => x.Mesas)
                .HasForeignKey(x => x.SalonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
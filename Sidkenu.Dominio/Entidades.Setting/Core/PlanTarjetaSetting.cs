using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class PlanTarjetaSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<PlanTarjeta>
    {
        public void Configure(EntityTypeBuilder<PlanTarjeta> builder)
        {
            // Propiedades

            builder.Property(x => x.TarjetaId)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.Alicuota).HasPrecision(18, 6)
                .IsRequired();

            // Propiedades de Navegacion
            builder.HasOne(x => x.Tarjeta)
                .WithMany(x => x.PlanesTarjetas)
                .HasForeignKey(x => x.TarjetaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class MovimientoCajaSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<MovimientoCaja>
    {
        public void Configure(EntityTypeBuilder<MovimientoCaja> builder)
        {
            // Propiedades

            builder.Property(x => x.CajaDetalleId)
                .IsRequired();

            builder.Property(x => x.Fecha)
            .IsRequired();

            builder.Property(x => x.Capital).HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.Interes).HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.TipoMovimiento)
                .IsRequired();

            builder.Property(x => x.TipoOperacion)
               .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            // Propiedades de Navegacion
            builder.HasOne(x => x.CajaDetalle)
                .WithMany(x => x.Movimientos)
                .HasForeignKey(x => x.CajaDetalleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

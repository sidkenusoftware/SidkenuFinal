using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class CuentaCorrienteProveedorSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<CuentaCorrienteProveedor>
    {
        public void Configure(EntityTypeBuilder<CuentaCorrienteProveedor> builder)
        {
            // Propiedades

            builder.Property(x => x.ComprobanteCompraId)
                .IsRequired();

            builder.Property(x => x.Fecha)
                .IsRequired();

            builder.Property(x => x.FechaVencimiento)
                .IsRequired(false);

            builder.Property(x => x.TipoMovimiento)
                .IsRequired();

            builder.Property(x => x.Monto).HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .IsRequired(false);

            // Propiedades de Navegacion

            builder.HasOne(x => x.ComprobanteCompra)
                .WithMany(x => x.CuentaCorrienteProveedores)
                .HasForeignKey(x => x.ComprobanteCompraId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}